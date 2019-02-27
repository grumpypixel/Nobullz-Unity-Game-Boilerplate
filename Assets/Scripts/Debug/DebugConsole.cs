using System.Collections.Generic;
using UnityEngine;

namespace game
{
	public class DebugConsole : GUIWindowBase
	{
		struct Entry
		{
			public string text;
			public Color color;

			public Entry(string text, Color color)
			{
				this.text = text;
				this.color = color;
			}
		}

		private List<Entry>		m_traces;
		private GUIContent		m_content = new GUIContent();
		private GUIStyle		m_style = new GUIStyle();
		private Vector2			m_scrollPosition;

		public void Log(string text, Color color)
		{
			m_traces.Add(new Entry(text, color));
		}

		void Awake()
		{
			this.caption = "Debug Console";
			m_traces = new List<Entry>();
			m_content = new GUIContent();
			m_style = new GUIStyle();
		}

		void OnDestroy()
		{
			m_traces = null;
			m_content = null;
			m_style = null;
		}

		protected override void HandleDrawWindow()
		{
			m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition);

			for (int i = 0; i < m_traces.Count; ++i)
			{
				m_content.text = m_traces[ i ].text;
				m_style.normal.textColor = m_traces[ i ].color;
				GUILayout.Label(m_content, m_style);
			}

			GUILayout.EndScrollView();

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Clear"))
				{
					m_traces.Clear();
				}
				if (GUILayout.Button("Close"))
				{
					this.enabled = false;
				}
			}
			GUILayout.EndHorizontal();
		}
	}
}
