using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class Spline
	{
		public struct Key
		{
			public Vector2 position;
			public Vector2 tangent;
			public float   time;

			public Key(Vector2 position)
			{
				this.position = position;
				this.tangent = Vector2.zero;
				this.time = 0f;
			}

			public Key(Vector2 position, Vector2 tangent, float time)
			{
				this.position = position;
				this.tangent = tangent;
				this.time = time;
			}
		}

		private List<Key> m_keys;
		private float     m_duration;

		public float duration
		{
			get { return m_duration; }
		}

		public int keyCount
		{
			get { return m_keys.Count; }
		}

		public Vector2 lastKeyPosition
		{
			get { return m_keys[m_keys.Count - 1].position; }
		}

		public float lastKeyTime
		{
			get { return m_keys[m_keys.Count - 1].time; }
		}

		public Spline(int initialCapacity = 128)
		{
			m_keys = new List<Key>(initialCapacity);
		}

		public Spline(Key[] keys) : base()
		{
			AddKeys(keys);
		}

		public void ClearKeys()
		{
			m_keys.Clear();
			m_duration = 0f;
		}

		public void AddKey(Key key)
		{
			m_keys.Add(key);
			m_duration = key.time;
		}

		public void AddKeys(Key[] keys)
		{
			int count = keys.Length;
			for (int i = 0; i < count; ++i)
			{
				AddKey(keys[i]);
			}
		}

		public void ReplaceLastKey(Key key)
		{
			int count = m_keys.Count;
			if (count > 0)
			{
				m_keys[count - 1] = key;
				m_duration = key.time;
			}
		}

		public Vector2 GetPosition(float time)
		{
			Vector2 a, b, c, d;
			float t = GetCVs(out a, out b, out c, out d, time);
			float omt = 1f - t;
			return a * (omt * omt * omt)
				+ b * (3f * omt * omt * t)
				+ c * (3f * omt * t * t)
				+ d * (t * t * t);
		}

		public Vector2 GetTangent(float time)
		{
			Vector2 a, b, c, d;
			float t = GetCVs(out a, out b, out c, out d, time);
			float t2 = 3f * t * t;

			return a * (-3f + 6f * t - t2)
				+ b * (3f - 12f * t + 3f * t2)
				+ c * (6f * t - 3f * t2)
				+ d * t2;
		}

		public void GetPositionAndTangent(float time, out Vector2 position, out Vector2 tangent)
		{
			Vector2 a, b, c, d;
			float t = GetCVs(out a, out b, out c, out d, time);
			float t2 = 3f * t * t;
			float omt = 1f - t;

			position = a * (omt * omt * omt)
				+ b * (3f * omt * omt * t)
				+ c * (3f * omt * t * t)
				+ d * (t * t * t);

			tangent = a * (-3f + 6f * t - t2)
				+ b * (3f - 12f * t + 3f * t2)
				+ c * (6f * t - 3f * t2)
				+ d * t2;
		}

		public void Dump()
		{
			Debug.Log(string.Format("Spline Dump keys:{0} duration:{1}", m_keys.Count, m_duration));
			for (int i = 0; i < m_keys.Count; ++i)
			{
				string s = string.Format("Key: {0} Pos: {1} Tangent: {2} Time: {3}", i, m_keys[i].position, m_keys[i].tangent, m_keys[i].time);
				Debug.Log(s);
			}
		}

		public Key GetKey(int index)
		{
			return m_keys[index];
		}

		public Vector2 GetKeyPosition(int index)
		{
			return m_keys[index].position;
		}

		public Vector2 GetKeyTangent(int index)
		{
			return m_keys[index].tangent;
		}

		public float GetKeyTime(int index)
		{
			return m_keys[index].time;
		}

		public void SetKeyTangent(int index, Vector2 tangent)
		{
			Spline.Key key = m_keys[index];
			key.tangent = tangent;
			m_keys[index] = key;
		}

		// Returns true if there are at least two keys
		public bool CalculateEndDirection(out Vector3 dir)
		{
			dir = Vector3.zero;
			int count = m_keys.Count;
			if(count > 1)
			{
				dir = m_keys[count-2].position - m_keys[count-1].position;
				dir.Normalize();
				return true;
			}
			return false;
		}

		private float GetCVs(out Vector2 a, out Vector2 b, out Vector2 c, out Vector2 d, float time)
		{
			time = Mathf.Clamp(time, 0f, m_duration);

			int index = GetIndex(time);
			float segmentLength = m_keys[index + 1].time - m_keys[index].time;
			float t = (time - m_keys[index].time) / segmentLength;
			float oneThirdSegmentLength = segmentLength / 3;

			a = m_keys[index].position;
			d = m_keys[index + 1].position;
			b = a + m_keys[index].tangent * oneThirdSegmentLength;
			c = d - m_keys[index + 1].tangent * oneThirdSegmentLength;

			return t;
		}

		private int GetIndex(float time)
		{
			int index;
			for (index = 0; index < m_keys.Count - 2; ++index)
			{
				if (time < m_keys[index + 1].time)
				{
					break;
				}
			}
			return index;
		}
	}
}
