using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
	public class MainState : MonoBehaviour
	{
		public bool   runAppInBackground = true;
		public int    appTargetFrameRate = -1;
		public string targetScene;

		void Awake()
		{
			Application.runInBackground = this.runAppInBackground;
			Application.targetFrameRate = this.appTargetFrameRate;
		}

		IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();

			if (string.IsNullOrEmpty(this.targetScene) == false)
			{
				SceneManager.LoadScene(this.targetScene);
			}
		}
	}
}
