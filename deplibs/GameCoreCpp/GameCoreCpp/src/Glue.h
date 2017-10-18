#ifndef INCLUDED_GLUE
#define INCLUDED_GLUE

#include "Defines/types.h"
#include "Singleton.h"

typedef void(*CallBack)();

#ifdef __cplusplus
extern "C" {
#endif

	extern DLL int testInterface();
	extern DLL void AddCallback(int code, CallBack cb);
	extern DLL void RmvCallback(int code, CallBack cb);
	extern DLL void UpdateNative(int turnLength);

#ifdef __cplusplus
}
#endif

class Glue : public Singleton<Glue>
{

};

#endif //INCLUDED_GLUE
