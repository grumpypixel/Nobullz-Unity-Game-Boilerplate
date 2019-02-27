using UnityEngine;

namespace game
{
	public struct AABB2
	{
		public Vector2 min;
		public Vector2 max;

		public Vector2 center
		{
			get { return (min + max) * 0.5f; }
		}

		public Vector2 size
		{
			get { return new Vector2(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y)); }
		}

		public Vector2 extents
		{
			get { return new Vector2(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y)) * 0.5f; }
		}

		public AABB2(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		public void Zero()
		{
			min = max = Vector2.zero;
		}

		public void Invalidate()
		{
			min = new Vector2(float.MaxValue, float.MaxValue);
			max = new Vector2(float.MinValue, float.MinValue);
		}

		public void Validate()
		{
			Vector2 tmp = min;
			min = Vector2.Min(min, max);
			max = Vector2.Max(tmp, max);
		}

		public void Grow(Vector2 extents)
		{
			min -= extents;
			max += extents;
		}
	}
}
