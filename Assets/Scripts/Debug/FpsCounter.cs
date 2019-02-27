using UnityEngine;

namespace game
{
	public class FpsCounter : MonoBehaviour
	{
		public float	updateInterval = 1f;

		private float	m_accumTime;
		private float	m_timeLeft;
		private int		m_frames;

		public float fps
		{
			get; private set;
		}

		void Awake()
		{
			Reset();
		#if !UNITY_DEBUG
			this.enabled = false;
		#endif
		}

		void Update()
		{
			float timeStep = Time.deltaTime;
			float timeScale = Time.timeScale;

			if (timeStep > 0f)
			{
				m_timeLeft -= timeStep;
				m_accumTime += timeScale / timeStep;
				++m_frames;

				if (m_timeLeft <= 0f)
				{
					this.fps = m_accumTime / (float)m_frames;
					Reset();
				}
			}
		}

		private void Reset()
		{
			m_timeLeft = this.updateInterval;
			m_accumTime = 0f;
			m_frames = 0;
		}
	}
}
