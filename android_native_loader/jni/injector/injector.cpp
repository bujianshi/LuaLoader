#include <stdio.h> 
#include <string.h>
#include <unistd.h>
#include "android-injector.h"

#define TICK_PROCESS 200000
#define TICK_RELY 20000

int main(int argc, char* argv[])
{
	if (argc<4)
	{
		printf("useage: %s proc_name libneed liblua\n\n", argv[0]);
		return 0;
	}

	if (access("/data/local/tmp/libxgame.so", F_OK) == -1)
	{
		printf("failed access: /data/local/tmp/libxgame.so\n\n");
		return 0;
	}

	ANDROID_INJECTOR injector;
	pid_t target_pid = -1;
	do
	{
		target_pid = injector.find_pid_of(argv[1]);
		usleep(TICK_PROCESS);
		printf("wait for %s...\n", argv[1]);
	} while (target_pid == -1);

	do
	{
		usleep(TICK_RELY);
	} while (!injector.find_injected_so_of(target_pid, argv[2]));
	usleep(TICK_RELY);

	int ret = injector.inject_remote_process(target_pid, "/data/local/tmp/libxgame.so", "entry", argv[3], strlen(argv[3]), 0);
	if (ret==-1)
	{
		printf("inject failed!\n\n");
		return 0;
	}
	
	printf("inject success!\n\n");
	return 1;
}
