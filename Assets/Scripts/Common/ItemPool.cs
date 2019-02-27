using System;
using System.Collections.Generic;

namespace game
{
	public interface IPoolable
	{
		void Reset();
	}

	public class ItemPool
	{
		private List<IPoolable> m_pool;
		private int m_used;
		private Type m_itemType;

		public ItemPool()
		{
			m_pool = new List<IPoolable>();
			m_used = 0;
		}

		public T GetItem<T>() where T : IPoolable, new()
		{
			if (m_used < m_pool.Count)
			{
				return (T)m_pool[m_used++];
			}
			else
			{
				if (m_itemType == null)
				{
					m_itemType = typeof(T);
				}

				IPoolable newItem;
				if (m_itemType == typeof(T))
				{
					newItem = new T();
					newItem.Reset();
					m_pool.Add(newItem);
					m_used++;
				}
				else
				{
					throw new InvalidOperationException("ItemPool.GetItem");
				}

				return (T)newItem;
			}
		}

		public void ReleaseItem(IPoolable item)
		{
			int index = m_pool.IndexOf(item);
			item.Reset();
			m_pool.RemoveAt(index);
			m_used--;
			m_pool.Insert(m_used, item);
		}
	}
}
