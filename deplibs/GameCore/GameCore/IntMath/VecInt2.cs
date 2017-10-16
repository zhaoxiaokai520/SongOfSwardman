using System;
using System.Diagnostics;

[Serializable]
public struct VecInt2
{
    public const int Precision = 1000;

    public const float FloatPrecision = 1000f;

    public const float PrecisionFactor = 0.001f;

    public int x;

	public int y;

    public float fx { get { return (float)x * PrecisionFactor; } set { x = (int)Math.Round(value * FloatPrecision); } }

    public float fy { get { return (float)y * PrecisionFactor; } set { y = (int)Math.Round(value * FloatPrecision); } }

    public static VecInt2 zero = default(VecInt2);

	private static readonly int[] Rotations = new int[]
	{
		1,
		0,
		0,
		1,
		0,
		1,
		-1,
		0,
		-1,
		0,
		0,
		-1,
		0,
		-1,
		1,
		0
	};

	public int sqrMagnitude
	{
		get
		{
#if UNITY_EDITOR
            //behaviac.Debug.Check(sqrMagnitudeLong < (long)System.Int32.MaxValue, "Vint2 unsafeSqrMagnitude!");
#endif
			return this.x * this.x + this.y * this.y;
		}
	}

	public long sqrMagnitudeLong
	{
		get
		{
			long num = (long)this.x;
			long num2 = (long)this.y;
			return num * num + num2 * num2;
		}
	}

	public int magnitude
	{
		get
		{
			long num = (long)this.x;
			long num2 = (long)this.y;
			return IntMath.Sqrt(num * num + num2 * num2);
		}
	}

	public VecInt2 normalized
	{
		get
		{
			VecInt2 result = new VecInt2(this.x, this.y);
			result.Normalize();
			return result;
		}
	}

    //public VecInt2(Vector2 position)
    //{
    //    this.x = (int)Math.Round((double)(position.x * 1000f));
    //    this.y = (int)Math.Round((double)(position.y * 1000f));
    //}

	public VecInt2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

    public VecInt2(long x, long y)
    {
#if UNITY_EDITOR
        Debug.Assert((System.Int32.MaxValue > Mathf.Abs(x) && System.Int32.MaxValue > Mathf.Abs(y) ), "VecInt2: value error !");
#endif
        this.x = (int)x;
        this.y = (int)y;
    }


    public static VecInt2 DivBy2(VecInt2 value)
    {
        return new VecInt2(value.x >> 1, value.y >> 1);
    }

	public static int Dot(VecInt2 a, VecInt2 b)
	{
		return a.x * b.x + a.y * b.y;
	}

	public static long DotLong(ref VecInt2 a, ref VecInt2 b)
	{
		return (long)a.x * (long)b.x + (long)a.y * (long)b.y;
	}

	public static long DotLong(VecInt2 a, VecInt2 b)
	{
		return (long)a.x * (long)b.x + (long)a.y * (long)b.y;
	}

	public static long DetLong(ref VecInt2 a, ref VecInt2 b)
	{
		return (long)a.x * (long)b.y - (long)a.y * (long)b.x;
	}

	public static long DetLong(VecInt2 a, VecInt2 b)
	{
		return (long)a.x * (long)b.y - (long)a.y * (long)b.x;
	}

	public override bool Equals(object o)
	{
		if (o == null)
		{
			return false;
		}
		VecInt2 vInt = (VecInt2)o;
		return this.x == vInt.x && this.y == vInt.y;
	}

	public override int GetHashCode()
	{
		return this.x * 49157 + this.y * 98317;
	}

	public static VecInt2 Rotate(VecInt2 v, int r)
	{
		r %= 4;
		return new VecInt2(v.x * VecInt2.Rotations[r * 4] + v.y * VecInt2.Rotations[r * 4 + 1], v.x * VecInt2.Rotations[r * 4 + 2] + v.y * VecInt2.Rotations[r * 4 + 3]);
	}

	public static VecInt2 Min(VecInt2 a, VecInt2 b)
	{
		return new VecInt2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
	}

	public static VecInt2 Max(VecInt2 a, VecInt2 b)
	{
		return new VecInt2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
	}

	public static VecInt2 FromInt3XZ(VecInt3 o)
	{
		return new VecInt2(o.x, o.z);
	}

	public static VecInt3 ToInt3XZ(VecInt2 o)
	{
		return new VecInt3(o.x, 0, o.y);
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"(",
			this.x,
			", ",
			this.y,
			")"
		});
	}

	public void Min(ref VecInt2 r)
	{
		this.x = Math.Min(this.x, r.x);
		this.y = Math.Min(this.y, r.y);
	}

	public void Max(ref VecInt2 r)
	{
		this.x = Math.Max(this.x, r.x);
		this.y = Math.Max(this.y, r.y);
	}

	public void Normalize()
	{
		long num = (long)(this.x * 100);
		long num2 = (long)(this.y * 100);
		long num3 = num * num + num2 * num2;
		if (num3 == 0L)
		{
			return;
		}
		long b = (long)IntMath.Sqrt(num3);
		this.x = (int)IntMath.Divide(num * 1000L, b);
		this.y = (int)IntMath.Divide(num2 * 1000L, b);
	}

    public VecInt2 NormalizeTo(long newMagn)
    {
        try
        {

            long num = (long)(this.x * 100);
            long num2 = (long)(this.y * 100);
            long num4 = num * num + num2 * num2;
            if (num4 == 0L)
            {
                return this;
            }
            long b = (long)IntMath.Sqrt(num4);
            long num5 = (long)newMagn;
            this.x = (int)IntMath.Divide(num * num5, b);
            this.y = (int)IntMath.Divide(num2 * num5, b);
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

    public VecInt2 NormalizeTo(int newMagn)
    {
        return NormalizeTo((long) newMagn);
    }

	public static VecInt2 ClampMagnitude(VecInt2 v, int maxLength)
	{
		long sqrMagnitudeLong = v.sqrMagnitudeLong;
		long num = (long)maxLength;
		if (sqrMagnitudeLong > num * num)
		{
			long b = (long)IntMath.Sqrt(sqrMagnitudeLong);
			int num2 = (int)IntMath.Divide((long)(v.x * maxLength), b);
			int num3 = (int)IntMath.Divide((long)(v.y * maxLength), b);
			return new VecInt2(num2, num3);
		}
		return v;
	}

    //public static explicit operator Vector2(VecInt2 ob)
    //{
    //    return new Vector2((float)ob.x * 0.001f, (float)ob.y * 0.001f);
    //}

    //public static explicit operator VecInt2(Vector2 ob)
    //{
    //    return new VecInt2((int)Math.Round((double)(ob.x * 1000f)), (int)Math.Round((double)(ob.y * 1000f)));
    //}

    public static VecInt2 operator +(VecInt2 a, VecInt2 b)
	{
		return new VecInt2(a.x + b.x, a.y + b.y);
	}

	public static VecInt2 operator -(VecInt2 a, VecInt2 b)
	{
		return new VecInt2(a.x - b.x, a.y - b.y);
	}

	public static bool operator ==(VecInt2 a, VecInt2 b)
	{
		return a.x == b.x && a.y == b.y;
	}

	public static bool operator !=(VecInt2 a, VecInt2 b)
	{
		return a.x != b.x || a.y != b.y;
	}

	public static VecInt2 operator -(VecInt2 lhs)
	{
		lhs.x = -lhs.x;
		lhs.y = -lhs.y;
		return lhs;
	}

	public static VecInt2 operator *(VecInt2 lhs, float rhs)
	{
        lhs.x = (int)Math.Round((float)lhs.x * rhs);
        lhs.y = (int)Math.Round((float)lhs.y * rhs);
		return lhs;
	}

    public static VecInt2 Mul(VecInt2 a, int b)
    {
        return new VecInt2((int)IntMath.Mul(a.x, b), (int)IntMath.Mul(a.y, b));
    }

    public static VecInt2 Mul(VecInt2 a, long b)
    {
        return new VecInt2((int)IntMath.Mul(a.x, b), (int)IntMath.Mul(a.y, b));
    }
}
