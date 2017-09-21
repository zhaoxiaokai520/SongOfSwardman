#include "test.h"
#include <iostream>

#ifdef __cplusplus
extern "C" {
#endif

 __declspec(dllexport) int testInterface()
{
    std::cout << "c style testInterface called" << std::endl;
    return 0;
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
