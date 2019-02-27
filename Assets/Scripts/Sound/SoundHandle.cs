using UnityEngine;

namespace game
{
	public class SoundHandle
	{
		private Transform m_transform;

		public SoundHandle(AudioSource source, GameObject entity)
		{
			this.source = source;
			this.entity = entity;

			this.source.ignoreListenerPause = false;
			this.source.ignoreListenerVolume = false;

			m_transform = entity.transform;

			Free();
		}

		public enum State
		{
			Free,
			Local,
			Dynamic
		}

		public State state { get; set; }
		public Transform target { get; set; }
		public int effect { get; set; }
		public int instanceId { get;  set; }
		public AudioSource source { get; private set; }
		public GameObject entity { get; private set; }
		public bool isFree { get { return this.state == State.Free; } }
		public bool isPlaying { get { return this.source.isPlaying; } }
		public bool mute { set { this.source.mute = value; } }
		public void Stop() { this.source.Stop(); }
		public void Pause() { this.source.Pause(); }
		public void Play() { this.source.Play(); }

		public Vector3 position
		{
			get { return m_transform.position; }
			set { m_transform.position = value; }
		}

		public void Free()
		{
			if (m_transform != null)
			{
				this.position = Vector3.zero;
			}

			this.effect = 0;
			this.instanceId = -1;
			this.source.clip =  null;
			this.source.Stop();
			this.state = State.Free;
			this.target = null;
		}

		public void Update()
		{
			switch (this.state)
			{
				case State.Free:
					break;

				case State.Local:
					break;

				case State.Dynamic:
					if (this.target != null)
					{
						m_transform.position = this.target.position;
					}
					else
					{
						Free();
					}
					break;

				default:
					break;
			}

			if (this.source.isPlaying == false)
			{
				Free();
			}
		}
	}
}
