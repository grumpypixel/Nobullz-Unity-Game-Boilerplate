using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace game
{
	public class UIScreenFader : MonoBehaviour
	{
		public Image image;

		public void FadeIn(Color color, float duration, float delay = 0f, System.Action onComplete = null)
		{
			Fade(color, color, 1f, 0f, duration, delay, true, onComplete);
		}

		public void FadeOut(Color color, float duration, float delay = 0f, System.Action onComplete = null)
		{
			Fade(color, color, 0f, 1f, duration, delay, false, onComplete);
		}

		public void Fade(Color sourceColor, Color targetColor, float sourceAlpha, float targetAlpha, float duration, float delay, bool disableImageOnCompletion, System.Action onComplete)
		{
			if (this.image)
			{
				this.image.enabled = true;
			}
			else
			{
				delay = 0f;
				duration = 0f;
				sourceColor = targetColor;
			}

			sourceColor.a = sourceAlpha;
			targetColor.a = targetAlpha;
			StopAllCoroutines();
			StartCoroutine(DoFade(sourceColor, targetColor, duration, delay, disableImageOnCompletion, onComplete));
		}

		void Awake()
		{
			if (this.image)
			{
				this.image.enabled = false;
			}
		}

		void Start()
		{
		}

		private IEnumerator DoFade(Color sourceColor, Color targetColor, float duration, float delay, bool disableImageOnCompletion, System.Action onComplete)
		{
			if (sourceColor == targetColor)
			{
				if (this.image)
				{
					this.image.color = targetColor;
				}
				yield break;
			}

			if (delay > 0)
			{
				yield return new WaitForSeconds(delay);
			}

			if (duration <= 0)
			{
				if (this.image)
				{
					this.image.color = targetColor;
				}
				OnDone(disableImageOnCompletion, onComplete);
				yield break;
			}

			float startTime = Time.realtimeSinceStartup;
			while (true)
			{
				float diffTime = Time.realtimeSinceStartup - startTime;
				float value = Mathf.Clamp01(diffTime / duration);
				if (this.image)
				{
					this.image.color = Color.Lerp(sourceColor, targetColor, value);
				}

				if (value < 1)
				{
					yield return 0;
				}
				else
				{
					break;
				}
			}
			OnDone(disableImageOnCompletion, onComplete);
		}

		void OnDone(bool disableImageOnCompletion, System.Action onComplete)
		{
			if (disableImageOnCompletion && this.image)
			{
				this.image.enabled = false;
			}
			if (onComplete != null)
			{
				onComplete();
			}
		}
	}
}
