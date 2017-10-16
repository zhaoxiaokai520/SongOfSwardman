/* Copyright (C) 2017 Wildfire Games.
 * This file is part of 0 A.D.
 *
 * 0 A.D. is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 *
 * 0 A.D. is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with 0 A.D.  If not, see <http://www.gnu.org/licenses/>.
 */

/**
 * @file
 * Common code and setup code for CCmpPathfinder.
 * Interact with c# code as bridge
 */

#include "PathFinder.h"
#include <iostream>
#include <assert.h>

template<class PathFinder> PathFinder* Singleton<PathFinder>::m_pInstance = NULL;

#ifdef __cplusplus
extern "C" {
#endif

DLL void ReqPathP(VecInt2 from, VecInt2 goal)
{
    printf("ReqPath c log %d %d", from.x, from.y);
    std::cout << "c++ style ReqPath called" << std::endl;
    //assert(false);
    //assert(from.x == goal.x);
    PathFinder::Instance()->RequestLongPath();
}

DLL void ReqPathC(VecInt2 from, VecInt2 center, int radius)
{
    printf("ReqPath c log");
    std::cout << "c++ style ReqPath called" << std::endl;
}

DLL void ReqPathR(VecInt2 from, CRECT goal)
{
    printf("ReqPath c log");
    std::cout << "c++ style ReqPath called" << std::endl;
}

DLL void ReqPathInvertC(VecInt2 from, VecInt2 center, int radius)
{
    printf("ReqPath c log");
    std::cout << "c++ style ReqPath called" << std::endl;
}

DLL void ReqPathInvertR(VecInt2 from, CRECT goal)
{
    printf("ReqPath c log");
    std::cout << "c++ style ReqPath called" << std::endl;
}

#ifdef __cplusplus
}
#endif

void PathFinder::RequestLongPath()
{

}