using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct RECT
{
    private float m_XMin;

    private float m_YMin;

    private float m_Width;

    private float m_Height;

    public float x
    {
        get
        {
            return this.m_XMin;
        }
        set
        {
            this.m_XMin = value;
        }
    }

    public float y
    {
        get
        {
            return this.m_YMin;
        }
        set
        {
            this.m_YMin = value;
        }
    }

    public float width
    {
        get
        {
            return this.m_Width;
        }
        set
        {
            this.m_Width = value;
        }
    }

    public float height
    {
        get
        {
            return this.m_Height;
        }
        set
        {
            this.m_Height = value;
        }
    }

    public float xMin
    {
        get
        {
            return this.m_XMin;
        }
        set
        {
            float xMax = this.xMax;
            this.m_XMin = value;
            this.m_Width = xMax - this.m_XMin;
        }
    }

    public float yMin
    {
        get
        {
            return this.m_YMin;
        }
        set
        {
            float yMax = this.yMax;
            this.m_YMin = value;
            this.m_Height = yMax - this.m_YMin;
        }
    }

    public float xMax
    {
        get
        {
            return this.m_Width + this.m_XMin;
        }
        set
        {
            this.m_Width = value - this.m_XMin;
        }
    }

    public float yMax
    {
        get
        {
            return this.m_Height + this.m_YMin;
        }
        set
        {
            this.m_Height = value - this.m_YMin;
        }
    }

    public RECT(float left, float top, float width, float height)
    {
        this.m_XMin = left;
        this.m_YMin = top;
        this.m_Width = width;
        this.m_Height = height;
    }

    public RECT(RECT source)
    {
        this.m_XMin = source.m_XMin;
        this.m_YMin = source.m_YMin;
        this.m_Width = source.m_Width;
        this.m_Height = source.m_Height;
    }

    public static RECT MinMaxRect(float left, float top, float right, float bottom)
    {
        return new RECT(left, top, right - left, bottom - top);
    }

    public void Set(float left, float top, float width, float height)
    {
        this.m_XMin = left;
        this.m_YMin = top;
        this.m_Width = width;
        this.m_Height = height;
    }


    private static RECT OrderMinMax(RECT rect)
    {
        if (rect.xMin > rect.xMax)
        {
            float xMin = rect.xMin;
            rect.xMin = rect.xMax;
            rect.xMax = xMin;
        }
        if (rect.yMin > rect.yMax)
        {
            float yMin = rect.yMin;
            rect.yMin = rect.yMax;
            rect.yMax = yMin;
        }
        return rect;
    }

    public bool Overlaps(RECT other)
    {
        return other.xMax > this.xMin && other.xMin < this.xMax && other.yMax > this.yMin && other.yMin < this.yMax;
    }

    public bool Overlaps(RECT other, bool allowInverse)
    {
        RECT rect = this;
        if (allowInverse)
        {
            rect = RECT.OrderMinMax(rect);
            other = RECT.OrderMinMax(other);
        }
        return rect.Overlaps(other);
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.width.GetHashCode() << 2 ^ this.y.GetHashCode() >> 2 ^ this.height.GetHashCode() >> 1;
    }

    public override bool Equals(object other)
    {
        if (!(other is RECT))
        {
            return false;
        }
        RECT rect = (RECT)other;
        return this.x.Equals(rect.x) && this.y.Equals(rect.y) && this.width.Equals(rect.width) && this.height.Equals(rect.height);
    }

    public static bool operator !=(RECT lhs, RECT rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.y || lhs.width != rhs.width || lhs.height != rhs.height;
    }

    public static bool operator ==(RECT lhs, RECT rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
    }
}
