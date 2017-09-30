using System;
using System.Diagnostics;

[Serializable]
public struct VecInt3
{
	public const int Precision = 1000;

	public const float FloatPrecision = 1000f;

	public const float PrecisionFactor = 0.001f;

	public int x;

	public int y;

	public int z;

    public float fx { get { return (float)x * PrecisionFactor; } set { x = (int)Math.Round(value * FloatPrecision); } }

    public float fy { get { return (float)y * PrecisionFactor; } set { y = (int)Math.Round(value * FloatPrecision); } }

    public float fz { get { return (float)z * PrecisionFactor; } set { z = (int)Math.Round(value * FloatPrecision); } }

	public static readonly VecInt3 zero = new VecInt3(0, 0, 0);

	public static readonly VecInt3 one = new VecInt3(1000, 1000, 1000);

	public static readonly VecInt3 half = new VecInt3(500, 500, 500);

	public static readonly VecInt3 forward = new VecInt3(0, 0, 1000);

	public static readonly VecInt3 up = new VecInt3(0, 1000, 0);

	public static readonly VecInt3 right = new VecInt3(1000, 0, 0);

	public int this[int i]
	{
		get
		{
			return (i != 0) ? ((i != 1) ? this.z : this.y) : this.x;
		}
		set
		{
			if (i == 0)
			{
				this.x = value;
			}
			else if (i == 1)
			{
				this.y = value;
			}
			else
			{
				this.z = value;
			}
		}
	}

	//public Vector3 vec3
	//{
	//	get
	//	{
	//		return new Vector3((float)this.x * 0.001f, (float)this.y * 0.001f, (float)this.z * 0.001f);
	//	}
	//}

	public VecInt2 xz
	{
		get
		{
			return new VecInt2(this.x, this.z);
		}
	}

	public int magnitude
	{
		get
		{
			long num = (long)this.x;
			long num2 = (long)this.y;
			long num3 = (long)this.z;
			return IntMath.Sqrt(num * num + num2 * num2 + num3 * num3);
		}
	}

	public int magnitude2D
	{
		get
		{
			long num = (long)this.x;
			long num2 = (long)this.z;
			return IntMath.Sqrt(num * num + num2 * num2);
		}
	}

	public int costMagnitude
	{
		get
		{
			return this.magnitude;
		}
	}

	public float worldMagnitude
	{
		get
		{
			double num = (double)this.x;
			double num2 = (double)this.y;
			double num3 = (double)this.z;
			return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3) * 0.001f;
		}
	}

	public double sqrMagnitude
	{
		get
		{
			double num = (double)this.x;
			double num2 = (double)this.y;
			double num3 = (double)this.z;
			return num * num + num2 * num2 + num3 * num3;
		}
	}

	public long sqrMagnitudeLong
	{
		get
		{
			long num = (long)this.x;
			long num2 = (long)this.y;
			long num3 = (long)this.z;
			return num * num + num2 * num2 + num3 * num3;
		}
	}

	public long sqrMagnitudeLong2D
	{
		get
		{
			long num = (long)this.x;
			long num2 = (long)this.z;
			return num * num + num2 * num2;
		}
	}

	public int unsafeSqrMagnitude
	{
		get
		{
#if UNITY_EDITOR 
            behaviac.Debug.Check(sqrMagnitudeLong < (long)System.Int32.MaxValue, "Vint3 unsafeSqrMagnitude!");
#endif
			return this.x * this.x + this.y * this.y + this.z * this.z;
		}
	}

	public VecInt3 abs
	{
		get
		{
			return new VecInt3(Math.Abs(this.x), Math.Abs(this.y), Math.Abs(this.z));
		}
	}

	[Obsolete("Same implementation as .magnitude")]
	public float safeMagnitude
	{
		get
		{
			double num = (double)this.x;
			double num2 = (double)this.y;
			double num3 = (double)this.z;
			return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
		}
	}

	[Obsolete(".sqrMagnitude is now per default safe (.unsafeSqrMagnitude can be used for unsafe operations)")]
	public float safeSqrMagnitude
	{
		get
		{
			float num = (float)this.x * 0.001f;
			float num2 = (float)this.y * 0.001f;
			float num3 = (float)this.z * 0.001f;
			return num * num + num2 * num2 + num3 * num3;
		}
	}

	//public VecInt3(Vector3 position)
	//{
	//	this.x = (int)Math.Round((double)(position.x * 1000f));
	//	this.y = (int)Math.Round((double)(position.y * 1000f));
	//	this.z = (int)Math.Round((double)(position.z * 1000f));
	//}

	public VecInt3(int _x, int _y, int _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

    public void Scale(VecInt3 scale)
    {
        this.x = IntMath.Mul(this.x, scale.x);
        this.y = IntMath.Mul(this.y, scale.y);
        this.z = IntMath.Mul(this.z, scale.z);
    }

    public VecInt3 DivBy2()
    {
        this.x >>= 1;
        this.y >>= 1;
        this.z >>= 1;
        return this;
    }

    public static VecInt3 DivBy2(VecInt3 value)
	{
        return new VecInt3(value.x >> 1, value.y >> 1, value.z >> 1);
	}

    public void Set(int vx, int vy, int vz)
    {
        this.x = vx;
        this.y = vy;
        this.z = vz;
    }

	public static float Angle(VecInt3 lhs, VecInt3 rhs)
	{
		double num = (double)VecInt3.Dot(lhs, rhs) / ((double)lhs.magnitude * (double)rhs.magnitude);
		num = ((num >= -1.0) ? ((num <= 1.0) ? num : 1.0) : -1.0);
		return (float)Math.Acos(num);
	}

	public static IntFactor AngleInt(VecInt3 lhs, VecInt3 rhs)
	{
		long den = (long)lhs.magnitude * (long)rhs.magnitude;
		return IntMath.acos((long)VecInt3.Dot(ref lhs, ref rhs), den);
	}

	public static int Dot(ref VecInt3 lhs, ref VecInt3 rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
	}

	public static int Dot(VecInt3 lhs, VecInt3 rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
	}

	public static long DotLong(VecInt3 lhs, VecInt3 rhs)
	{
		return (long)lhs.x * (long)rhs.x + (long)lhs.y * (long)rhs.y + (long)lhs.z * (long)rhs.z;
	}

	public static long DotLong(ref VecInt3 lhs, ref VecInt3 rhs)
	{
		return (long)lhs.x * (long)rhs.x + (long)lhs.y * (long)rhs.y + (long)lhs.z * (long)rhs.z;
	}

    public static int DotXZ(ref VecInt3 lhs, ref VecInt3 rhs)
    {
        return lhs.x * rhs.x + lhs.z * rhs.z;
    }

    public static int DotXZ(VecInt3 lhs, VecInt3 rhs)
    {
        return lhs.x * rhs.x + lhs.z * rhs.z;
    }

    public static long DotXZLong(ref VecInt3 lhs, ref VecInt3 rhs)
	{
		return (long)lhs.x * (long)rhs.x + (long)lhs.z * (long)rhs.z;
	}

	public static long DotXZLong(VecInt3 lhs, VecInt3 rhs)
	{
		return (long)lhs.x * (long)rhs.x + (long)lhs.z * (long)rhs.z;
	}

	public static VecInt3 Cross(ref VecInt3 lhs, ref VecInt3 rhs)
	{
		return new VecInt3(IntMath.Divide(lhs.y * rhs.z - lhs.z * rhs.y, 1000), IntMath.Divide(lhs.z * rhs.x - lhs.x * rhs.z, 1000), IntMath.Divide(lhs.x * rhs.y - lhs.y * rhs.x, 1000));
	}

	public static VecInt3 Cross(VecInt3 lhs, VecInt3 rhs)
	{
		return new VecInt3(IntMath.Divide(lhs.y * rhs.z - lhs.z * rhs.y, 1000), IntMath.Divide(lhs.z * rhs.x - lhs.x * rhs.z, 1000), IntMath.Divide(lhs.x * rhs.y - lhs.y * rhs.x, 1000));
	}

	public static VecInt3 MoveTowards(VecInt3 from, VecInt3 to, int dt)
	{
		if ((to - from).sqrMagnitudeLong <= (long)(dt * dt))
		{
			return to;
		}
		return from + (to - from).NormalizeTo(dt);
	}

	public VecInt3 Normal2D()
	{
		return new VecInt3(this.z, this.y, -this.x);
	}

	public VecInt3 NormalizeTo(int newMagn)
	{
	    try
	    {

	        long num = (long) (this.x*100);
	        long num2 = (long) (this.y*100);
	        long num3 = (long) (this.z*100);
	        long num4 = num*num + num2*num2 + num3*num3;
	        if (num4 == 0L)
	        {
	            return this;
	        }
	        long b = (long) IntMath.Sqrt(num4);
            long num5 = (long)newMagn;
            this.x = (int)IntMath.Divide(num * num5, b);
            this.y = (int)IntMath.Divide(num2 * num5, b);
            this.z = (int)IntMath.Divide(num3 * num5, b);
        }
        catch (System.Exception e)
        {
#if DEBUFG_LOGOUT
             if (MobaGo.Game.Data.DebugMask.HasMask(MobaGo.Game.Data.DebugMask.E_DBG_MASK.MASK_FRAMEDATA))
            {
                DebugHelper.LogOut("NormalizeTo Vint3 ERROR " + e.Message + " : " + e.StackTrace);
            }
#endif
            Debug.Print("Caught exception while Num = " + this);
        }
	    
		return this;
	}

    public VecInt3 NormalizeTo2D(int newMagn)
    {
        try
        {

            long num = (long)(this.x * 100);
            long num3 = (long)(this.z * 100);
            long num4 = num * num + num3 * num3;
            if (num4 == 0L)
            {
                return this;
            }
            long b = (long)IntMath.Sqrt(num4);
            long num5 = (long)newMagn;
            this.x = (int)IntMath.Divide(num * num5, b);
            this.z = (int)IntMath.Divide(num3 * num5, b);
        }
        catch (System.Exception e)
        {
#if DEBUFG_LOGOUT
             if (MobaGo.Game.Data.DebugMask.HasMask(MobaGo.Game.Data.DebugMask.E_DBG_MASK.MASK_FRAMEDATA))
            {
                DebugHelper.LogOut("NormalizeTo Vint3 ERROR " + e.Message + " : " + e.StackTrace);
            }
#endif
            Debug.Print("Caught exception while Num = " + this);
        }

        return this;
    }

	public VecInt3 RotateY(ref IntFactor radians)
	{
		IntFactor vFactor;
		IntFactor vFactor2;
		IntMath.sincos(out vFactor, out vFactor2, radians.nom, radians.den);
		long num = vFactor2.nom * vFactor.den;
		long num2 = vFactor2.den * vFactor.nom;
		long b = vFactor2.den * vFactor.den;
		VecInt3 vInt;
		vInt.x = (int)IntMath.Divide((long)this.x * num + (long)this.z * num2, b);
		vInt.z = (int)IntMath.Divide((long)(-(long)this.x) * num2 + (long)this.z * num, b);
		vInt.y = 0;
		return vInt.NormalizeTo(1000);
	}

	public VecInt3 RotateY(int degree)
	{
		IntFactor vFactor;
		IntFactor vFactor2;
		IntMath.sincos(out vFactor, out vFactor2, (long)(31416 * degree), 1800000L);
		long num = vFactor2.nom * vFactor.den;
		long num2 = vFactor2.den * vFactor.nom;
		long b = vFactor2.den * vFactor.den;
		VecInt3 vInt;
		vInt.x = (int)IntMath.Divide((long)this.x * num + (long)this.z * num2, b);
		vInt.z = (int)IntMath.Divide((long)(-(long)this.x) * num2 + (long)this.z * num, b);
		vInt.y = 0;
		return vInt.NormalizeTo(1000);
	}

    public static VecInt3 ClampMagnitude(VecInt3 v, int maxLength)
    {
        long sqrMagnitudeLong = v.sqrMagnitudeLong;
        long num = (long)maxLength;
        if (sqrMagnitudeLong > num * num)
        {
            long b = (long)IntMath.Sqrt(sqrMagnitudeLong);
            int num2 = (int)IntMath.Divide((long)(v.x * maxLength), b);
            int num3 = (int)IntMath.Divide((long)(v.y * maxLength), b);
            int num4 = (int)IntMath.Divide((long)(v.z * maxLength), b);
            return new VecInt3(num2, num3, num4);
        }
        return v;
    }

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"( ",
			this.x,
			", ",
			this.y,
			", ",
			this.z,
			")"
		});
	}

	public override bool Equals(object o)
	{
		if (o == null)
		{
			return false;
		}
		VecInt3 vInt = (VecInt3)o;
		return this.x == vInt.x && this.y == vInt.y && this.z == vInt.z;
	}

	public override int GetHashCode()
	{
		return this.x * 73856093 ^ this.y * 19349663 ^ this.z * 83492791;
	}

	public static VecInt3 Lerp(VecInt3 a, VecInt3 b, float f)
	{
		return new VecInt3((int)Math.Round((float)a.x * (1f - f)) + (int)Math.Round((float)b.x * f), (int)Math.Round((float)a.y * (1f - f)) + (int)Math.Round((float)b.y * f), (int)Math.Round((float)a.z * (1f - f)) + (int)Math.Round((float)b.z * f));
	}

	public static VecInt3 Lerp(VecInt3 a, VecInt3 b, IntFactor f)
	{
		return new VecInt3((int)IntMath.Divide((long)(b.x - a.x) * f.nom, f.den) + a.x, (int)IntMath.Divide((long)(b.y - a.y) * f.nom, f.den) + a.y, (int)IntMath.Divide((long)(b.z - a.z) * f.nom, f.den) + a.z);
	}

    public static VecInt3 Lerp(VecInt3 a, VecInt3 b, int factorNom, int factorDen)
    {
        return new VecInt3(IntMath.Divide((b.x - a.x) * factorNom, factorDen) + a.x, IntMath.Divide((b.y - a.y) * factorNom, factorDen) + a.y, IntMath.Divide((b.z - a.z) * factorNom, factorDen) + a.z);
    }

	public long XZSqrMagnitude(VecInt3 rhs)
	{
		long num = (long)(this.x - rhs.x);
		long num2 = (long)(this.z - rhs.z);
		return num * num + num2 * num2;
	}

	public long XZSqrMagnitude(ref VecInt3 rhs)
	{
		long num = (long)(this.x - rhs.x);
		long num2 = (long)(this.z - rhs.z);
		return num * num + num2 * num2;
	}

    static public long XYZSqrMagnitude(VecInt3 lhs, VecInt3 rhs)
	{
        long num = (long)(lhs.x - rhs.x);
        long num1 = (long)(lhs.y - rhs.y);
        long num2 = (long)(lhs.z - rhs.z);
        return num * num + num1 * num1 + num2 * num2;
	}

	public bool IsEqualXZ(VecInt3 rhs)
	{
		return this.x == rhs.x && this.z == rhs.z;
	}

	public bool IsEqualXZ(ref VecInt3 rhs)
	{
		return this.x == rhs.x && this.z == rhs.z;
	}

	public static bool operator ==(VecInt3 lhs, VecInt3 rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
	}

	public static bool operator !=(VecInt3 lhs, VecInt3 rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
	}

	//public static explicit operator VecInt3(Vector3 ob)
	//{
	//	return new VecInt3((int)Math.Round((double)(ob.x * 1000f)), (int)Math.Round((double)(ob.y * 1000f)), (int)Math.Round((double)(ob.z * 1000f)));
	//}

	//public static explicit operator Vector3(VecInt3 ob)
	//{
	//	return new Vector3((float)ob.x * 0.001f, (float)ob.y * 0.001f, (float)ob.z * 0.001f);
	//}

	public static VecInt3 operator -(VecInt3 lhs, VecInt3 rhs)
	{
		lhs.x -= rhs.x;
		lhs.y -= rhs.y;
		lhs.z -= rhs.z;
		return lhs;
	}

	public static VecInt3 operator -(VecInt3 lhs)
	{
		lhs.x = -lhs.x;
		lhs.y = -lhs.y;
		lhs.z = -lhs.z;
		return lhs;
	}

	public static VecInt3 operator +(VecInt3 lhs, VecInt3 rhs)
	{
		lhs.x += rhs.x;
		lhs.y += rhs.y;
		lhs.z += rhs.z;
		return lhs;
	}

    public VecInt3 Mul(int rhs)
	{
        return new VecInt3( IntMath.Mul(x, rhs), IntMath.Mul(y, rhs), IntMath.Mul(z, rhs));
	}

    public VecInt3 Mul(long rhs)
    {
        return new VecInt3((int)IntMath.Mul(x, rhs), (int)IntMath.Mul(y, rhs), (int)IntMath.Mul(z, rhs));
    }

    public VecInt3 MulXZ(int rhs)
    {
        return new VecInt3(IntMath.Mul(x, rhs), y, IntMath.Mul(z, rhs));
    }

    public static VecInt3 Mul(VecInt3 a, int b)
    {
        return new VecInt3(IntMath.Mul(a.x, b), IntMath.Mul(a.y, b), IntMath.Mul(a.z, b));
    }

    public static VecInt3 Mul(VecInt3 a, long b)
    {
        return new VecInt3((int)IntMath.Mul(a.x, b), (int)IntMath.Mul(a.y, b), (int)IntMath.Mul(a.z, b));
    }

    public static VecInt3 operator *(VecInt3 lhs, float rhs)
	{
        lhs.x = (int)Math.Round(lhs.x * rhs);
        lhs.y = (int)Math.Round(lhs.y * rhs);
        lhs.z = (int)Math.Round(lhs.z * rhs);
		return lhs;
	}

	public static VecInt3 operator *(VecInt3 lhs, double rhs)
	{
		lhs.x = (int)Math.Round((double)lhs.x * rhs);
		lhs.y = (int)Math.Round((double)lhs.y * rhs);
		lhs.z = (int)Math.Round((double)lhs.z * rhs);
		return lhs;
	}

	//public static VecInt3 operator *(VecInt3 lhs, Vector3 rhs)
	//{
	//	lhs.x = (int)Math.Round((double)((float)lhs.x * rhs.x));
	//	lhs.y = (int)Math.Round((double)((float)lhs.y * rhs.y));
	//	lhs.z = (int)Math.Round((double)((float)lhs.z * rhs.z));
	//	return lhs;
	//}

    //public static VecInt3 operator *(VecInt3 lhs, VecInt3 rhs)
    //{
    //	lhs.x *= rhs.x;
    //	lhs.y *= rhs.y;
    //	lhs.z *= rhs.z;
    //	return lhs;
    //}

	public static VecInt3 operator /(VecInt3 lhs, float rhs)
	{
        lhs.x = (int)Math.Round((float)lhs.x / rhs);
        lhs.y = (int)Math.Round((float)lhs.y / rhs);
        lhs.z = (int)Math.Round((float)lhs.z / rhs);
		return lhs;
	}

    public static VecInt3 Min(VecInt3 lhs, VecInt3 rhs) { return new VecInt3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z)); }
    public static VecInt3 Max(VecInt3 lhs, VecInt3 rhs) { return new VecInt3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z)); }

    public static implicit operator string(VecInt3 ob)
	{
		return ob.ToString();
	}
}
