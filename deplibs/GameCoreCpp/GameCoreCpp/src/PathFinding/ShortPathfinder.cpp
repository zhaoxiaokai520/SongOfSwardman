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
 * Vertex-based algorithm for CCmpPathfinder.
 * Computes paths around the corners of rectangular obstructions.
 *
 * Useful search term for this algorithm: "points of visibility".
 *
 * Since we sometimes want to use this for avoiding moving units, there is no
 * pre-computation - the whole visibility graph is effectively regenerated for
 * each path, and it does A* over that graph.
 *
 * This scales very poorly in the number of obstructions, so it should be used
 * with a limited range and not exceedingly frequently.
 */

#include "precompiled.h"


/* Quadrant optimisation:
 * (loosely based on GPG2 "Optimizing Points-of-Visibility Pathfinding")
 *
 * Consider the vertex ("@") at a corner of an axis-aligned rectangle ("#"):
 *
 * TL  :  TR
 *     :
 * ####@ - - -
 * #####
 * #####
 * BL ##  BR
 *
 * The area around the vertex is split into TopLeft, BottomRight etc quadrants.
 *
 * If the shortest path reaches this vertex, it cannot continue to a vertex in
 * the BL quadrant (it would be blocked by the shape).
 * Since the shortest path is wrapped tightly around the edges of obstacles,
 * if the path approached this vertex from the TL quadrant,
 * it cannot continue to the TL or TR quadrants (the path could be shorter if it
 * skipped this vertex).
 * Therefore it must continue to a vertex in the BR quadrant (so this vertex is in
 * *that* vertex's TL quadrant).
 *
 * That lets us significantly reduce the search space by quickly discarding vertexes
 * from the wrong quadrants.
 *
 * (This causes badness if the path starts from inside the shape, so we add some hacks
 * for that case.)
 *
 * (For non-axis-aligned rectangles it's harder to do this computation, so we'll
 * not bother doing any discarding for those.)
 */
