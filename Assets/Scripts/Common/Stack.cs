using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class Stack<T>
	{
		protected List<T> m_items;

		public int Size
		{
			get { return m_items.Count; }
		}

		public Stack(int initialCapacity = 0)
		{
			m_items = new List<T>(initialCapacity);
		}

		public void Clear()
		{
			m_items.Clear();
		}

		public void Push(T item)
		{
			m_items.Add(item);
		}

		public T Pop()
		{
			return RemoveAt(m_items.Count - 1);
		}

		public T Peek()
		{
			return GetAt(m_items.Count - 1);
		}

		protected T GetAt(int index)
		{
			return m_items[index];
		}

		protected T RemoveAt(int index)
		{
			T item = m_items[index];
			m_items.RemoveAt(index);
			return item;
		}

		public override string ToString()
		{
			string output = string.Format("Stack Count: {0} Values:", m_items.Count);
			for (int i = 0; i < m_items.Count; ++i)
			{
				output += " " + GetAt(i);
			}
			return output;
		}
	}
}
