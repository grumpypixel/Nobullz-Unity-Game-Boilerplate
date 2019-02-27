using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class SoundManager : MonoBehaviour
	{
		[Range(8, 64)]
		public int m_numAudioSources = 16;

		[Range(0.0f, 1.0f)]
		public float m_baseVolume = 1.0f;

		[Range(0.0f, 1.0f)]
		public float m_panLevel = 0.3f;

		private List<SoundHandle> m_handles;
		private bool m_muted = false;
		private float m_volume = 1f;

		public float volume
		{
			get { return m_volume; }
			set { m_volume = value; }
		}

		public void Play(AudioClip clip, float volume = 1.0f)
		{
			SoundHandle handle = GetFreeHandle();

			handle.source.loop   = false;
			handle.source.volume = this.volume * volume;
			handle.state         = SoundHandle.State.Local;
			handle.target        = null;

			handle.source.clip = clip;
			handle.source.Play();
		}

		public void Play(Vector3 position, AudioClip clip, float volume = 1.0f)
		{
			SoundHandle handle = GetFreeHandle();

			handle.position      = position;
			handle.source.loop   = false;
			handle.source.volume = this.volume * volume;
			handle.state         = SoundHandle.State.Local;
			handle.target        = null;

			handle.source.clip = clip;
			handle.source.Play();
		}

		public void Play(Transform target, AudioClip clip, float volume = 1.0f)
		{
			SoundHandle handle = GetFreeHandle();

			handle.position      = target.position;
			handle.source.loop   = false;
			handle.source.volume = this.volume * volume;
			handle.state         = SoundHandle.State.Dynamic;
			handle.target        = target;

			handle.source.clip = clip;
			handle.source.Play();
		}

		public void StartLoop(GameObject obj, int sfxId, AudioClip clip, float volume = 1.0f)
		{
			int id = obj.GetInstanceID();
			int count = m_handles.Count; // ???

			for (int i = 0; i < count; ++i)
			{
				if (m_handles[ i ].instanceId == id
					&& m_handles[ i ].effect == sfxId
					&& m_handles[ i ].source.loop == true
					&& m_handles[ i ].isFree)
				{
					if (m_handles[ i ].source.isPlaying == false)
					{
						m_handles[ i ].source.Play();
					}
					return;
				}
			}

			Transform target = obj.transform;
			SoundHandle handle = GetFreeHandle();

			handle.effect        = sfxId;
			handle.instanceId    = id;
			handle.position      = target.position;
			handle.source.loop   = true;
			handle.source.volume = m_baseVolume * volume;
			handle.state         = SoundHandle.State.Dynamic;
			handle.target        = target;

			handle.source.clip = clip;
			handle.source.Play();
		}

		public void StopLoop(GameObject obj, int sfxId)
		{
			int id = obj.GetInstanceID();
			int count = m_handles.Count;
			for (int i = 0; i < count; ++i)
			{
				if (m_handles[ i ].instanceId == id
					&& m_handles[ i ].effect == sfxId)
				{
					m_handles[ i ].Free();
					return;
				}
			}
		}

		public void StopAll(GameObject obj)
		{
			int id = obj.GetInstanceID();
			int count = m_handles.Count;
			for (int i = 0; i < count; ++i)
			{
				if (m_handles[ i ].instanceId == id)
				{
					m_handles[ i ].Free();
				}
			}
		}

		public void StopAll()
		{
			int count = m_handles.Count;
			for (int i = 0; i < count; ++i)
			{
				m_handles[ i ].Stop();
			}
		}

		public bool muted
		{
			get
			{
				return m_muted;
			}
			set
			{
				m_muted = value;
				int count = m_handles.Count;
				for (int i = 0; i < count; ++i)
				{
					m_handles[ i ].source.mute = m_muted;
				}
			}
		}

		void Awake()
		{
			m_handles = new List<SoundHandle>(m_numAudioSources);
			for (int i = 0; i < m_numAudioSources; ++i)
			{
				SoundHandle handle = CreateHandle();
		#if UNITY_EDITOR
				handle.entity.SetActive(false);
		#endif
				m_handles.Add(handle);
			}
		}

		void Update()
		{
			int count = m_handles.Count;
			for (int i = 0; i < count; ++i)
			{
				m_handles[ i ].Update();
				m_handles[ i ].source.spatialBlend = m_panLevel;
			}
		}

		private SoundHandle GetFreeHandle()
		{
			int count = m_handles.Count;
			for (int i = 0; i < count; ++i)
			{
				if (m_handles[ i ].isFree)
				{
		#if UNITY_EDITOR
					m_handles[ i ].entity.SetActive(true);
		#endif
					return m_handles[ i ];
				}
			}
			SoundHandle newHandle = CreateHandle();
			m_handles.Add(newHandle);
		#if UNITY_EDITOR
			Log.Warning("Created new sound handle {0}", m_handles.Count);
		#endif
			return newHandle;
		}

		private SoundHandle CreateHandle()
		{
			GameObject entity = new GameObject("SfxSource");
			entity.transform.parent = this.transform;
			AudioSource source = entity.AddComponent<AudioSource>();
			source.spatialBlend = m_panLevel;
			source.mute = m_muted;
			return new SoundHandle(source, entity);
		}
	}
}
