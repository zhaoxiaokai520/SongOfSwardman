#include "Glue.h"

#ifdef __cplusplus
extern "C" {
#endif

	DLL void AddCallback(int code, CallBack cb)
	{
		//std::thread t1(&myThread, this);//创建一个分支线程，回调到myThread函数里
		//t1.join();
        Glue::Instance()->AddCallback(code, cb);
	}
	DLL void RmvCallback(int code, CallBack cb)
	{
        Glue::Instance()->RmvCallback(code, cb);
	}

	DLL void UpdateNative(int turnLength)
	{
        Glue::Instance()->UpdateNative(turnLength);
	}
#ifdef __cplusplus
}
#endif

void Glue::AddCallback(int code, CallBack cb)
{

}

void Glue::RmvCallback(int code, CallBack cb)
{

}

void Glue::UpdateNative(int turnLength)
{

}