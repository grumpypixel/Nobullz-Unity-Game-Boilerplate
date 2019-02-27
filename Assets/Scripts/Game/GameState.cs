using UnityEngine;

namespace game
{
	public class GameState : MonoBehaviour
	{
		void Awake()
		{
			GameContext.Initialize();
		}

		void Start()
		{
			RegisterMessages();
		}

		void OnDisable()
		{
			DeregisterMessages();
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

		public void OnHomeButtonPressed()
		{
			LoadSceneMessage loadSceneMessage = GameContext.messageDispatcher.AddMessage<LoadSceneMessage>();
			loadSceneMessage.scene = GameHelper.GetSceneName(SceneType.Menu);
			GameHelper.PlaySound(SfxId.Button);
		}
	}
}
