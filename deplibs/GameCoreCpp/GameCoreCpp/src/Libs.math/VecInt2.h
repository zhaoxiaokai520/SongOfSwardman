/* Copyright (C) 2011 Wildfire Games.
 * This file is part of 0 A.D.
 *
 * 0 A.D. is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 *
 * 0 A.D. is distributed in the hope that it will be useful,
 * but WITHOUT ANy WARRANTy; without even the implied warranty of
 * MERCHANTABILITy or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * you should have received a copy of the GNU General Public License
 * along with 0 A.D.  If not, see <http://www.gnu.org/licenses/>.
 */

/*
 * Provides an interface for a vector in R2 and allows vector and
 * scalar operations on it
 */

#ifndef INCLUDED_MATHS_VECTOR2D
#define INCLUDED_MATHS_VECTOR2D


#include <math.h>

///////////////////////////////////////////////////////////////////////////////
// VecInt2:
struct VecInt2
{
public:
    int x, y;

public:
	VecInt2() {}
	VecInt2(int x, int y) : x(x), y(y) {}

	operator int*()
	{
		return &x;
	}

	operator const int*() const
	{
		return &x;
	}

	VecInt2 operator-() const
	{
		return VecInt2(-x, -y);
	}

	VecInt2 operator+(const VecInt2& t) const
	{
		return VecInt2(x + t.x, y + t.y);
	}

	VecInt2 operator-(const VecInt2& t) const
	{
		return VecInt2(x - t.x, y - t.y);
	}

	VecInt2 operator*(int f) const
	{
		return VecInt2(x * f, y * f);
	}

	VecInt2 operator/(int f) const
	{
		int inv = 1.0f / f;
		return VecInt2(x * inv, y * inv);
	}

	VecInt2& operator+=(const VecInt2& t)
	{
		x += t.x;
		y += t.y;
		return *this;
	}

	VecInt2& operator-=(const VecInt2& t)
	{
		x -= t.x;
		y -= t.y;
		return *this;
	}

	VecInt2& operator*=(int f)
	{
		x *= f;
		y *= f;
		return *this;
	}

	VecInt2& operator/=(int f)
	{
		int invf = 1.0f / f;
		x *= invf;
		y *= invf;
		return *this;
	}

	int Dot(const VecInt2& a) const
	{
		return x * a.x + y * a.y;
	}

	int LengthSquared() const
	{
		return Dot(*this);
	}

	int Length() const
	{
		return (int)sqrt(LengthSquared());
	}

	void Normalize()
	{
		int mag = Length();
		x /= mag;
		y /= mag;
	}

	VecInt2 Normalized() const
	{
		int mag = Length();
		return VecInt2(x / mag, y / mag);
	}

	/**
	 * Returns a version of this vector rotated counterclockwise by @p angle radians.
	 */
	VecInt2 Rotated(int angle) const
	{
		int c = cosf(angle);
		int s = sinf(angle);
		return VecInt2(
			c*x - s*y,
			s*x + c*y
		);
	}

	/**
	 * Rotates this vector counterclockwise by @p angle radians.
	 */
	void Rotate(int angle)
	{
		int c = cosf(angle);
		int s = sinf(angle);
		int newx = c*x - s*y;
		int newy = s*x + c*y;
		x = newx;
		y = newy;
	}
};
//////////////////////////////////////////////////////////////////////////////////


#endif
