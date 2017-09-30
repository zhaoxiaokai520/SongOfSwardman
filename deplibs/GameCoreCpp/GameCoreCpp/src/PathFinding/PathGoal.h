/* Copyright (C) 2015 Wildfire Games.
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

#ifndef INCLUDED_PATHGOAL
#define INCLUDED_PATHGOAL


/**
 * Pathfinder goal.
 * The goal can be either a point, a circle, or a square (rectangle).
 * For circles/squares, any point inside the shape is considered to be
 * part of the goal.
 * Also, it can be an 'inverted' circle/square, where any point outside
 * the shape is part of the goal.
 */
class PathGoal
{
public:
	enum Type {
		POINT,           // single point
		CIRCLE,          // the area inside a circle
		INVERTED_CIRCLE, // the area outside a circle
		SQUARE,          // the area inside a square
		INVERTED_SQUARE  // the area outside a square
	} type;

	
};

#endif // INCLUDED_PATHGOAL
