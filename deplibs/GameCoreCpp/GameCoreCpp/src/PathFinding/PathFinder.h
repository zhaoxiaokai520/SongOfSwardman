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

#ifndef INCLUDED_PATHFINDER
#define INCLUDED_PATHFINDER

#include "Libs.math/VecInt2.h"
#include "Libs.math/RectInt.h"
#include "PathFinding/PathGoal.h"
#include "Singleton.h"

/**
 * @file
 * Declares Pathfinder. Its implementation is mainly done in CCmpPathfinder.cpp,
 * but the short-range (vertex) pathfinding is done in CCmpPathfinder_Vertex.cpp.
 * This file provides common code needed for both files.
 *
 * Interact with c# code as bridge
 * The long-range pathfinding is done by a LongPathfinder object.
 */
#include <vector>
#include "Defines/types.h"

typedef u16 passable_type;//object can passable terrain type. like ground, grass, water,sea 
static const int PASS_CLASS_BITS = 16;
typedef u16 NavcellData; // 1 bit per passability class (up to PASS_CLASS_BITS)
#define IS_PASSABLE(item, classmask) (((item) & (classmask)) == 0)
#define PASS_CLASS_MASK_FROM_INDEX(id) ((passable_type)(1u << id))
#define SPECIAL_PASS_CLASS PASS_CLASS_MASK_FROM_INDEX((PASS_CLASS_BITS-1)) // 16th bit, used for special in-place computations

struct AsyncLongPathReq
{
	int from_x;
	int from_z;
	PathGoal goal;
	passable_type passables;
	int object_id;
};

struct AsyncShortPathRequest
{
	u32 ticket;
	//entity_pos_t x0;
	//entity_pos_t z0;
	//entity_pos_t clearance;
	//entity_pos_t range;
	//PathGoal goal;
	//pass_class_t passClass;
	//bool avoidMovingUnits;
	//entity_id_t group;
	//entity_id_t notify;
};

#ifdef __cplusplus
extern "C" {
#endif

    extern DLL void ReqPathP(VecInt2 from, VecInt2 goal, int objectId);
    extern DLL void ReqPathC(VecInt2 from, VecInt2 center, int radius);
    extern DLL void ReqPathR(VecInt2 from, CRECT goal);
    extern DLL void ReqPathInvertC(VecInt2 from, VecInt2 center, int radius);
    extern DLL void ReqPathInvertR(VecInt2 from, CRECT goal);

#ifdef __cplusplus
}
#endif

class PathFinder : public Singleton<PathFinder>
{
public:
	void ReqPathP(VecInt2 from, VecInt2 goal, int objectId);
	void ReqPathC(VecInt2 from, VecInt2 center, int radius);
	void ReqPathR(VecInt2 from, CRECT goal);
	void ReqPathInvertC(VecInt2 from, VecInt2 center, int radius);
	void ReqPathInvertR(VecInt2 from, CRECT goal);

private:
	std::vector<AsyncLongPathReq> m_AsyncLongPathReqs;
};
#endif // INCLUDED_PATHFINDER
