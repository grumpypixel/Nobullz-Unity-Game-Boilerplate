using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class Deque<T>
	{
		private List<T> m_items;

		public int Count
		{
			get { return m_items.Count; }
		}

		public Deque(int initialCapacity = 0)
		{
			m_items = new List<T>(initialCapacity);
		}

		public void Clear()
		{
			m_items.Clear();
		}

		public void PushFront(T item)
		{
			m_items.Insert(0, item);
		}

		public void PushBack(T item)
		{
			m_items.Add(item);
		}

		public T PopFront()
		{
			return RemoveAt(0);
		}

		public T PopBack()
		{
			return RemoveAt(m_items.Count - 1);
		}

		public T PeekFront()
		{
			return GetAt(0);
		}

		public T PeekBack()
		{
			return GetAt(m_items.Count - 1);
		}

		public T GetAt(int index)
		{
			return m_items[index];
		}

		public List<T> GetAll()
		{
			return m_items;
		}

		public void ReplaceAt(int index, T newItem)
		{
			m_items[index] = newItem;
		}

		private T RemoveAt(int index)
		{
			T item = m_items[index];
			m_items.RemoveAt(index);
			return item;
		}

		public override string ToString()
		{
			string output = string.Format("Deque Count: {0} Values:", m_items.Count);
			for (int i = 0; i < m_items.Count; ++i)
			{
				output += " " + GetAt(i);
			}
			return output;
		}
	}
}
