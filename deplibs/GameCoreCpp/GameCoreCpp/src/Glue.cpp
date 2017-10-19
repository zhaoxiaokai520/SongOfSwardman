#include "Glue.h"
#include<iostream>
#include<string>

template<class Glue> Glue* Singleton<Glue>::m_pInstance = NULL;

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
#ifdef __cplusplus
}
#endif

void Glue::AddCB(int code, CallBack cb)
{
	m_callbackMap.insert(std::make_pair(code, cb));
}

void Glue::RmvCB(int code)
{
	m_callbackMap.erase(code);
}

void Glue::UpdateNativeImpl(int turnLength)
{
	CallBack cb = m_callbackMap.at(0);
	if (NULL != cb)
	{
		cb("cpp update called!!");
	}
	//std::map<int, CallBack>::iterator it = m_callbackMap.begin;

	//if (it != m_callbackMap.end())
	//{
	//	it++;
	//}
}