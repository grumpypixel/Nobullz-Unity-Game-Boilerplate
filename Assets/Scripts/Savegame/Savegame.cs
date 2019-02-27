using UnityEngine;

namespace game
{
	public class Savegame : ScriptableObject
	{
		public int    version;
		public string hmac;
		public string data;

		public const string DefaultHmac = "super.secret.hmac";
		public const int Version = 1;

		public Savegame()
		{
			this.version = Version;
			this.hmac = DefaultHmac;
		}

		public void Initialize()
		{
			version = Version;
			hmac = DefaultHmac;
			data = "";
		}
	}
}
