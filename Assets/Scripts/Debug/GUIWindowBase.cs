using UnityEngine;

namespace game
{
	public class GUIWindowBase : MonoBehaviour
	{
		protected virtual void HandleDrawWindow() {}

		private static int	s_m_id = 0;

		private Rect		m_windowRect = new Rect(0, 0, Screen.width, Screen.height);
		private int			m_windowId = ++s_m_id;
		private string		m_caption = "GUIWindow";

		public int windowId { get { return m_windowId; } }
		public Rect windowRect { get { return m_windowRect; } }
		public int windowWidth { get { return System.Convert.ToInt32(m_windowRect.width); } }

		public string caption
		{
			get { return m_caption; }
			set { m_caption = value; }
		}

	#if UNITY_DEBUG
		void OnGUI()
		{
			HandleScreenRect(ref m_windowRect);
			GUI.Window(m_windowId, m_windowRect, DrawWindow, m_caption);
		}
	#endif

		private void DrawWindow(int id)
		{
			HandleDrawWindow();
		}

		protected virtual void HandleScreenRect(ref Rect screenRect)
		{
			if (screenRect.width != Screen.width)
			{
				screenRect.width = Screen.width;
			}

			if (screenRect.height != Screen.height)
			{
				screenRect.height = Screen.height;
			}
		}
	}
}
