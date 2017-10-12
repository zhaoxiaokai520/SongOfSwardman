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

#include "Pathfinder.h"
#include "Libs.math/Vector2D.h"
#include "Libs.math/RECT.h"
#include <iostream>

DLL void ReqPathP(CVector2D from, CVector2D goal)
{
    printf("ReqPath c log");
    std::cout << "c++ style ReqPath called" << std::endl;
}

DLL void ReqPathC(CVector2D from, CVector2D center, int radius)
{
    printf("ReqPath c log");
    std::cout << "c++ style ReqPath called" << std::endl;
}

DLL void ReqPathR(CVector2D from, CRECT goal)
{
    printf("ReqPath c log");
    std::cout << "c++ style ReqPath called" << std::endl;
}

DLL void ReqPathInvertC(CVector2D from, CVector2D center, int radius)
{
    printf("ReqPath c log");
    std::cout << "c++ style ReqPath called" << std::endl;
}

DLL void ReqPathInvertR(CVector2D from, CRECT goal)
{
    printf("ReqPath c log");
    std::cout << "c++ style ReqPath called" << std::endl;
}

void PathFinder::RequestLongPath()
{

}