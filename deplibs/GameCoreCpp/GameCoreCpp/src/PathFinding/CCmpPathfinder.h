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

#ifndef INCLUDED_CCMPPATHFINDER
#define INCLUDED_CCMPPATHFINDER

/**
 * @file
 * Declares CCmpPathfinder. Its implementation is mainly done in CCmpPathfinder.cpp,
 * but the short-range (vertex) pathfinding is done in CCmpPathfinder_Vertex.cpp.
 * This file provides common code needed for both files.
 *
 * The long-range pathfinding is done by a LongPathfinder object.
 */
#include <vector>
#include "Defines/types.h"

struct AsyncLongPathRequest
{
	u32 ticket;
	//entity_pos_t x0;
	//entity_pos_t z0;
	//PathGoal goal;
	//pass_class_t passClass;
	//entity_id_t notify;
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

class CCmpPathfinder
{
public:
	void RequestLongPath();

private:
	std::vector<AsyncLongPathRequest> m_AsyncLongPathRequests;
};
#endif // INCLUDED_CCMPPATHFINDER
