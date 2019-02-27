using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
	public class LoadSceneMessage : Message
	{
		public string scene;

		public override void Reset()
		{
			scene = string.Empty;
		}
	}

	public class SceneLoader : MonoBehaviour
	{
		public Color          fadeInColor = Color.black;
		public Color          fadeOutColor = Color.black;

		public float          fadeInTime = 0.5f;
		public float          fadeOutTime = 0.5f;

		public bool           autoFadeIn = true;

		private UIScreenFader m_screenFader;
		private string        m_targetSceneName = "";
		private bool          m_loading = false;

		public bool isLoading
		{
			get { return m_loading; }
		}

		public void Load(string sceneName)
		{
			FadeOut(sceneName);
		}

		void Awake()
		{
			m_screenFader = GameObject.FindObjectOfType<UIScreenFader>();
		}

		void Start()
		{
			RegisterMessages();
		}

		void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void OnDisable()
		{
			DeregisterMessages();
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			Resources.UnloadUnusedAssets();
			System.GC.Collect();

			if (autoFadeIn)
			{
				FadeIn();
			}
		}

		public void FadeIn()
		{
			if (m_screenFader != null)
			{
				m_screenFader.FadeIn(this.fadeInColor, this.fadeInTime, 0f, this.OnFadeInComplete);
			}
		}

		public void FadeOut(string targetSceneName)
		{
			m_targetSceneName = targetSceneName;

			if (m_screenFader != null)
			{
				m_loading = true;
				m_screenFader.FadeOut(this.fadeOutColor, this.fadeOutTime, 0f, this.LoadGameScene);
			}
			else
			{
				LoadGameScene();
			}
		}

		private void OnFadeInComplete()
		{
			if (m_screenFader != null)
			{
				m_screenFader.enabled = false;
			}
		}

		private void LoadGameScene()
		{
			if (string.IsNullOrEmpty(m_targetSceneName) == false)
			{
				SceneManager.LoadScene(m_targetSceneName);
			}
		}

		private void RegisterMessages()
		{
			MessageCenter center = GameContext.messageCenter;
			if (center)
			{
				center.AddListener<LoadSceneMessage>(HandleLoadSceneMessage);
			}
		}

		private void DeregisterMessages()
		{
			MessageCenter center = GameContext.messageCenter;
			if (center)
			{
				center.RemoveListener<LoadSceneMessage>(HandleLoadSceneMessage);
			}
		}

		private void HandleLoadSceneMessage(IMessageProvider provider)
		{
			LoadSceneMessage message = provider.GetMessage<LoadSceneMessage>();
			Load(message.scene);
		}
	}
}
