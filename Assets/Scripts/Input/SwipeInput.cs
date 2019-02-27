using UnityEngine;

namespace game
{
	public enum SwipeDirection
	{
		None       = 0x0,
		Left       = 0x1,
		Right      = 0x2,
		Up         = 0x4,
		Down       = 0x8,
		Horizontal = (Left | Right),
		Vertical   = (Up | Down),
		All        = (Horizontal | Vertical)
	}

	public class SwipeMessage : Message
	{
		public SwipeDirection direction;
		public Vector2 beginPosition;
		public Vector2 endPosition;
		public float velocity;

		public override void Reset()
		{
			direction = SwipeDirection.None;
			beginPosition = Vector2.zero;
			endPosition = Vector2.zero;
			velocity = 0f;
		}
	}

	public class SwipeInput : MonoBehaviour
	{
		public SwipeDirection      swipeFilter = SwipeDirection.All;
		public float               swipeTime = 0.5f;
		public float               minSwipeDistanceCm = 2f;
		public float               maxSwipeVariance = 1f;

		private IMessageDispatcher m_messageDispatcher;
		private Vector2            m_origin;
		private float              m_time;
		private SwipeDirection     m_state = SwipeDirection.None;
		private bool               m_isSwiping = false;

		private float              m_pixelsPerCm = 1f;

		private Pair<KeyCode, SwipeDirection>[] m_swipeKeys = new Pair<KeyCode, SwipeDirection>[]
		{
			new Pair<KeyCode, SwipeDirection>(KeyCode.UpArrow, SwipeDirection.Up),
			new Pair<KeyCode, SwipeDirection>(KeyCode.RightArrow, SwipeDirection.Right),
			new Pair<KeyCode, SwipeDirection>(KeyCode.DownArrow, SwipeDirection.Down),
			new Pair<KeyCode, SwipeDirection>(KeyCode.LeftArrow, SwipeDirection.Left),
		};

		void Awake()
		{
 			m_pixelsPerCm = UnityHelper.GetScreenPixelsPerCentimeter();
		}

		void Start()
		{
			m_messageDispatcher = GameContext.messageDispatcher;
 			RegisterMessages();
		}

		void OnDisable()
		{
			DeregisterMessages();
		}

	#if UNITY_EDITOR
		const float KeySwipeDistance = 100f;
		void Update()
		{
			for (int i = 0; i < m_swipeKeys.Length; ++i)
			{
				if (Input.GetKeyDown(m_swipeKeys[i].first))
				{
					m_origin = Vector2.zero;
					SendSwipeMessage(m_swipeKeys[i].second, KeySwipeDistance, this.swipeTime, Vector2.zero);
					m_isSwiping = false;
					break;
				}
			}
		}
	#endif

		private void RegisterMessages()
		{
			MessageCenter center = GameContext.messageCenter;
			center.AddListener<TouchBeganMessage>(HandleTouchBeganMessage);
			center.AddListener<TouchMovedMessage>(HandleTouchMovedMessage);
			center.AddListener<TouchEndedMessage>(HandleTouchEndedMessage);
		}

		private void DeregisterMessages()
		{
			MessageCenter center = GameContext.messageCenter;
			center.RemoveListener<TouchBeganMessage>(HandleTouchBeganMessage);
			center.RemoveListener<TouchMovedMessage>(HandleTouchMovedMessage);
			center.RemoveListener<TouchEndedMessage>(HandleTouchEndedMessage);
		}

		private void HandleTouchBeganMessage(IMessageProvider provider)
		{
			TouchBeganMessage message = provider.GetMessage<TouchBeganMessage>();
			m_origin = message.touchPosition;
			m_time = Time.time;
			m_state = SwipeDirection.None;
			m_isSwiping = true;
		}

		private void HandleTouchMovedMessage(IMessageProvider provider)
		{
			if (m_isSwiping)
			{
				TouchMovedMessage message = provider.GetMessage<TouchMovedMessage>();
				if (DetectSwipe(message.touchPosition, message.deltaPosition))
				{
					m_isSwiping = false;
				}
			}
		}

		private void HandleTouchEndedMessage(IMessageProvider provider)
		{
			m_isSwiping = false;
		}

		private bool DetectSwipe(Vector2 position, Vector2 deltaPosition)
		{
			float elapsed = Time.time - m_time;
			if (this.swipeTime > 0 && elapsed > this.swipeTime)
			{
				return false;
			}

			UpdateState(deltaPosition);

			float dx = Mathf.Abs(position.x - m_origin.x) / m_pixelsPerCm;
			float dy = Mathf.Abs(position.y - m_origin.y) / m_pixelsPerCm;

			if (CheckSwipe(SwipeDirection.Left, dx, dy, elapsed, position)
				|| CheckSwipe(SwipeDirection.Right, dx, dy, elapsed, position)
				|| CheckSwipe(SwipeDirection.Up, dy, dx, elapsed, position)
				|| CheckSwipe(SwipeDirection.Down, dy, dx, elapsed, position))
			{
				return true;
			}
			return false;
		}

		private void UpdateState(Vector2 deltaPosition)
		{
			if (deltaPosition.x != 0 || deltaPosition.y != 0)
			{
				if (deltaPosition.x > 0)
				{
					m_state |= SwipeDirection.Right;
					m_state &= ~SwipeDirection.Left;
				}
				else if (deltaPosition.x < 0)
				{
					m_state |= SwipeDirection.Left;
					m_state &= ~SwipeDirection.Right;
				}

				if (deltaPosition.y > 0)
				{
					m_state |= SwipeDirection.Up;
					m_state &= ~SwipeDirection.Down;
				}
				else if (deltaPosition.y < 0)
				{
					m_state |= SwipeDirection.Down;
					m_state &= ~SwipeDirection.Up;
				}
			}
		}

		private bool CheckSwipe(SwipeDirection direction, float distance, float variance, float elapsedTime, Vector2 position)
		{
			if ((this.swipeFilter & direction) != 0)
			{
				if ((m_state & direction) != 0)
				{
					if (distance > this.minSwipeDistanceCm)
					{
						if (variance <= this.maxSwipeVariance)
						{
							SendSwipeMessage(direction, distance, elapsedTime, position);
							return true;
						}
						else
						{
							m_state &= ~direction;
						}
					}
				}
			}
			return false;
		}

		private void SendSwipeMessage(SwipeDirection direction, float distance, float elapsedTime, Vector2 position)
		{
			SwipeMessage message = m_messageDispatcher.AddMessage<SwipeMessage>();
			message.direction = direction;
			message.beginPosition = m_origin;
			message.endPosition = position;
			message.velocity = distance / elapsedTime;
		}
	}
}
