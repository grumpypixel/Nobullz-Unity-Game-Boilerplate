using UnityEngine;

namespace game
{
	public enum MouseButtonEvent
	{
		None = -1,
		Down,
		Up,
		Pressed
	}

	public class MouseButtonMessage : Message
	{
		public MouseButtonEvent buttonEvent;
		public int button;
		public Vector2 mousePosition;

		public override void Reset()
		{
			buttonEvent = MouseButtonEvent.None;
			button = -1;
			mousePosition = Vector2.zero;
		}
	}

	public class MouseMoveMessage : Message
	{
		public Vector2 mousePosition;
		public Vector2 deltaPosition;
		public float deltaTime;

		public override void Reset()
		{
			mousePosition = Vector2.zero;
			deltaPosition = Vector2.zero;
			deltaTime = 0f;
		}
	}

	public class MouseInput : MonoBehaviour
	{
		private IMessageDispatcher m_messageDispatcher;
		private Vector2 m_lastMousePosition;
		private float m_lastTime;

		void Start()
		{
			m_messageDispatcher = GameContext.messageDispatcher;
			m_lastMousePosition = Input.mousePosition;
			m_lastTime = Time.time;
		}

		void Update()
		{
			UpdateMouseButtons();
			UpdateMouseMovement();
		}

		private void UpdateMouseButtons()
		{
			Vector2 mousePosition = Input.mousePosition;
			for (int i = 0; i < 3; ++i)
			{
				if (Input.GetMouseButtonDown(i))
				{
					SendMouseButtonMessage(MouseButtonEvent.Down, i, mousePosition);
				}
				if (Input.GetMouseButtonUp(i))
				{
					SendMouseButtonMessage(MouseButtonEvent.Up, i, mousePosition);
				}
				if (Input.GetMouseButton(i))
				{
					SendMouseButtonMessage(MouseButtonEvent.Pressed, i, mousePosition);
				}
			}
		}

		private void UpdateMouseMovement()
		{
			Vector2 mousePosition = Input.mousePosition;
			Vector2 deltaPosition = mousePosition - m_lastMousePosition;
			if (deltaPosition.x != 0f || deltaPosition.y != 0f)
			{
				float now = Time.time;
				SendMouseMoveMessage(mousePosition, deltaPosition, now - m_lastTime);
				m_lastMousePosition = mousePosition;
				m_lastTime = now;
			}
		}

		private void SendMouseButtonMessage(MouseButtonEvent buttonEvent, int button, Vector2 mousePosition)
		{
			MouseButtonMessage message = m_messageDispatcher.AddMessage<MouseButtonMessage>();
			message.buttonEvent = buttonEvent;
			message.button = button;
			message.mousePosition =  mousePosition;
		}

		private void SendMouseMoveMessage(Vector2 mousePosition, Vector2 deltaPosition, float deltaTime)
		{
			MouseMoveMessage message = m_messageDispatcher.AddMessage<MouseMoveMessage>();
			message.mousePosition = mousePosition;
			message.deltaPosition = deltaPosition;
			message.deltaTime = deltaTime;
		}
	}
}
