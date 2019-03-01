using UnityEngine;

namespace game
{
	public class SfxHandler : MonoBehaviour
	{
		private SfxPlayer m_sfxPlayer;

		void Start()
		{
			m_sfxPlayer = GameObject.FindObjectOfType<SfxPlayer>();
			RegisterMessages();
		}

		void OnDisable()
		{
			DeregisterMessages();
		}

		private void RegisterMessages()
		{
			MessageCenter center = GameContext.messageCenter;
			if (center)
			{
				center.AddListener<SoundMessage>(HandleSoundMessage);
			}
		}

		private void DeregisterMessages()
		{
			MessageCenter center = GameContext.messageCenter;
			if (center)
			{
				center.RemoveListener<SoundMessage>(HandleSoundMessage);
			}
		}

		private void HandleSoundMessage(IMessageProvider provider)
		{
			if (m_sfxPlayer)
			{
				SoundMessage message = provider.GetMessage<SoundMessage>();
				m_sfxPlayer.Play((int)message.sfxId);
			}
		}
	}
}
