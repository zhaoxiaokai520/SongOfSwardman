/* Copyright (C) 2010 Wildfire Games.
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
 * Provides an interface for a vector in R3 and allows vector and
 * scalar operations on it
 */

#ifndef INCLUDED_VECTOR3D
#define INCLUDED_VECTOR3D

class CFixedVector3D;

struct VecInt3
{
	public:
		int x, y, z;

	public:
		VecInt3() : x(0), y(0), z(0) {}
		VecInt3(int x, int y, int z) : x(x), y(y), z(z) {}
		VecInt3(const CFixedVector3D& v);

		int operator!() const;

		int& operator[](int index) { return *((&x)+index); }
		const int& operator[](int index) const { return *((&x)+index); }

		// vector equality (testing int equality, so please be careful if necessary)
		bool operator==(const VecInt3 &vector) const
		{
			return (x == vector.x && y == vector.y && z == vector.z);
		}

		bool operator!=(const VecInt3& vector) const
		{
			return !operator==(vector);
		}

		VecInt3 operator+(const VecInt3& vector) const
		{
			return VecInt3(x + vector.x, y + vector.y, z + vector.z);
		}

		VecInt3& operator+=(const VecInt3& vector)
		{
			x += vector.x;
			y += vector.y;
			z += vector.z;
			return *this;
		}

		VecInt3 operator-(const VecInt3& vector) const
		{
			return VecInt3(x - vector.x, y - vector.y, z - vector.z);
		}

		VecInt3& operator-=(const VecInt3& vector)
		{
			x -= vector.x;
			y -= vector.y;
			z -= vector.z;
			return *this;
		}

		VecInt3 operator*(int value) const
		{
			return VecInt3(x * value, y * value, z * value);
		}

		VecInt3& operator*=(int value)
		{
			x *= value;
			y *= value;
			z *= value;
			return *this;
		}

		VecInt3 operator-() const
		{
			return VecInt3(-x, -y, -z);
		}

	public:
		int Dot (const VecInt3 &vector) const
		{
			return ( x * vector.x +
					 y * vector.y +
					 z * vector.z );
		}

		VecInt3 Cross (const VecInt3 &vector) const
		{
			VecInt3 Temp;
			Temp.x = (y * vector.z) - (z * vector.y);
			Temp.y = (z * vector.x) - (x * vector.z);
			Temp.z = (x * vector.y) - (y * vector.x);
			return Temp;
		}

		int Length () const;
		int LengthSquared () const;
		void Normalize ();
		VecInt3 Normalized () const;

		// Returns 3 element array of ints, e.g. for glVertex3fv
		const int* GetintArray() const { return &x; }
};

extern int MaxComponent(const VecInt3& v);

#endif
