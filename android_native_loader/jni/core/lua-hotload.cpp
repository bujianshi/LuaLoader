#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <sys/socket.h>
#include <sys/types.h>
#include <netinet/in.h>
#include <event.h>
#include <event2/listener.h>
#include <core/logger.h>
#include <pthread.h>
#include <dlfcn.h>
#include <core/substrate-hook.h>
#include <json/json.h>

#define PORT        5566
#define BACKLOG     1
#define MAGIC_NUM	0xfefe
#define HEAD_LEN	0x6
#define LUA_MASKCALL	1
#define LUA_GLOBALSINDEX	-10002

static bufferevent *client = NULL;
static char send_buff[1024 * 1024 * 64] = { 0x00 };
static unsigned int recved_len = 0;
static char recv_buff[1024 * 1024 * 64] = { 0x00 };
static pthread_mutex_t mutex_loaded_chunk;
static std::vector<std::string> v_loaded_chunk;
static pthread_mutex_t mutex_reload_chunk;
static std::vector<Json::Value> v_reload_chunk;

static int(*lua_sethook)(int, void*, int, int) = NULL;
static void(*lua_pushcclosure)(int, void*, int) = NULL;
static void(*lua_setfield)(int, int, const char*) = NULL;
static void(*lua_getfield)(int, int, const char*) = NULL;
static int(*lua_gettop)(int) = NULL;
static int(*lua_settop)(int, int) = NULL;
static void(*lua_pushvalue)(int, int) = NULL;
static int(*lua_pcall)(int, int, int, int) = NULL;
static char*(*lua_tolstring)(int, int, int*) = NULL;
static int*(*luaL_loadbufferx)(int, char*, int, char*, char*) = NULL;
static int*(*lua_newstate)(int, int) = NULL;
static void*(*lua_setglobal)(int, const char*) = NULL;
static void*(*lua_getglobal)(int, const char*) = NULL;

static void send_chunk(std::string jsonStr)
{
	if (client != NULL)
	{
		unsigned int data_len = jsonStr.length();
		unsigned int send_len = data_len + HEAD_LEN;
		*(unsigned short *)(send_buff) = MAGIC_NUM;
		*(unsigned int *)(send_buff + 2) = data_len;
		memcpy(send_buff + HEAD_LEN, jsonStr.c_str(), data_len);

		bufferevent_write(client, send_buff, send_len);
	}
}

static void route_package()
{
	Json::Reader reader;
	Json::Value jvalue;
	std::string str;

	recv_buff[recved_len] = 0x00;
	str.append((char *)&recv_buff[HEAD_LEN]);
	if (reader.parse(str, jvalue, false))
	{
		pthread_mutex_lock(&mutex_reload_chunk);
		v_reload_chunk.push_back(jvalue);
		pthread_mutex_unlock(&mutex_reload_chunk);
	}
}

static void read_package(char *buff, int pos, int len)
{
	if (len==0)
	{
		return;
	}

	if (recved_len<HEAD_LEN)
	{
		int headNeed = HEAD_LEN - recved_len;
		if (len<headNeed)
		{
			memcpy(recv_buff + recved_len, buff + pos, len);
			recved_len += len;
			return;
		}

		memcpy(recv_buff + recved_len, buff + pos, headNeed);
		pos += headNeed;
		len -= headNeed;
		recved_len += headNeed;

		if (*(unsigned short *)recv_buff!=MAGIC_NUM)
		{
			recved_len = 0;
			return;
		}

		read_package(buff, pos, len);
	}
	else
	{
		int bodyNeed = *(unsigned int *)(recv_buff + 2) + HEAD_LEN - recved_len;
		if (len>=bodyNeed)
		{
			memcpy(recv_buff + recved_len, buff + pos, bodyNeed);
			recved_len += bodyNeed;
			pos += bodyNeed;
			len -= bodyNeed;
			route_package();
			recved_len = 0;

			read_package(buff, pos, len);
		}
		else
		{
			memcpy(recv_buff + recved_len, buff + pos, len);
			recved_len += len;
		}
	}
}

static void socket_read_cb(bufferevent *bev, void *arg)
{
	char sztmp[4096] = { 0x00 };
	int len = bufferevent_read(bev, sztmp, 4096);
	read_package(sztmp, 0, len);
}

static void socket_event_cb(bufferevent *bev, short events, void *arg)
{
	if (events & BEV_EVENT_EOF)
	{
		LOGD("connection closed...\n");
	}
	else
	{
		LOGD("libevent error...\n");
	}

	client = NULL;
	bufferevent_free(bev);
}

static void listener_cb(evconnlistener *listener, evutil_socket_t s, sockaddr *sin, int socklen, void *arg)
{
	LOGD("new client come...");
	event_base *base = (event_base *)arg;
	bufferevent *bev = bufferevent_socket_new(base, s, BEV_OPT_CLOSE_ON_FREE);
	bufferevent_setcb(bev, socket_read_cb, NULL, socket_event_cb, NULL);
	bufferevent_enable(bev, EV_READ | EV_PERSIST);
	client = bev;

	pthread_mutex_lock(&mutex_loaded_chunk);
	for (int i = 0; i < v_loaded_chunk.size(); i++)
	{
		send_chunk(v_loaded_chunk[i]);
	}
	pthread_mutex_unlock(&mutex_loaded_chunk);
}

static void* work_server(void *param)
{
	struct sockaddr_in sin;
	memset(&sin, 0, sizeof(sin));
	sin.sin_family = AF_INET;
	sin.sin_port = htons(PORT);
	sin.sin_addr.s_addr = INADDR_ANY;

	event_base *base = event_base_new();
	evconnlistener *listener = evconnlistener_new_bind(base, listener_cb, base, LEV_OPT_REUSEABLE | LEV_OPT_CLOSE_ON_FREE, BACKLOG, (sockaddr *)&sin, sizeof(sockaddr_in));
	event_base_dispatch(base);
	evconnlistener_free(listener);
	event_base_free(base);
	return NULL;
}

static int(*old_lua_loadbufferx)(int l, char *buff, int size, char *chunk_name, char *mode) = 0;
static int my_lua_loadbufferx(int l, char *buff, int size, char *chunk_name, char *mode)
{
	Json::Value json;
	Json::FastWriter writer;
	json["l"] = (unsigned int)l;
	json["chunk_name"] = chunk_name;
	json["chunk_content"] = std::string(buff, 0, size);
	std::string jsonStr = writer.write(json);

	send_chunk(jsonStr);

	pthread_mutex_lock(&mutex_loaded_chunk);
	v_loaded_chunk.push_back(jsonStr);
	pthread_mutex_unlock(&mutex_loaded_chunk);

	return old_lua_loadbufferx(l, buff, size, chunk_name, mode);
}

static int lua_logd(int l) 
{
	int n = lua_gettop(l);

	if (lua_getglobal != NULL)
		lua_getglobal(l, "tostring");
	else
		lua_getfield(l, LUA_GLOBALSINDEX, "tostring");

	for (int i = 1; i <= n; i++) 
	{
		const char *s;
		int len;
		lua_pushvalue(l, -1);  /* function to be called */
		lua_pushvalue(l, i);   /* value to print */
		lua_pcall(l, 1, 1, 0);
		s = lua_tolstring(l, -1, &len);  /* get result */
		if (s != NULL)
		{
			LOGD("%s", s);
			lua_settop(l, -2);
		}
	}
	return 0;
}

static void lua_hook(int l, int ar)
{
	pthread_mutex_lock(&mutex_reload_chunk);
	for (std::vector<Json::Value>::iterator itr = v_reload_chunk.begin(); itr != v_reload_chunk.end(); )
	{
		Json::Value jvalue = *itr;
		if (jvalue["l"].asUInt() == (unsigned int)l)
		{
			char *chunk_name = (char *)jvalue["chunk_name"].asCString();
			char *chunk_content = (char *)jvalue["chunk_content"].asCString();

			LOGD("reload %s......", chunk_name);

			int n = lua_gettop(l);
			my_lua_loadbufferx(l, chunk_content, strlen(chunk_content), chunk_name, NULL);
			lua_pcall(l, 0, 0, 0);
			lua_settop(l, n);
			itr = v_reload_chunk.erase(itr);
		}
		else
		{
			itr++;
		}
	}
	pthread_mutex_unlock(&mutex_reload_chunk);
}

static int (*old_lua_newstate)(int a1, int a2);
static int my_lua_newstate(int a1, int a2)
{
	int l = old_lua_newstate(a1, a2);

	if (lua_setglobal)
	{
		lua_pushcclosure(l, (void *)lua_logd, 0);
		lua_setglobal(l, "logd");
	}
	else
	{
		lua_pushcclosure(l, (void *)lua_logd, 0);
		lua_setfield(l, LUA_GLOBALSINDEX, "logd");
	}

	lua_sethook(l, (void *)lua_hook, LUA_MASKCALL, 0);

	return l;
}

static void init_lua_hotload(char *luaso)
{
	bool init_ok = false;
	void *plib = dlopen(luaso, RTLD_LAZY);
	if (plib != NULL)
	{
		lua_sethook = (int(*)(int, void*, int, int))dlsym(plib, "lua_sethook");
		lua_pushcclosure = (void(*)(int, void*, int))dlsym(plib, "lua_pushcclosure");
		lua_setfield = (void(*)(int, int, const char*))dlsym(plib, "lua_setfield");
		lua_getfield = (void(*)(int, int, const char*))dlsym(plib, "lua_getfield");
		lua_gettop = (int(*)(int))dlsym(plib, "lua_gettop");
		lua_settop = (int(*)(int, int))dlsym(plib, "lua_settop");
		lua_pushvalue = (void(*)(int, int))dlsym(plib, "lua_pushvalue");
		lua_pcall = (int(*)(int, int, int, int))dlsym(plib, "lua_pcall");
		lua_tolstring = (char*(*)(int, int, int*))dlsym(plib, "lua_tolstring");
		luaL_loadbufferx = (int*(*)(int, char*, int, char*, char *))dlsym(plib, "luaL_loadbufferx");
		lua_newstate = (int*(*)(int, int))dlsym(plib, "lua_newstate");
		// not necessary
		lua_setglobal = (void*(*)(int, const char*))dlsym(plib, "lua_setglobal");
		lua_getglobal = (void*(*)(int, const char*))dlsym(plib, "lua_getglobal");

		init_ok = lua_sethook != NULL &&
			lua_pushcclosure != NULL &&
			lua_setfield != NULL &&
			lua_getfield != NULL &&
			lua_gettop != NULL &&
			lua_settop != NULL &&
			lua_pushvalue != NULL &&
			lua_pcall != NULL &&
			lua_tolstring != NULL &&
			luaL_loadbufferx != NULL &&
			lua_newstate != NULL;
	}

	if (init_ok)
	{
		LOGD("hot lua init ok...");

		pthread_t tid;
		pthread_create(&tid, NULL, work_server, NULL);

		pthread_mutex_init(&mutex_loaded_chunk, NULL);
		pthread_mutex_init(&mutex_reload_chunk, NULL);

		substrate_hook((void *)luaL_loadbufferx, (void *)my_lua_loadbufferx, (void **)&old_lua_loadbufferx);
		substrate_hook((void *)lua_newstate, (void *)my_lua_newstate, (void **)&old_lua_newstate);
	}
	else
	{
		LOGD("hot lua init faield...");
	}
}


extern "C" void entry(char *param)
{
	LOGD("entry...");

	init_lua_hotload(param);
}