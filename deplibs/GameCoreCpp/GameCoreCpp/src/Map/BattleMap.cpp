#include "BattleMap.h"
#include<iostream>
#include<string>
#include <chrono>
using namespace std;
using namespace chrono;
//
//template<class Glue> Glue* Singleton<Glue>::m_pInstance = NULL;
//system_clock::time_point baseTime = system_clock::now();

#ifdef __cplusplus
extern "C" {
#endif

	//DLL void AddCallback(int code, CallBack cb)
	//{
	//	//std::thread t1(&myThread, this);//创建一个分支线程，回调到myThread函数里
	//	//t1.join();
 //       Glue::Instance()->AddCB(code, cb);
	//}
	//DLL void RmvCallback(int code)
	//{
 //       Glue::Instance()->RmvCB(code);
	//}

	//DLL void UpdateNative(int turnLength)
	//{
 //       Glue::Instance()->UpdateNativeImpl(turnLength);
	//}

 //   DLL void OnTimerNative()
 //   {
 //       Glue::Instance()->OnTimerNativeImpl();
 //   }

 //   DLL float GetSystemClock()
 //   {
 //       return Glue::Instance()->GetSystemClock();
 //   }
#ifdef __cplusplus
}
#endif