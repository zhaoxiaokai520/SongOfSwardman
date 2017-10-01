#include "test.h"
#include "Defines/types.h"
#include <iostream>

#ifdef __cplusplus
extern "C" {
#endif

DLL int testInterface()
{
     printf("test c log");
    std::cout << "c style testInterface called" << std::endl;
    return 9999;
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
