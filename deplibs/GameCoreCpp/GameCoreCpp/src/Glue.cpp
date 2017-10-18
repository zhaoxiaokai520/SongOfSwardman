#include "Glue.h"

#ifdef __cplusplus
extern "C" {
#endif

	DLL void AddCallback(int code, CallBack cb)
	{
		//std::thread t1(&myThread, this);//创建一个分支线程，回调到myThread函数里
		//t1.join();
		//assert(false);
	}
	DLL void RmvCallback(int code, CallBack cb)
	{
		//assert(false);
	}

	DLL void UpdateNative(int turnLength)
	{
		//assert(false);
	}
#ifdef __cplusplus
}
#endif
