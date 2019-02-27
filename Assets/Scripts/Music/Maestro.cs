using UnityEngine;

namespace game
{
	public class Maestro : MonoBehaviour
	{
		public AudioClip[]		tracks;

		public bool				playOnStart   = true;
		public bool				random        = true;

		[Range(0.0f, 1.0f)]
		public float			baseVolume    = 0.5f;
		public float			waitTime      = 0f;
		public float			startDelay    = 0f;

		private enum State
		{
			Idle,
			Load,
			Start,
			Play,
			Stopped,
			Advance,
			Wait
		}

		private AudioSource		m_audioSource;
		private bool			m_paused;

		private int				m_currentTrack = -1;
		private State			m_state = State.Idle;
		private float			m_delay = 0.0f;
		private float			m_wait = 0.0f;
		private bool			m_muted = false;

		public float volume { get; set; }

		public bool paused
		{
			get
			{
				return m_paused;
			}
			set
			{
				m_paused = value;

				if (m_audioSource != null)
				{
					if (m_paused)
					{
						m_audioSource.Pause();
					}
					else
					{
						m_audioSource.Play();
					}
				}
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
				bool wasMuted = m_muted;

				m_muted = value;
				m_audioSource.mute = value;

				if (wasMuted && !m_muted && m_state == State.Idle)
				{
					m_state = State.Load;
				}
			}
		}

		void Awake()
		{
			m_audioSource = gameObject.AddComponent<AudioSource>();
			m_audioSource.bypassEffects = true;
			m_audioSource.ignoreListenerVolume = true;

			this.volume = 1f;

			if (this.hasTracks)
			{
				if (this.random && this.tracks.Length > 1)
				{
					m_currentTrack = GetRandomTrackIndex();
				}
				else
				{
					m_currentTrack = 0;
				}
			}
			else
			{
				this.enabled = false;
			}
		}

		void Update()
		{
			if (!this.hasTracks)
			{
				return;
			}

			switch (m_state)
			{
				case State.Idle:
					if (this.playOnStart)
					{
						m_delay += Time.deltaTime;
						if (m_delay > this.startDelay)
						{
							m_state = State.Load;
						}
					}
					break;

				case State.Load:
					if (m_currentTrack >= 0)
					{
						if (this.tracks[m_currentTrack].loadState == AudioDataLoadState.Unloaded
							&& this.tracks[m_currentTrack].preloadAudioData == false)
						{
							this.tracks[m_currentTrack].LoadAudioData();
						}
						else if (this.tracks[m_currentTrack].loadState == AudioDataLoadState.Loaded)
						{
							m_audioSource.clip = this.tracks[m_currentTrack];
							m_state = State.Start;
						}
					}
					break;

				case State.Start:
					m_audioSource.Play();

					m_audioSource.volume = this.volume * this.baseVolume;
					m_audioSource.mute = m_muted;

					if (m_audioSource.isPlaying)
					{
						m_state = State.Play;
					}
					break;

				case State.Play:
					if (!m_audioSource.isPlaying && !m_paused)
					{
						m_state = State.Advance;
					}
					break;

				case State.Advance:
					if (this.random)
					{
						m_currentTrack = GetRandomTrackIndex();
					}
					else
					{
						m_currentTrack = GetNextTrackIndex();
					}

					m_state = State.Wait;
					m_wait = 0.0f;
					break;

				case State.Wait:
					m_wait += Time.deltaTime;
					if (m_wait > this.waitTime)
					{
						m_state = State.Load;
					}
					break;

				default:
					break;
			}
		}

		public void Play()
		{
			if (!this.hasTracks)
			{
				return;
			}

			if (m_audioSource.clip == null)
			{
				m_state = State.Load;
				return;
			}

			if (!m_audioSource.isPlaying)
			{
				m_state = State.Start;
			}
		}

		public void Stop()
		{
			if (m_audioSource.isPlaying)
			{
				m_audioSource.Stop();
				m_state = State.Stopped;
			}
		}

		private bool hasTracks
		{
			get { return this.tracks.Length > 0; }
		}

		private int GetNextTrackIndex()
		{
			return (this.hasTracks ? m_currentTrack = (m_currentTrack + 1) % this.tracks.Length : -1);
		}

		private int GetRandomTrackIndex()
		{
			int next = m_currentTrack;
			int last = m_currentTrack;

			if (this.tracks.Length > 1)
			{
				while (last == next)
				{
					next = UnityEngine.Random.Range(0, this.tracks.Length);
				}
			}
			return next;
		}
	}
}
