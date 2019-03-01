using UnityEngine;

namespace game
{
	public class PlaySoundMessage : Message
	{
		public SfxId sfxId;

		public override void Reset()
		{
			sfxId = SfxId.None;
		}
	}

	public abstract class SfxPlayerBase : MonoBehaviour
	{
		public abstract AudioClip GetClip(int effectId, out float volume);

		protected SoundManager m_soundManager;

		public void Play(int effectId)
		{
			float volume;
			AudioClip clip = GetClip(effectId, out volume);
			if (clip != null)
			{
				m_soundManager.Play(clip, volume);
			}
		}

		public void Play(int effectId, Vector3 position)
		{
			float volume;
			AudioClip clip = GetClip(effectId, out volume);
			if (clip != null)
			{
				m_soundManager.Play(position, clip, volume);
			}
		}

		public void Play(int effectId, Transform target)
		{
			float volume;
			AudioClip clip = GetClip(effectId, out volume);
			if (clip != null)
			{
				m_soundManager.Play(target, clip, volume);
			}
		}

		public void StartLoop(int effectId, GameObject obj)
		{
			float volume;
			AudioClip clip = GetClip(effectId, out volume);
			if (clip != null)
			{
				m_soundManager.StartLoop(obj, effectId, clip, volume);
			}
		}

		public void StopLoop(int effectId, GameObject obj)
		{
			m_soundManager.StopLoop(obj, effectId);
		}

		protected virtual void Start()
		{
			m_soundManager = GameObject.FindObjectOfType<SoundManager>();
		}
	}
}
