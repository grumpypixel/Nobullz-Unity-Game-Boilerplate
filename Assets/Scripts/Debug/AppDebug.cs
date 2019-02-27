using UnityEngine;

namespace game
{
	public class AppDebug : MonoBehaviour
	{
		public bool					showDebugInfo = true;
		public int					guiDepth = 0;

	#if UNITY_DEBUG
		public Color				debugInfoColor = Color.magenta;
		public float				debugInfoRectBottomOffset;

		private FpsCounter			m_fpsCounter;
		private DebugConsole		m_debugConsole;
		private ErrorWindow			m_errorWindow;
		private GUIContent			m_debugInfoContent;
		private GUIStyle			m_debugInfoStyle;
	#endif

		void Awake()
		{
			Application.logMessageReceivedThreaded += HandleUnityLog;

		#if UNITY_DEBUG
			Log.SetLogDelegate(DebugLogDelegate);
			Debugger.SetBreakDelegate(DebugBreakDelegate);

			m_debugConsole = GetComponent<DebugConsole>();
			m_errorWindow = GetComponent<ErrorWindow>();
			m_fpsCounter = GetComponent<FpsCounter>();

			m_debugInfoStyle = new GUIStyle();
			m_debugInfoContent = new GUIContent(string.Empty);
		#endif
		}

	#if UNITY_DEBUG
		void OnGUI()
		{
			if (m_errorWindow.enabled)
			{
				return;
			}

			if (this.showDebugInfo)
			{
				long memory = System.GC.GetTotalMemory(false) >> 10;
				m_debugInfoContent.text = string.Format("Fps: {0:f0} Frame: {1} Mem: {2}kb", m_fpsCounter.fps, Time.frameCount, memory);

				GUI.depth = this.guiDepth;
				m_debugInfoStyle.normal.textColor = this.debugInfoColor;

				Vector2 size = m_debugInfoStyle.CalcSize(m_debugInfoContent);
				Rect rect = new Rect(0, Screen.height - size.y - this.debugInfoRectBottomOffset, Screen.width, size.y);
				GUI.Label(rect, m_debugInfoContent, m_debugInfoStyle);
			}
		}
	#endif

		void OnApplicationQuit()
		{
		#if UNITY_STANDALONE
			if (Screen.fullScreen)
			{
				Screen.fullScreen = false;
			}
		#endif

		#if UNITY_DEBUG
			Application.logMessageReceivedThreaded -= HandleUnityLog;
			Log.SetLogDelegate(null);
			Debugger.SetBreakDelegate(null);
		#endif

		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
		}

		private void DebugBreakDelegate(string format, params object[] args)
		{
		#if UNITY_DEBUG
			string errorMessage = string.Format(format, args);
			SetError(errorMessage);
			Log.Error(errorMessage);
			throw new System.ApplicationException(errorMessage);
		#endif
		}

		private void DebugLogDelegate(Log.Level logLevel, string format, params object[] args)
		{
		#if UNITY_DEBUG
			string output = FormatOutputString(format, args);

			switch (logLevel)
			{
				case Log.Level.Info:
					Debug.Log(output);
					break;
				case Log.Level.Warning:
					Debug.LogWarning(output);
					break;
				case Log.Level.Error:
					Debug.LogError(output);
					break;
				default:
					break;
			}
		#endif
		}

		private string FormatOutputString(string format, params object[] args)
		{
			string output = string.Empty;
		#if UNITY_DEBUG
			try
			{
				output = string.Format(format, args);
			}
			catch (System.ArgumentNullException)
			{
				output = "Could not format trace string <ArgumentNullException>!";
			}
			catch (System.FormatException)
			{
				output = "Could not format trace string <FormatException>!";
			}
		#endif
			return output;
		}

		private void HandleUnityLog(string message, string stackTrace, LogType logType)
		{
		#if UNITY_DEBUG
			if (logType == LogType.Error || logType == LogType.Exception)
			{
				SetError(string.Concat(message, "\n", stackTrace));
				m_debugConsole.Log(message, Color.red);
			}
			else if (Log.GetLogLevel() != Log.Level.None)
			{
				Color color = logType == LogType.Log ? Color.white : Color.yellow;
				m_debugConsole.Log(message, color);
			}
		#endif
		}

		private void SetError(string errorMessage)
		{
		#if UNITY_DEBUG
			m_errorWindow.errorText = errorMessage;
			m_errorWindow.enabled = true;
		#endif
		}
	}
}
