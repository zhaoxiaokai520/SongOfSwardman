#ifndef INCLUDED_BATTLE_MAP
#define INCLUDED_BATTLE_MAP

#include "Defines/types.h"
//#include "Singleton.h"
#include <map>
#include <iostream>

//typedef void(*CallBack)(const char * data);

#ifdef __cplusplus
extern "C" {
#endif

    //extern DLL void AddCallback(int code, CallBack cb);
    //extern DLL void RmvCallback(int code);
    //extern DLL void UpdateNative(int turnLength);
    //extern DLL float GetSystemClock();

#ifdef __cplusplus
}
#endif

class BattleMap
{
public:
    //void AddCB(int code, CallBack cb);
    //void RmvCB(int code);
    //void UpdateNativeImpl(int turnLength);
    //void OnTimerNativeImpl();
    //float GetSystemClock();
	
private:
	//std::map<int,CallBack> m_callbackMap;
};

#endif //INCLUDED_BATTLE_MAP
