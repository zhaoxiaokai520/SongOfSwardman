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

DLL void ReqPathP(VecInt2 from, VecInt2 goal, int objectId)
{
    printf("ReqPath c log %d %d", from.x, from.y);
    std::cout << "c++ style ReqPath called" << std::endl;
    //assert(false);
    //assert(from.x == goal.x);
    //PathFinder::Instance()->RequestLongPath();
	PathFinder::Instance()->ReqPathP(from, goal, objectId);
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

void PathFinder::ReqPathP(VecInt2 from, VecInt2 goal, int objectId)
{
	AsyncLongPathReq req = { from.x, from.y,{ PathGoal::POINT, goal.x, goal.y }, (passable_type)(1u), objectId };
	m_AsyncLongPathReqs.push_back(req);
	//assert(from.x == goal.x);
}

void PathFinder::ReqPathC(VecInt2 from, VecInt2 center, int radius)
{

}

void PathFinder::ReqPathR(VecInt2 from, CRECT goal)
{

}

void PathFinder::ReqPathInvertC(VecInt2 from, VecInt2 center, int radius)
{

}

void PathFinder::ReqPathInvertR(VecInt2 from, CRECT goal)
{

}

void PathFinder::UpdateNative()
{
	// Figure out how many moves we can do this time
	int moveCount = 50;

	if (moveCount <= 0)
		return;

	// Copy the request elements we are going to process into a new array
	// for solving add remove req sync problem
	std::vector<AsyncLongPathReq> pathReqs;
	if (m_AsyncLongPathReqs.size() >= 0 && m_AsyncLongPathReqs.size() <= 50)
	{
		m_AsyncLongPathReqs.swap(pathReqs);
		moveCount = (i32)pathReqs.size();
	}
	else
	{
		pathReqs.resize(moveCount);
		copy(m_AsyncLongPathReqs.begin(), m_AsyncLongPathReqs.begin() + moveCount, pathReqs.begin());
		m_AsyncLongPathReqs.erase(m_AsyncLongPathReqs.begin(), m_AsyncLongPathReqs.begin() + moveCount);
	}

	for (size_t i = 0; i < pathReqs.size(); ++i)
	{
		const AsyncLongPathReq& req = pathReqs[i];
		ComputePath(req);
	}
}

void PathFinder::ComputePath(const AsyncLongPathReq &req)
{
	//case 1:distance is short and not way point between starter to dest
	// then use Short-Path-Finder with RVO
	//case 2:dist is long, then Long-Path-Finder with JPS is needed,
	//each path in sequent two way points need to use Short-Path-Finder
	//case 3:formation move as a single path finder starter
}