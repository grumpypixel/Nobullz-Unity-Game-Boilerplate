using UnityEngine;

namespace game
{
	public class ErrorWindow : GUIWindowBase
	{
		public bool breakOnError = true;

		public string errorText { set; private get; }

		void Start()
		{
			this.caption = "Error";
			this.errorText = string.Empty;
		}

		protected override void HandleDrawWindow()
		{
			GUI.FocusWindow(this.windowId);
			GUI.BringWindowToFront(this.windowId);

			GUILayout.BeginVertical();
			{
				GUILayout.Label(this.errorText);
				if (GUILayout.Button("Close"))
				{
					this.enabled = false;
				}
			}
			GUILayout.EndVertical();

			if (this.breakOnError)
			{
				Debug.Break();
			}
		}
	}
}
