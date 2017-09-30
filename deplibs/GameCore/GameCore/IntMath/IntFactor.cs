using System;
using System.Diagnostics;

[Serializable]
public struct IntFactor
{
	public long nom;

	public long den;

    [NonSerialized]
    public static IntFactor max = new IntFactor(System.Int64.MaxValue, 1L);

    [NonSerialized]
    public static IntFactor min = new IntFactor(System.Int64.MinValue, 1L);

    [NonSerialized]
	public static IntFactor zero = new IntFactor(0L, 1L);


	[NonSerialized]
	public static IntFactor one = new IntFactor(1L, 1L);

	[NonSerialized]
	public static IntFactor pi = new IntFactor(31416L, 10000L);

	[NonSerialized]
	public static IntFactor twoPi = new IntFactor(62832L, 10000L);

	private static long mask_ = 0x7fffffffffffffff;//9223372036854775807L;

	private static long upper_ = 0x00ffffff;//16777215L;

	public int roundInt
	{
		get
		{
			return (int)IntMath.Divide(this.nom, this.den);
		}
	}

	public int integer
	{
		get
		{
			return (int)(this.nom / this.den);
		}
	}

	public float single
	{
		get
		{
			double num = (double)this.nom / (double)this.den;
			return (float)num;
		}
	}

	public bool IsPositive
	{
		get
		{
			Debug.Assert(this.den != 0L, "IntFactor: denominator is zero !");
			if (this.nom == 0L)
			{
				return false;
			}
			bool flag = this.nom > 0L;
			bool flag2 = this.den > 0L;
			return !(flag ^ flag2);
		}
	}

	public bool IsNegative
	{
		get
		{
            Debug.Assert(this.den != 0L, "IntFactor: denominator is zero !");
			if (this.nom == 0L)
			{
				return false;
			}
			bool flag = this.nom > 0L;
			bool flag2 = this.den > 0L;
			return flag ^ flag2;
		}
	}

	public bool IsZero
	{
		get
		{
			return this.nom == 0L;
		}
	}

	public IntFactor Inverse
	{
		get
		{
			return new IntFactor(this.den, this.nom);
		}
	}

	public IntFactor(long n, long d)
	{
		this.nom = n;
		this.den = d;
	}

	public override bool Equals(object obj)
	{
		return obj != null && base.GetType() == obj.GetType() && this == (IntFactor)obj;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return this.single.ToString();
	}

	public void strip()
	{
		while ((this.nom & IntFactor.mask_) > IntFactor.upper_ && (this.den & IntFactor.mask_) > IntFactor.upper_)
		{
			this.nom >>= 1;
			this.den >>= 1;
		}
	}

	public static bool operator <(IntFactor a, IntFactor b)
	{
		long num = a.nom * b.den;
		long num2 = b.nom * a.den;
		bool flag = b.den > 0L ^ a.den > 0L;
		return (!flag) ? (num < num2) : (num > num2);
	}

	public static bool operator >(IntFactor a, IntFactor b)
	{
		long num = a.nom * b.den;
		long num2 = b.nom * a.den;
		bool flag = b.den > 0L ^ a.den > 0L;
		return (!flag) ? (num > num2) : (num < num2);
	}

	public static bool operator <=(IntFactor a, IntFactor b)
	{
		long num = a.nom * b.den;
		long num2 = b.nom * a.den;
		bool flag = b.den > 0L ^ a.den > 0L;
		return (!flag) ? (num <= num2) : (num >= num2);
	}

	public static bool operator >=(IntFactor a, IntFactor b)
	{
		long num = a.nom * b.den;
		long num2 = b.nom * a.den;
		bool flag = b.den > 0L ^ a.den > 0L;
		return (!flag) ? (num >= num2) : (num <= num2);
	}

	public static bool operator ==(IntFactor a, IntFactor b)
	{
		return a.nom * b.den == b.nom * a.den;
	}

	public static bool operator !=(IntFactor a, IntFactor b)
	{
		return a.nom * b.den != b.nom * a.den;
	}

	public static bool operator <(IntFactor a, long b)
	{
		long num = a.nom;
		long num2 = b * a.den;
		return (a.den <= 0L) ? (num > num2) : (num < num2);
	}

	public static bool operator >(IntFactor a, long b)
	{
		long num = a.nom;
		long num2 = b * a.den;
		return (a.den <= 0L) ? (num < num2) : (num > num2);
	}

	public static bool operator <=(IntFactor a, long b)
	{
		long num = a.nom;
		long num2 = b * a.den;
		return (a.den <= 0L) ? (num >= num2) : (num <= num2);
	}

	public static bool operator >=(IntFactor a, long b)
	{
		long num = a.nom;
		long num2 = b * a.den;
		return (a.den <= 0L) ? (num <= num2) : (num >= num2);
	}

	public static bool operator ==(IntFactor a, long b)
	{
		return a.nom == b * a.den;
	}

	public static bool operator !=(IntFactor a, long b)
	{
		return a.nom != b * a.den;
	}

	public static IntFactor operator +(IntFactor a, IntFactor b)
	{
		return new IntFactor
		{
			nom = a.nom * b.den + b.nom * a.den,
			den = a.den * b.den
		};
	}

	public static IntFactor operator +(IntFactor a, long b)
	{
		a.nom += b * a.den;
		return a;
	}

	public static IntFactor operator -(IntFactor a, IntFactor b)
	{
		return new IntFactor
		{
			nom = a.nom * b.den - b.nom * a.den,
			den = a.den * b.den
		};
	}

	public static IntFactor operator -(IntFactor a, long b)
	{
		a.nom -= b * a.den;
		return a;
	}

	public static IntFactor operator *(IntFactor a, long b)
	{
		a.nom *= b;
		return a;
	}

    public static IntFactor operator *(IntFactor a, IntFactor b)
    {
        return new IntFactor(a.nom * b.nom, a.den * b.den);
    }

    public static IntFactor operator /(IntFactor a, long b)
	{
		a.den *= b;
		return a;
	}

    public static IntFactor operator /(IntFactor a, IntFactor b)
    {
        return new IntFactor( a.nom * b.den, a.den * b.nom);
    }

	public static VecInt3 operator *(VecInt3 v, IntFactor f)
	{
		return IntMath.Divide(v, f.nom, f.den);
	}

	public static VecInt2 operator *(VecInt2 v, IntFactor f)
	{
		return IntMath.Divide(v, f.nom, f.den);
	}

	public static VecInt3 operator /(VecInt3 v, IntFactor f)
	{
		return IntMath.Divide(v, f.den, f.nom);
	}

	public static VecInt2 operator /(VecInt2 v, IntFactor f)
	{
		return IntMath.Divide(v, f.den, f.nom);
	}

	public static int operator *(int i, IntFactor f)
	{
		return (int)IntMath.Divide((long)i * f.nom, f.den);
	}

	public static IntFactor operator -(IntFactor a)
	{
		a.nom = -a.nom;
		return a;
	}
}
