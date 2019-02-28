using System;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensions
{
	static private readonly float Epsilon = 0.00001f;

	public static ColorHSV ToHSV(this Color color)
	{
		float h, s, v;
		Color.RGBToHSV(color, out h, out s, out v);
		return new ColorHSV(h, s, v, color.a);
	}

	public static Color FromHSV(this ColorHSV hsv)
	{
		Color color = Color.HSVToRGB(hsv.h, hsv.s, hsv.v);
		color.a = hsv.a;
		return color;
	}

	// https://www.rapidtables.com/convert/color/rgb-to-hsl.html
	public static ColorHSL ToHSL(this Color color)
	{
		float min = Min(color.r, color.g, color.b);
		float max = Max(color.r, color.g, color.b);

		float h = 0f;
		float s = 0f;
		float l = (max + min) / 2f;

		float dx = max - min;
		if (Mathf.Abs(dx) < Epsilon)
		{
			h = 0f;
			s = 0f;
		}
		else
		{
			s = l <= 0.5f ? dx / (max + min) : dx / (2f - max - min);
			if (color.r == max)
			{
				h = (color.g - color.b) / dx;
			}
			else if (color.g == max)
			{
				h = (color.b - color.r) / dx + 2f;
			}
			else
			{
				h = (color.r - color.g) / dx + 4f;
			}

			h = h * 60f;
			if (h < 0f)
			{
				h += 360f;
			}
		}
		return new ColorHSL(h, s, l, color.a);
	}

	// https://www.rapidtables.com/convert/color/hsl-to-rgb.html
	public static Color FromHSL(this ColorHSL hsl)
	{
		float h = hsl.h;
		float c = (1f - Mathf.Abs(hsl.l * 2f - 1f)) * hsl.s;
		float x = c * (1f - Mathf.Abs(Modf(h / 60f, 2f) - 1f));
		float m = hsl.l - c / 2f;

		float r, g, b;
		if (h <= 60f)
		{
			r = c;
			g = x;
			b = 0;
		}
		else if (h < 120f)
		{
			r = x;
			g = c;
			b = 0;
		}
		else if (h < 180f)
		{
			r = 0;
			g = c;
			b = x;
		}
		else if (h < 240f)
		{
			r = 0;
			g = x;
			b = c;
		}
		else if (h < 300f)
		{
			r = x;
			g = 0;
			b = c;
		}
		else
		{
			r = c;
			g = 0;
			b = x;
		}
		return new Color(r + m, g + m, b + m, hsl.a);
	}

	private static float Min(float a, float b, float c)
	{
		return Mathf.Min(Math.Min(a, b), c);
	}

	private static float Max(float a, float b, float c)
	{
		return Mathf.Max(Math.Max(a, b), c);
	}

	public static float Modf(float a, float b)
	{
		return a - b * Mathf.Floor(a / b);
	}
}
