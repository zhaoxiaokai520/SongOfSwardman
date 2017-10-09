#include "test.h"
#include <iostream>
#include <assert.h>
#include <thread>

#ifdef __cplusplus
extern "C" {
#endif

DLL int testInterface()
{
     printf("test c log");
    std::cout << "c style testInterface called" << std::endl;
    assert(false);
    return 1111;
}

DLL void AddCallback(int code, CallBack cb)
{
    //std::thread t1(&myThread, this);//创建一个分支线程，回调到myThread函数里
    //t1.join();
    //assert(false);
}
DLL void RmvCallback(int code, CallBack cb)
{
    //assert(false);
}

DLL void UpdateNative(int turnLength)
{
    //assert(false);
}

void myThread()
{

}

#ifdef __cplusplus
}
#endif

test::test()
{

}


test::~test()
{

}

int test::testtest(float p)
{
    std::cout << "testtest called" << std::endl;
    return 0;
}

int main(void)
{
    return 0;
}
