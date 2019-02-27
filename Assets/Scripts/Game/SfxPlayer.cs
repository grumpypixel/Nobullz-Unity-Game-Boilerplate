using UnityEngine;

namespace game
{
	public class SfxPlayer : SfxPlayerBase
	{
		public SoundBank button;

		public override AudioClip GetClip(int effectId, out float volume)
		{
			switch ((SfxId)effectId)
			{
				case SfxId.Button:
					return GetClipAndVolume(this.button, out volume);
				default:
					break;
			}
			volume = 1.0f;
			return null;
		}

		private AudioClip GetClipAndVolume(SoundBank bank, out float volume)
		{
			volume = bank.volume;
			return bank.GetNext();
		}
	}
}
