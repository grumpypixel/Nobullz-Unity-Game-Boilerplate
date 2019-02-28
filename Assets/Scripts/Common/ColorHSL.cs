using UnityEngine;

public struct ColorHSL
{
	public float h;
	public float s;
	public float l;
	public float a;

	public ColorHSL(float h = 0f, float s = 0f, float l = 0f, float a = 1f)
	{
		this.h = h;
		this.s = s;
		this.l = l;
		this.a = a;
	}

	public override string ToString()
	{
		return string.Format("HSL({0:F3}, {1:F3}, {2:F3}, {3:F3})", h, s, l, a);
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
		if (other is ColorHSL || other is Color || other is Color32)
		{
			return this == (ColorHSL) other;
		}
		return false;
	}

	public static implicit operator ColorHSL(Color color)
	{
		return color.ToHSL();
	}

	public static implicit operator Color(ColorHSL hsl)
	{
		return hsl.FromHSL();
	}

	public static implicit operator ColorHSL(Color32 color32)
	{
		return ((Color)color32).ToHSL();
	}

	public static implicit operator Color32(ColorHSL hsl)
	{
		return hsl.FromHSL();
	}

	public static bool operator !=(ColorHSL lhs, ColorHSL rhs)
	{
		return !(lhs == rhs);
	}

	public static bool operator ==(ColorHSL lhs, ColorHSL rhs)
	{
		if (lhs.a != rhs.a)
		{
			return false;
		}
		if (lhs.s == 0f && rhs.s == 0f)
		{
			return lhs.l == rhs.l;
		}
		return lhs.h == rhs.h
			&& lhs.s == rhs.s
			&& lhs.l == rhs.l;
	}
}

