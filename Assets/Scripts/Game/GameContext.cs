using UnityEngine;

namespace game
{
	public class GameContext : MonoBehaviour
	{
		private static CameraController		m_cameraController;
		private static MessageCenter		m_messageCenter;
		private static Prefabs				m_prefabs;
		private static SfxPlayer			m_sfxPlayer;

		public static CameraController cameraController { get { return m_cameraController; } }
		public static IMessageDispatcher messageDispatcher { get { return m_messageCenter; } }
		public static MessageCenter messageCenter { get { return m_messageCenter; } }
		public static Prefabs prefabs { get { return m_prefabs; } }
		public static SfxPlayer sfxPlayer { get { return m_sfxPlayer; } }

		public static void Initialize()
		{
			m_cameraController = GameObject.FindObjectOfType<CameraController>();
			m_messageCenter = GameObject.FindObjectOfType<MessageCenter>();
			m_prefabs = GameObject.FindObjectOfType<Prefabs>();
			m_sfxPlayer = GameObject.FindObjectOfType<SfxPlayer>();
		}
	}
}
