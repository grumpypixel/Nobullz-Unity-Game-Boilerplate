using UnityEngine;

namespace game
{
	public class Savegame : ScriptableObject
	{
		public int			version;
		public string		hmac;
		public string		data;

		public const int	Version = 1;
		public const string	DefaultHmac = "super.secret.hmac";

		public Savegame()
		{
			this.version = Version;
			this.hmac = DefaultHmac;
			this.data = null;
		}

		public void Initialize()
		{
			this.version = Version;
			this.hmac = DefaultHmac;
			data = string.Empty;
		}
	}
}
