#ifndef INCLUDED_GLUE
#define INCLUDED_GLUE

#include "Defines/types.h"
#include "Singleton.h"
#include <map>
#include <iostream>

typedef void(*CallBack)(const char * data);
class MapMgr;

#ifdef __cplusplus
extern "C" {
#endif
	
	//public begin
	extern DLL void AddCallback(int code, CallBack cb);
	extern DLL void RmvCallback(int code);
	extern DLL void UpdateNative(int turnLength);
    extern DLL __int64 GetSystemClock();
	//public end

	//battle begin
    extern DLL void LoadBattleNative();
    extern DLL void UnloadBattleNative();
	extern DLL void SetBattleMapNative(const char *mapPath);
	//battle end

#ifdef __cplusplus
}
#endif

class Glue : public Singleton<Glue>
{
public:
	Glue();
	~Glue();
    void AddCB(int code, CallBack cb);
    void RmvCB(int code);
    void UpdateNativeImpl(int turnLength);
    void OnTimerNativeImpl();
    __int64 GetSystemClock();
    void LoadBattle();
    void UnloadBattle();
	void SetBattleMap(const char *mapPath);

private:
    void InvokCallback(int protocolCode, const char *data);
	
private:
	std::map<int,CallBack> m_callbackMap;
	MapMgr *m_mapMgr;
};

#endif //INCLUDED_GLUE
