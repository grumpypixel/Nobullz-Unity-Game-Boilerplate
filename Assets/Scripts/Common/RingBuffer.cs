using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class RingBuffer<T>
	{
		private T[]	m_values;
		private int	m_first;
		private int	m_count;

		public int Count
		{
			get { return m_count; }
		}

		public int Capacity
		{
			get { return m_values.Length; }
		}

		public RingBuffer(int capacity)
		{
			Initialize(capacity);
		}

		public void Clear()
		{
			m_first = 0;
			m_count = 0;
		}

		public void Add(T value)
		{
			if (m_count == m_values.Length)
			{
				m_values[m_first] = value;
				m_first = (m_first + 1) % m_values.Length;
			}
			else
			{
				m_values[m_first + m_count] = value;
			}

			m_count = Mathf.Min(m_count + 1, m_values.Length);
		}

		public T GetAt(int index)
		{
			return m_values[(m_first + index) % m_values.Length];
		}

		public List<T> ToList()
		{
			List<T> values = new List<T>(m_count);
			for (int i = 0; i < m_count; ++i)
			{
				values.Add(GetAt(i));
			}
			return values;
		}

		public void Set(List<T> values, int capacity)
		{
			Initialize(capacity);
			foreach (T value in values)
			{
				Add(value);
			}
		}

		public override string ToString()
		{
			string output = string.Format("RingBuffer Capacity: {0} Count: {1} Values:", this.Capacity, this.Count);
			for (int i = 0; i < m_count; ++i)
			{
				output += " " + GetAt(i);
			}
			return output;
		}

		private void Initialize(int capacity)
		{
			m_values = new T[capacity];
			Clear();
		}
	}
}
