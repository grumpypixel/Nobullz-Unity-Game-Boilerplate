using UnityEngine;
using UnityEngine.EventSystems;

namespace game
{
	public class TouchBeganMessage : Message
	{
		public Vector2 touchPosition;
		public bool isPointerOverUIObject;

		public override void Reset()
		{
			touchPosition = Vector2.zero;
			isPointerOverUIObject = false;
		}
	}

	public class TouchMovedMessage : Message
	{
		public Vector2 touchPosition;
		public Vector2 deltaPosition;
		public float deltaTime;

		public override void Reset()
		{
			touchPosition = Vector2.zero;
			deltaPosition = Vector2.zero;
			deltaTime = 0f;
		}
	}

	public class TouchEndedMessage : Message
	{
		public Vector2 touchPosition;
		public Vector2 deltaPosition;
		public float deltaTime;
		public bool canceled;

		public override void Reset()
		{
			touchPosition = Vector2.zero;
			deltaPosition = Vector2.zero;
			deltaTime = 0f;
			canceled = false;
		}
	}

	public class TouchInput : MonoBehaviour
	{
		private IMessageDispatcher m_messageDispatcher;
		private Vector2 m_lastPointerPosition;
		private float m_lastPointerTime;
		private int m_fingerId = -1;

		void Start()
		{
			m_messageDispatcher = GameContext.messageDispatcher;
		}

		void Update()
		{
			if (m_fingerId == -1)
			{
		#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
				if (Input.touchCount > 0)
				{
					Touch touch = Input.GetTouch(0);
					if (touch.phase == TouchPhase.Began)
					{
						m_fingerId = 0;
						SendTouchBeganMessage(touch.position, m_fingerId);
					}
				}
		#else
				if (Input.GetMouseButtonDown(0))
				{
					m_fingerId = 0;
					m_lastPointerPosition = Input.mousePosition;
					m_lastPointerTime = Time.time;
					SendTouchBeganMessage(m_lastPointerPosition, m_fingerId);
				}
		#endif
			}
			else
			{
		#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
				int touchCount = Input.touchCount;
				for (int i = 0; i < touchCount; ++i)
				{
					Touch touch = Input.GetTouch(i);
					if (touch.fingerId == m_fingerId)
					{
						if (touch.phase == TouchPhase.Moved)
						{
							SendTouchMovedMessage(touch.position, touch.deltaPosition, touch.deltaTime);
						}
						else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
						{
							m_fingerId = -1;
							bool canceled = touch.phase == TouchPhase.Canceled;
							SendTouchEndedMessage(touch.position, touch.deltaPosition, touch.deltaTime, canceled);
						}
					}
				}
		#else
				if (m_fingerId == 0)
				{
					Vector2 position = Input.mousePosition;
					float now = Time.time;

					Vector2 deltaPosition = position - m_lastPointerPosition;
					float deltaTime = now - m_lastPointerTime;
					SendTouchMovedMessage(position, deltaPosition, deltaTime);

					m_lastPointerPosition = position;
					m_lastPointerTime = now;
				}
				if (Input.GetMouseButtonUp(0))
				{
					m_fingerId = -1;
					Vector2 position = Input.mousePosition;
					float now = Time.time;

					Vector2 deltaPosition = position - m_lastPointerPosition;
					float deltaTime = now - m_lastPointerTime;
					SendTouchEndedMessage(position, deltaPosition, deltaTime, false);

					m_lastPointerPosition = position;
					m_lastPointerTime = now;
				}
		#endif
			}
		}

		private void SendTouchBeganMessage(Vector2 position, int fingerId)
		{
			TouchBeganMessage message = m_messageDispatcher.AddMessage<TouchBeganMessage>();
			message.touchPosition = position;
			message.isPointerOverUIObject = IsPointerOverGameObject(fingerId);
		}

		private void SendTouchMovedMessage(Vector2 position, Vector2 deltaPosition, float deltaTime)
		{
			TouchMovedMessage message = m_messageDispatcher.AddMessage<TouchMovedMessage>();
			message.touchPosition = position;
			message.deltaPosition = deltaPosition;
			message.deltaTime = deltaTime;
		}

		private void SendTouchEndedMessage(Vector2 position, Vector2 deltaPosition, float deltaTime, bool canceled)
		{
			TouchEndedMessage message = m_messageDispatcher.AddMessage<TouchEndedMessage>();
			message.touchPosition = position;
			message.deltaPosition = deltaPosition;
			message.deltaTime = deltaTime;
			message.canceled = canceled;
		}

		private bool IsPointerOverGameObject(int fingerId)
		{
			EventSystem eventSystem = EventSystem.current;
			if (eventSystem == null)
			{
				return false;
			}
		#if UNITY_EDITOR
			return (eventSystem.IsPointerOverGameObject() && eventSystem.currentSelectedGameObject != null);
		#else
			return (eventSystem.IsPointerOverGameObject(fingerId) && eventSystem.currentSelectedGameObject != null);
		#endif
		}
	}
}
