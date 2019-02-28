using UnityEngine;

public struct ColorHSV
{
	public float h;
	public float s;
	public float v;
	public float a;

	public ColorHSV(float h = 0f, float s = 0f, float v = 0f, float a = 1f)
	{
		this.h = h;
		this.s = s;
		this.v = v;
		this.a = a;
	}

	public override string ToString()
	{
		return string.Format("HSV({0:F3}, {1:F3}, {2:F3}, {3:F3})", h, s, v, a);
	}

	public override int GetHashCode()
	{
		return ((Color) this).GetHashCode();
	}

	public override bool Equals(object other)
	{
		if (other == null)
		{
			return false;
		}
		if (other is ColorHSV || other is Color || other is Color32)
		{
			return this == (ColorHSV) other;
		}
		return false;
	}

	public static implicit operator ColorHSV(Color color)
	{
		return color.ToHSV();
	}

	public static implicit operator Color(ColorHSV hsv)
	{
		return hsv.FromHSV();
	}

	public static implicit operator ColorHSV(Color32 color32)
	{
		return ((Color)color32).ToHSV();
	}

	public static implicit operator Color32(ColorHSV hsv)
	{
		return hsv.FromHSV();
	}

	public static bool operator !=(ColorHSV lhs, ColorHSV rhs)
	{
		return !(lhs == rhs);
	}

	public static bool operator ==(ColorHSV lhs, ColorHSV rhs)
	{
		if (lhs.a != rhs.a)
		{
			return false;
		}
		if (lhs.s == 0f && rhs.s == 0f)
		{
			return lhs.v == rhs.v;
		}
		return lhs.h == rhs.h
			&& lhs.s == rhs.s
			&& lhs.v == rhs.v;
	}
}

