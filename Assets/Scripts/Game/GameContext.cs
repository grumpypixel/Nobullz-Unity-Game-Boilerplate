using UnityEngine;

namespace game
{
	public class GameContext : MonoBehaviour
	{
		private static CameraController		m_cameraController;
		private static GameSettings			m_gameSettings;
		private static MessageCenter		m_messageCenter;
		private static Prefabs				m_prefabs;
		private static SavegameManager		m_savegameManager;
		private static SfxPlayer			m_sfxPlayer;
		private static SoundManager			m_soundManager;

		public static CameraController cameraController { get { return m_cameraController; } }
		public static GameSettings gameSettings { get { return m_gameSettings; } }
		public static IMessageDispatcher messageDispatcher { get { return m_messageCenter; } }
		public static MessageCenter messageCenter { get { return m_messageCenter; } }
		public static Prefabs prefabs { get { return m_prefabs; } }
		public static SavegameManager savegameManager { get { return m_savegameManager; } }
		public static SfxPlayer sfxPlayer { get { return m_sfxPlayer; } }
		public static SoundManager soundManager { get { return m_soundManager; } }

		public static void Initialize()
		{
			m_cameraController = GameObject.FindObjectOfType<CameraController>();
			m_gameSettings = GameObject.FindObjectOfType<GameSettings>();
			m_messageCenter = GameObject.FindObjectOfType<MessageCenter>();
			m_prefabs = GameObject.FindObjectOfType<Prefabs>();
			m_savegameManager = GameObject.FindObjectOfType<SavegameManager>();
			m_sfxPlayer = GameObject.FindObjectOfType<SfxPlayer>();
			m_soundManager = GameObject.FindObjectOfType<SoundManager>();
		}
	}
}
