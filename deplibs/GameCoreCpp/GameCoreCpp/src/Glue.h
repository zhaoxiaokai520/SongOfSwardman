#ifndef INCLUDED_GLUE
#define INCLUDED_GLUE

#include "Defines/types.h"
#include "Singleton.h"

typedef void(*CallBack)();

#ifdef __cplusplus
extern "C" {
#endif

	extern DLL void AddCallback(int code, CallBack cb);
	extern DLL void RmvCallback(int code, CallBack cb);
	extern DLL void UpdateNative(int turnLength);

#ifdef __cplusplus
}
#endif

class Glue : public Singleton<Glue>
{
public:
    void AddCallback(int code, CallBack cb);
    void RmvCallback(int code, CallBack cb);
    void UpdateNative(int turnLength);
};

#endif //INCLUDED_GLUE
