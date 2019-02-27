using UnityEngine;

namespace game
{
	[System.Serializable]
	public class SoundBank
	{
		[Range(0.0f, 1.0f)]
		public float       volume = 1.0f;
		public bool        randomize = false;
		public AudioClip[] soundEffects;

		private int        m_current = 0;

		public bool hasSounds
		{
			get { return (this.soundEffects.Length > 0); }
		}

		public AudioClip GetFirst()
		{
			if (this.soundEffects.Length == 0)
			{
				return null;
			}
			if (this.randomize)
			{
				this.randomize = false;
			}
			m_current = 0;
			return this.soundEffects[m_current];
		}

		public AudioClip GetNext()
		{
			if (this.soundEffects.Length == 0)
			{
				return null;
			}

			if (this.randomize)
			{
				return GetRandomNext();
			}

			m_current++;
			if (m_current >= this.soundEffects.Length)
			{
				m_current = 0;
			}
			return this.soundEffects[m_current];
		}

		public AudioClip GetRandomNext()
		{
			int count = this.soundEffects.Length;
			if (count == 0)
			{
				return null;
			}

			if (count == 1)
			{
				return this.soundEffects[0];
			}

			int last = m_current;
			while (last == m_current)
			{
				m_current = UnityEngine.Random.Range(0, this.soundEffects.Length);
			}
			return this.soundEffects[m_current];
		}
	}
}
