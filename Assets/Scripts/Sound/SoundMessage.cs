using UnityEngine;

namespace game
{
	public class SoundMessage : Message
	{
		public SfxId sfxId;

		public override void Reset()
		{
			sfxId = SfxId.None;
		}
	}
}
