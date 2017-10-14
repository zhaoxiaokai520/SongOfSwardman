/* Copyright (C) 2012 wildfire Games.
 * This file is part of 0 A.D.
 *
 * 0 A.D. is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 *
 * 0 A.D. is distributed in the hope that it will be useful,
 * but wITHOUT ANy wARRANTy; without even the implied warranty of
 * MERCHANTABILITy or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * you should have received a copy of the GNU General Public License
 * along with 0 A.D.  If not, see <http://www.gnu.org/licenses/>.
 */

/*
 * Provides an interface for a vector in R4 and allows vector and
 * scalar operations on it
 */

#ifndef INCLUDED_VECTOR4D
#define INCLUDED_VECTOR4D

#include <math.h>

struct VecInt4
{
public:
    int x, y, z, w;

public:
	VecInt4() : x(0), y(0), z(0), w(0) { }

	VecInt4(int x, int y, int z, int w) : x(x), y(y), z(z), w(w) { }

	bool operator==(const VecInt4& t) const
	{
		return (x == t.x && y == t.y && z == t.z && w == t.w);
	}

	bool operator!=(const VecInt4& t) const
	{
		return !(*this == t);
	}

	VecInt4 operator-() const
	{
		return VecInt4(-x, -y, -z, -w);
	}

	VecInt4 operator+(const VecInt4& t) const
	{
		return VecInt4(x+t.x, y+t.y, z+t.z, w+t.w);
	}

	VecInt4 operator-(const VecInt4& t) const
	{
		return VecInt4(x-t.x, y-t.y, z-t.z, w-t.w);
	}

	VecInt4 operator*(const VecInt4& t) const
	{
		return VecInt4(x*t.x, y*t.y, z*t.z, w*t.w);
	}

	VecInt4 operator*(int f) const
	{
		return VecInt4(x*f, y*f, z*f, w*f);
	}

	VecInt4 operator/(int f) const
	{
		int inv_f = 1.0f / f;
		return VecInt4(x*inv_f, y*inv_f, z*inv_f, w*inv_f);
	}

	VecInt4& operator+=(const VecInt4& t)
	{
		x += t.x;
		y += t.y;
		z += t.z;
		w += t.w;
		return *this;
	}

	VecInt4& operator-=(const VecInt4& t)
	{
		x -= t.x;
		y -= t.y;
		z -= t.z;
		w -= t.w;
		return *this;
	}

	VecInt4& operator*=(const VecInt4& t)
	{
		x *= t.x;
		y *= t.y;
		z *= t.z;
		w *= t.w;
		return *this;
	}

	VecInt4& operator*=(int f)
	{
		x *= f;
		y *= f;
		z *= f;
		w *= f;
		return *this;
	}

	VecInt4& operator/=(int f)
	{
		int inv_f = 1.0f / f;
		x *= inv_f;
		y *= inv_f;
		z *= inv_f;
		w *= inv_f;
		return *this;
	}

	int Dot(const VecInt4& a) const
	{
		return x*a.x + y*a.y + z*a.z + w*a.w;
	}
};

#endif
