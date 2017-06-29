#include <stdio.h>
#include <cstring>
#include <string.h> 
#include <unistd.h>
#include <core/logger.h>
#include <core/common-help.h>
#include <cstdlib>

//输出16进制内存数据
void hexdump(void *data, unsigned int len)
{
	unsigned int i;
	unsigned int r, c;
	char szTmp[4096] = { 0x00 };

	if (!data) return;
	if (len >= 4096) return;

	for (r = 0, i = 0; r < (len / 16 + (len % 16 != 0)); r++, i += 16)
	{
		sprintf(szTmp, "%08x:   ", (int)data+i); /* location of first byte in line */

		for (c = i; c < i + 16; c++) /* left half of hex dump */
			if (c < len)
				sprintf(&szTmp[strlen(szTmp)], "%02X ", ((unsigned char *)data)[c]);
			else
				sprintf(&szTmp[strlen(szTmp)], "   "); /* pad if short line */

		for (c = i; c < i + 16; c++) /* ASCII dump */
			if (c < len)
				if (((unsigned char *)data)[c] >= 32 && ((unsigned char *)data)[c] < 127)
					sprintf(&szTmp[strlen(szTmp)], "%c", ((char const *)data)[c]);
				else
					sprintf(&szTmp[strlen(szTmp)], "."); /* put this for non-printables */

		LOGD("%s", szTmp);
		memset(szTmp, 0x00, 4096);
	}
}

void splitstring(char *str, char split, std::vector<char *> &ret)
{
	bool found = false;
	char *last = str;
	while (true)
	{
		if (*str==0x00)
		{
			ret.push_back(last);
			break;
		}

		if (found)
		{
			ret.push_back(last);
			last = str;
		}

		if (*str==split)
		{
			*str = 0x00;
			found = true;
		}
		else
		{
			found = false;
		}

		str++;
	}
}


void get_self_process_name(char* name, int len)
{
	char sztmp[256] = { 0x00 };
	snprintf(sztmp, 256, "/proc/%d/cmdline", getpid());

	FILE *fp = fopen(sztmp, "r");
	if (fp==NULL)
	{
		LOGD("fopen %s failed", sztmp);
		return;
	}

	fgets(name, len, fp);

	fclose(fp);
}

void get_module_range(char* module_name, unsigned int &start, unsigned int &end)
{
	FILE *fp;
	char *pch;
	char filename[32];
	char line[1024];

	snprintf(filename, sizeof(filename), "/proc/self/maps");

	fp = fopen(filename, "r");

	if (fp != NULL)
	{
		start = 0xffffffff, end = 0;
		unsigned int istart = 0, iend = 0;
		while (!feof(fp))
		{
			fscanf(fp, "%08x-%08x %[^'\n']", &istart, &iend, line);
			if (strstr(line, module_name) != NULL)
			{
				if (iend > end)
					end = iend;
				if (istart < start)
					start = istart;
			}
		}
		fclose(fp);
	}
}

void* get_module_base(pid_t pid, const char* module_name)
{
	FILE *fp;
	long addr = 0;
	char *pch;
	char filename[32];
	char line[1024];

	if (pid < 0) {
		/* self process */
		snprintf(filename, sizeof(filename), "/proc/self/maps");
	}
	else {
		snprintf(filename, sizeof(filename), "/proc/%d/maps", pid);
	}

	fp = fopen(filename, "r");

	if (fp != NULL) {
		while (fgets(line, sizeof(line), fp)) {
			if (strstr(line, module_name)) {
				pch = strtok(line, "-");
				addr = strtoul(pch, NULL, 16);

				if (addr == 0x8000)
					addr = 0;

				break;
			}
		}

		fclose(fp);
	}

	return (void *)addr;
}


int read_int32_little(char *buff, int idx)
{
	return *(int *)(buff + idx);
}

int read_int32_big(char *buff, int idx)
{
	char int_bytes[4] = { 0x00 };
	int_bytes[0] = buff[idx + 3];
	int_bytes[1] = buff[idx + 2];
	int_bytes[2] = buff[idx + 1];
	int_bytes[3] = buff[idx + 0];
	return *(int *)int_bytes;
}

void write_int32_little(char *buff, int idx, int v)
{
	*(int *)&buff[idx] = v;
}

void write_int32_big(char *buff, int idx, int v)
{
	char *p_int_bytes = (char *)&v;
	buff[idx] = p_int_bytes[3];
	buff[idx + 1] = p_int_bytes[2];
	buff[idx + 2] = p_int_bytes[1];
	buff[idx + 3] = p_int_bytes[0];
}

short read_int16_little(char *buff, int idx)
{
	return *(short *)(buff + idx);
}

short read_int16_big(char *buff, int idx)
{
	char short_bytes[2] = { 0x00 };
	short_bytes[0] = buff[idx + 1];
	short_bytes[1] = buff[idx];
	return *(short *)short_bytes;
}

void write_int16_little(char *buff, int idx, short v)
{
	*(short *)&buff[idx] = v;
}

void write_int16_big(char *buff, int idx, short v)
{
	char *p_bytes = (char *)&v;
	buff[idx] = p_bytes[1];
	buff[idx + 1] = p_bytes[0];
}

int64_t read_int64_little(char *buff, int idx)
{
	return *(int64_t *)(buff + idx);
}

int64_t read_int64_big(char *buff, int idx)
{
	char p_bytes[8] = { 0x00 };
	for (int i=0; i<8; i++)
	{
		p_bytes[i] = buff[idx + 7 - i];
	}
	return *(int64_t *)p_bytes;
}

void write_int64_little(char *buff, int idx, int64_t v)
{
	*(int64_t *)&buff[idx] = v;
}

void write_int64_big(char *buff, int idx, int64_t v)
{
	char *p_bytes = (char *)&v;
	for (int i = 0; i < 8; i++)
	{
		buff[idx + 7 - i] = p_bytes[i];
	}
}


int find_sub_char(char *src, int src_len, char *sub, int sub_len)
{
	for (int i = 0; i != src_len - sub_len; i++)
	{
		if (src[i] == sub[0])
		{
			if (memcmp(src + i, sub, sub_len) == 0)
			{
				return i;
			}
		}
	}
	return -1;
}