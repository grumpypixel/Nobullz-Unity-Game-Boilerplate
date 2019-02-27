using System;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class KeyEventMessage : Message
	{
		public KeyEvent keyEvent;
		public KeyCode keyCode;

		public override void Reset()
		{
			keyEvent = KeyEvent.None;
			keyCode = KeyCode.None;
		}
	}

	public enum KeyEvent
	{
		None = -1,
		Down = 0,
		Up = 1,
		Pressed = 2
	}

   	public class KeyboardInput : MonoBehaviour
	{
		private class Data
		{
			public KeyEvent keyEvent;
			public List<KeyCode> keys;
			public Func<KeyCode, bool> func;

			public Data(KeyEvent keyEvent, Func<KeyCode, bool> func)
			{
				this.keyEvent = keyEvent;
				this.keys = new List<KeyCode>();
				this.func = func;
			}
		}

		private IMessageDispatcher m_messageDispatcher;
		private List<Data> m_data = new List<Data>(3);

		public void AddKey(KeyEvent keyEvent, KeyCode keyCode)
		{
			if (keyEvent == KeyEvent.None || keyCode == KeyCode.None)
			{
				throw new ArgumentException("keyEvent == None or keyCode == None");
			}

			Data data = GetData(keyEvent);
			Debugger.Assert(data != null);
			if (!data.keys.Contains(keyCode))
			{
				data.keys.Add(keyCode);
			}
		}

		public void RemoveKey(KeyEvent keyEvent, KeyCode keyCode)
		{
			if (keyEvent == KeyEvent.None || keyCode == KeyCode.None)
			{
				throw new ArgumentException("keyEvent == None or keyCode == None");
			}

			Data data = GetData(keyEvent);
			Debugger.Assert(data != null);
			data.keys.Remove(keyCode);
		}

		void Awake()
		{
			m_data.Add(new Data(KeyEvent.Down, Input.GetKeyDown));
			m_data.Add(new Data(KeyEvent.Up, Input.GetKeyUp));
			m_data.Add(new Data(KeyEvent.Pressed, Input.GetKey));
		}

		void Start()
		{
			m_messageDispatcher = GameContext.messageDispatcher;
		}

		void Update()
		{
			int count = m_data.Count;
			for (int i = 0; i < count; ++i)
			{
				Data data = m_data[i];
				int numKeys = data.keys.Count;
				for (int k = 0; k < numKeys; ++k)
				{
					if (data.func(data.keys[k]))
					{
						SendKeyEventMessage(data.keyEvent, data.keys[k]);
					}
				}
			}
		}

		private Data GetData(KeyEvent keyEvent)
		{
			foreach (Data data in m_data)
			{
				if (data.keyEvent == keyEvent)
				{
					return data;
				}
			}
			return null;
		}

		private void SendKeyEventMessage(KeyEvent keyEvent, KeyCode keyCode)
		{
			KeyEventMessage message = m_messageDispatcher.AddMessage<KeyEventMessage>();
			message.keyEvent = keyEvent;
			message.keyCode = keyCode;
		}
	}
}
