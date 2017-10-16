#ifndef INCLUDED_TEST_INTERFACE
#define INCLUDED_TEST_INTERFACE

#include "Defines/types.h"

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

#pragma once
class test
{
public:
    test();
    ~test();

    int testtest(float p);
};

#endif //INCLUDED_TEST_INTERFACE
