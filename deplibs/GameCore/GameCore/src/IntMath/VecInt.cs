using System;

[Serializable]
public struct VecInt
{
	public int i;

    public static readonly VecInt zero = new VecInt(0);

	public float scalar
	{
		get
		{
			return (float)this.i * 0.001f;
		}
	}

	public VecInt(int i)
	{
		this.i = i;
	}

	public VecInt(float f)
	{
		this.i = (int)Math.Round((double)(f * 1000f));
	}

	public override bool Equals(object o)
	{
		if (o == null)
		{
			return false;
		}
		VecInt vInt = (VecInt)o;
		return this.i == vInt.i;
	}

	public override int GetHashCode()
	{
		return this.i.GetHashCode();
	}

	public static VecInt Min(VecInt a, VecInt b)
	{
		return new VecInt(Math.Min(a.i, b.i));
	}

	public static VecInt Max(VecInt a, VecInt b)
	{
		return new VecInt(Math.Max(a.i, b.i));
	}

	public override string ToString()
	{
		return this.scalar.ToString();
	}

	public static explicit operator VecInt(float f)
	{
		return new VecInt((int)Math.Round((double)(f * 1000f)));
	}

	public static implicit operator VecInt(int i)
	{
		return new VecInt(i);
	}

	public static explicit operator float(VecInt ob)
	{
		return (float)ob.i * 0.001f;
	}

	public static explicit operator long(VecInt ob)
	{
		return (long)ob.i;
	}

    public static explicit operator int(VecInt ob)
    {
        return ob.i;
    }

	public static VecInt operator +(VecInt a, VecInt b)
	{
		return new VecInt(a.i + b.i);
	}

	public static VecInt operator -(VecInt a, VecInt b)
	{
		return new VecInt(a.i - b.i);
	}

	public static bool operator ==(VecInt a, VecInt b)
	{
		return a.i == b.i;
	}

	public static bool operator !=(VecInt a, VecInt b)
	{
		return a.i != b.i;
	}

    public static VecInt operator *(VecInt a, VecInt b)
    {
        return new VecInt(a.i * b.i);
    }
}
