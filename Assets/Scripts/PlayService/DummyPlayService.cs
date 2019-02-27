using UnityEngine;

namespace game
{
	class DummyPlayService : IPlayService
	{
		public int highScore { get { return 0; } }
		public int totalScore { get { return 0; } }
		public int bestBlockCount { get { return 0; } }
		public int bestLineCount { get { return 0; } }
		public bool isAuthenticated { get { return m_isAuthenticated; } }
		public PlayServiceState state { get; set; }

		public DummyPlayService() {}
		public void Initialize(string applicationId) {}
		public void Shutdown() {}

		private bool m_isAuthenticated = false;

		public void SignIn(bool silent)
		{
			m_isAuthenticated = true;
		}

		public void SignOut()
		{
			m_isAuthenticated = false;
		}

		public void ShowLeaderboards()
		{
		#if UNITY_DEBUG
			Debug.Log("DummyPlayService.ShowLeaderboards");
		#endif
		}

		public void ShowAchievements()
		{
		#if UNITY_DEBUG
			Debug.Log("DummyPlayService.ShowAchievements");
		#endif
		}

		public void ReportScore(string leaderboardId, int score)
		{
		#if UNITY_DEBUG
			Debug.Log("DummyPlayService.ReportScore");
		#endif
		}

		public void LoadScore(string leaderboardId)
		{
		#if UNITY_DEBUG
			Debug.Log("DummyPlayService.LoadScore");
		#endif
		}

		public bool GetLoadedScore(string leaderboardId, out int loadedScore)
		{
		#if UNITY_DEBUG
			Debug.Log("DummyPlayService.GetLoadedScore");
		#endif
			loadedScore = 0;
			return true;
		}

		public void UnlockAchievement(string achievementId)
		{
		#if UNITY_DEBUG
			Debug.Log("DummyPlayService.UnlockAchievement");
		#endif
		}

		public void IncrementAchievement(string achievementId, int steps)
		{
		#if UNITY_DEBUG
			Debug.Log("DummyPlayService.IncrementAchievement");
		#endif
		}

		public void ReportAchievement(string achievementId, float percentage)
		{
		#if UNITY_DEBUG
			Debug.Log("DummyPlayService.ReportAchievement");
		#endif
		}

		public void RevealAchievement(string achievementId)
		{
		#if UNITY_DEBUG
			Debug.Log("DummyPlayService.RevealAchievement");
		#endif
		}
	}
}
