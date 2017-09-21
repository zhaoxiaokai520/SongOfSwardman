#ifndef INCLUDED_TEST_INTERFACE
#define INCLUDED_TEST_INTERFACE

#ifdef __cplusplus
extern "C" {
#endif

    extern __declspec(dllexport) int testInterface();

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
