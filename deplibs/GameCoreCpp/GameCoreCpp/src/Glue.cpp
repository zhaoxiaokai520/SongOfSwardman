#include "Glue.h"
#include<iostream>
#include<string>
#include <chrono>
#include <assert.h>
using namespace std;
using namespace chrono;

template<class Glue> Glue* Singleton<Glue>::m_pInstance = NULL;
system_clock::time_point baseTime = system_clock::now();

#ifdef __cplusplus
extern "C" {
#endif

	DLL void AddCallback(int code, CallBack cb)
	{
		//std::thread t1(&myThread, this);//创建一个分支线程，回调到myThread函数里
		//t1.join();
        Glue::Instance()->AddCB(code, cb);
	}
	DLL void RmvCallback(int code)
	{
        Glue::Instance()->RmvCB(code);
	}

	DLL void UpdateNative(int turnLength)
	{
        Glue::Instance()->UpdateNativeImpl(turnLength);
	}

    DLL void OnTimerNative()
    {
        Glue::Instance()->OnTimerNativeImpl();
    }

    DLL __int64 GetSystemClock()
    {
        return Glue::Instance()->GetSystemClock();
    }

    DLL void LoadBattleNative()
    {
        //assert(false);
        return Glue::Instance()->LoadBattleNative();
    }

    DLL void UnloadBattleNative()
    {
        //assert(false);
        return Glue::Instance()->UnloadBattleNative();
    }
#ifdef __cplusplus
}
#endif

void Glue::AddCB(int code, CallBack cb)
{
	m_callbackMap.insert(std::make_pair(code, cb));
}

void StartTimer()
{
    //std::chrono::steady_clock::time_point t1, t2;
    //std::thread([this, interval, task]() {
    //    while (!try_to_expire_) {
    //        std::this_thread::sleep_for(std::chrono::milliseconds(interval));
    //        task();
    //    }
    //    //          std::cout << "stop task..." << std::endl;
    //    {
    //        std::lock_guard<std::mutex> locker(mutex_);
    //        expired_ = true;
    //        expired_cond_.notify_one();
    //    }
    //}).detach();
}

void Glue::RmvCB(int code)
{
	m_callbackMap.erase(code);
}

void Glue::UpdateNativeImpl(int turnLength)
{
    InvokCallback(0, "===cpp update called!!===");
	//std::map<int, CallBack>::iterator it = m_callbackMap.begin;

	//if (it != m_callbackMap.end())
	//{
	//	it++;
	//}
}

void Glue::OnTimerNativeImpl()
{
    InvokCallback(0, "===cpp per sec timer called!!===");
}

__int64 Glue::GetSystemClock()
{
    auto time = std::chrono::system_clock::now();
    
    return std::chrono::duration_cast<std::chrono::microseconds>(time- baseTime).count();
}

//************************************
// load roles, maps
//************************************
void Glue::LoadBattleNative()
{
    InvokCallback(1, "===cpp LoadBattleNative called!!===");
}

//************************************
// unload roles, maps, release memory
//************************************
void Glue::UnloadBattleNative()
{
    InvokCallback(1, "===cpp UnloadBattleNative called!!===");
}

void Glue::InvokCallback(int protocolCode, const char *data)
{
    if (m_callbackMap.find(protocolCode) != m_callbackMap.end())
    {
        CallBack cb = m_callbackMap.at(protocolCode);
        if (nullptr != cb)
        {
            cb(data);
        }
    }
}