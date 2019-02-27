using UnityEngine;

namespace game
{
	public class DummyMessage : Message
	{
		public string text;

		public override void Reset()
		{
			text = string.Empty;
		}
	}
}
