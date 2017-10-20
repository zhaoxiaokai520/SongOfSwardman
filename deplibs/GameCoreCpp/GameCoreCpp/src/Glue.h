#ifndef INCLUDED_GLUE
#define INCLUDED_GLUE

#include "Defines/types.h"
#include "Singleton.h"
#include <map>
#include <iostream>

typedef void(*CallBack)(const char * data);

#ifdef __cplusplus
extern "C" {
#endif
	
	extern DLL void AddCallback(int code, CallBack cb);
	extern DLL void RmvCallback(int code);
	extern DLL void UpdateNative(int turnLength);

#ifdef __cplusplus
}
#endif

class Glue : public Singleton<Glue>
{
public:
    void AddCB(int code, CallBack cb);
    void RmvCB(int code);
    void UpdateNativeImpl(int turnLength);
    void OnTimerNativeImpl();
	
private:
	std::map<int,CallBack> m_callbackMap;
};

#endif //INCLUDED_GLUE
