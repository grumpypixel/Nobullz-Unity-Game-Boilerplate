using UnityEngine;

public static class Vector2Extensions
{
	public static Vector2 Rotate(this Vector2 v, float degrees)
	{
		float rad = degrees * Mathf.Deg2Rad;
		float sin = Mathf.Sin(rad);
		float cos = Mathf.Cos(rad);
		float tx = v.x;
		float ty = v.y;
		return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
	}
}
