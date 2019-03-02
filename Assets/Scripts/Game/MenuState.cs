using UnityEngine;

namespace game
{
	public class MenuState : MonoBehaviour
	{
		void Awake()
		{
			GameContext.Initialize();
		}

		void Start()
		{
			RegisterMessages();
			LoadGame();
		}

		void OnDisable()
		{
			DeregisterMessages();
		}

		private void LoadGame()
		{
			GameSettings gameSettings = GameContext.gameSettings;
			GameContext.savegameManager.Load(gameSettings);

			SetSoundVolume(gameSettings.soundVolume);
		}

		private void SaveGame()
		{
			GameSettings gameSettings = GameContext.gameSettings;
			GameContext.savegameManager.Save(gameSettings);
		}

		private void SetSoundVolume(Volume volume)
		{
			GameContext.soundManager.muted = volume == Volume.Off;
		}

		private void RegisterMessages()
		{
			MessageCenter center = GameContext.messageCenter;
			center.AddListener<DummyMessage>(HandleDummyMessage);
		}

		private void DeregisterMessages()
		{
			MessageCenter center = GameContext.messageCenter;
			center.RemoveListener<DummyMessage>(HandleDummyMessage);
		}

		private void HandleDummyMessage(IMessageProvider provider)
		{
			DummyMessage message = provider.GetMessage<DummyMessage>();
			Debug.Log("Dummy Message: " + message.text);
		}

		public void OnPlayButtonPressed()
		{
			LoadSceneMessage loadSceneMessage = GameContext.messageDispatcher.AddMessage<LoadSceneMessage>();
			loadSceneMessage.scene = GameHelper.GetSceneName(SceneType.Game);
			GameHelper.PlaySound(SfxId.Button);
		}
	}
}
