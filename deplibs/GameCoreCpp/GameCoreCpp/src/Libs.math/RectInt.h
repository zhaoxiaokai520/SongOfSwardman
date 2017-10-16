/* Copyright (C) 2011 Wildfire Games.
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

/*
 * Provides an interface for a vector in R2 and allows vector and
 * scalar operations on it
 */

#ifndef INCLUDED_MATHS_RECTINT
#define INCLUDED_MATHS_RECTINT


#include <math.h>

///////////////////////////////////////////////////////////////////////////////
// CVector2D:
class CRECT
{
public:
    CRECT() {}
    CRECT(int x, int y) : X(x), Y(y) {}

	/*operator int*()
	{
		return &X;
	}

	operator const int*() const
	{
		return &X;
	}*/

    CRECT operator-() const
	{
		return CRECT(-X, -Y);
	}

    CRECT operator+(const CRECT& t) const
	{
		return CRECT(X + t.X, Y + t.Y);
	}

    CRECT operator-(const CRECT& t) const
	{
		return CRECT(X - t.X, Y - t.Y);
	}

    CRECT operator*(int f) const
	{
		return CRECT(X * f, Y * f);
	}

    CRECT operator/(int f) const
	{
		int inv = 1.0f / f;
		return CRECT(X * inv, Y * inv);
	}

    CRECT& operator+=(const CRECT& t)
	{
		X += t.X;
		Y += t.Y;
		return *this;
	}

    CRECT& operator-=(const CRECT& t)
	{
		X -= t.X;
		Y -= t.Y;
		return *this;
	}

    CRECT& operator*=(int f)
	{
		X *= f;
		Y *= f;
		return *this;
	}

    CRECT& operator/=(int f)
	{
		int invf = 1.0f / f;
		X *= invf;
		Y *= invf;
		return *this;
	}

	int Dot(const CRECT& a) const
	{
		return X * a.X + Y * a.Y;
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
		X /= mag;
		Y /= mag;
	}

    CRECT Normalized() const
	{
		int mag = Length();
		return CRECT(X / mag, Y / mag);
	}

	/**
	 * Returns a version of this vector rotated counterclockwise by @p angle radians.
	 */
    CRECT Rotated(int angle) const
	{
		int c = cosf(angle);
		int s = sinf(angle);
		return CRECT(
			c*X - s*Y,
			s*X + c*Y
		);
	}

	/**
	 * Rotates this vector counterclockwise by @p angle radians.
	 */
	void Rotate(int angle)
	{
		int c = cosf(angle);
		int s = sinf(angle);
		int newX = c*X - s*Y;
		int newY = s*X + c*Y;
		X = newX;
		Y = newY;
	}

public:
	int X, Y;
    int width, height;
};
//////////////////////////////////////////////////////////////////////////////////


#endif//INCLUDED_MATHS_RECT
