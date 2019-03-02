using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class GameSettings : MonoBehaviour
	{
		public int			highScore;
		public Volume		soundVolume;
		public Volume		musicVolume;

		void Awake()
		{
			Reset();
		}

		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}

		public void FromJson(string json)
		{
			JsonUtility.FromJsonOverwrite(json, this);
		}

		public void Reset()
		{
			this.highScore = 0;
			this.soundVolume = Volume.On;
			this.musicVolume = Volume.On;
		}
	}
}
