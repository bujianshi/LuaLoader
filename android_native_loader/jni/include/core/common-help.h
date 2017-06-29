#pragma once
#include <vector>

void hexdump(void *data, unsigned int len);
void splitstring(char *str, char split, std::vector<char *> &ret);
void get_self_process_name(char* name, int len);
void* get_module_base(pid_t pid, const char* module_name);
void get_module_range(char* module_name, unsigned int &start, unsigned int &end);
int read_int32_little(char *buff, int idx);
int read_int32_big(char *buff, int idx);
void write_int32_little(char *buff, int idx, int v);
void write_int32_big(char *buff, int idx, int v);
short read_int16_little(char *buff, int idx);
short read_int16_big(char *buff, int idx);
void write_int16_little(char *buff, int idx, short v);
void write_int16_big(char *buff, int idx, short v);
int64_t read_int64_little(char *buff, int idx);
int64_t read_int64_big(char *buff, int idx);
void write_int64_little(char *buff, int idx, int64_t v);
void write_int64_big(char *buff, int idx, int64_t v);
int find_sub_char(char *src, int src_len, char *sub, int sub_len);