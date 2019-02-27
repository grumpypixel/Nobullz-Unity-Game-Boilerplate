using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace game
{
	public abstract class Message : IPoolable
	{
		public virtual void Reset() {}
	}

	public interface IMessageProvider
	{
		T GetMessage<T>() where T : Message, new();
	}

	public interface IMessageDispatcher
	{
		T AddMessage<T>() where T : Message, new();
	}

	public class MessageCenter : MonoBehaviour, IMessageProvider, IMessageDispatcher
	{
		private class MessageEvent : UnityEvent<IMessageProvider> {}

		private Dictionary<Type, MessageEvent>	m_listeners;
		private Dictionary<Type, ItemPool>		m_pool;
		private List<Message>					m_messages;
		private List<Message>					m_pendingMessages;
		private Message							m_currentMessage;

		private static MessageCenter s_instance;

		public static MessageCenter instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = FindObjectOfType(typeof(MessageCenter)) as MessageCenter;
					if (s_instance == null)
					{
						Debug.LogError("No active MessageCenter found in scene.");
					}
				}
				return s_instance;
			}
		}

		void Awake()
		{
			m_listeners = new Dictionary<Type, MessageEvent>();
			m_pool = new Dictionary<Type, ItemPool>();
			m_messages = new List<Message>();
			m_pendingMessages = new List<Message>();
		}

		void LateUpdate()
		{
			ProcessMessages();
		}

		public void AddListener<T>(UnityAction<IMessageProvider> listener)
		{
			MessageEvent evt = null;
			if (m_listeners.TryGetValue(typeof(T), out evt) == false)
			{
				evt = new MessageEvent();
				m_listeners.Add(typeof(T), evt);
			}
			evt.AddListener(listener);
		}

		public void RemoveListener<T>(UnityAction<IMessageProvider> listener)
		{
			MessageEvent evt = null;
			if (m_listeners.TryGetValue(typeof(T), out evt))
			{
				evt.RemoveListener(listener);
			}
			else
			{
				Debug.LogWarning("Could not find listener for type " + typeof(T));
			}
		}

		public T AddMessage<T>() where T : Message, new()
		{
			Type type = typeof(T);
			if (m_pool.ContainsKey(type) == false)
			{
				m_pool[type] = new ItemPool();
			}

			ItemPool pool = m_pool[type];
			T item = pool.GetItem<T>();
			item.Reset();

			if (m_currentMessage == null)
			{
				m_messages.Add(item);
			}
			else
			{
				m_pendingMessages.Add(item);
			}

			return item;
		}

		public T GetMessage<T>() where T : Message, new()
		{
			return (T)m_currentMessage;
		}

		private void ReleaseMessage(Message message)
		{
			ItemPool pool;
			if (m_pool.TryGetValue(message.GetType(), out pool))
			{
				pool.ReleaseItem(message);
			}
		}

		public void ProcessMessages()
		{
			if (m_messages == null)
			{
				return;
			}

			int count = m_messages.Count;
			for (int i = 0; i < count; ++i)
			{
				Message message = m_messages[i];

				MessageEvent evt = null;
				if (m_listeners.TryGetValue(message.GetType(), out evt) == false)
				{
					continue;
				}

				m_currentMessage = message;

				evt.Invoke(this);

				ReleaseMessage(m_currentMessage);
			}

			m_currentMessage = null;
			m_messages.Clear();

			if (m_pendingMessages.Count > 0)
			{
				m_messages.AddRange(m_pendingMessages);
				m_pendingMessages.Clear();
			}
		}
	}
}
