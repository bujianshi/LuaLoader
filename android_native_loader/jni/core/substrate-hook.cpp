#include <core/substrate-hook.h>
#include <core/common-help.h>
#include <cstdio>
#include <dlfcn.h>

bool substrate_hook(void *src, void *dest, void **old)
{
	void *p_lib = dlopen("/data/local/tmp/libsubstrate.so", RTLD_LAZY);
	if (p_lib)
	{
		void(*mshook)(void *, void *, void **) = (void(*)(void *, void *, void **))dlsym(p_lib, "MSHookFunction");
		if (mshook)
		{
			mshook(src, dest, old);
			dlclose(p_lib);
			return true;
		}
		
		dlclose(p_lib);
		return false;
	}

	return false;
}
