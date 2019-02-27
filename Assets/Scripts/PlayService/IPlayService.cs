namespace game
{
	public enum PlayServiceState
	{
		NotAuthenticated,
		InProgress,
		Authenticated,
		FailedToAuthenticate,
		Error
	}

	interface IPlayService
	{
		bool isAuthenticated { get; }
		PlayServiceState state { get; }

		void Initialize(string applicationId);
		void Shutdown();

		void SignIn(bool silent);
		void SignOut();

		void ShowLeaderboards();
		void ShowAchievements();

		void ReportScore(string leaderboardId, int score);
		void LoadScore(string leaderboardId);
		bool GetLoadedScore(string leaderboardId, out int loadedScore);
		void UnlockAchievement(string achievementId);
		void IncrementAchievement(string achievementId, int steps);
		void ReportAchievement(string achievementId, float percentage);
		void RevealAchievement(string achievementId);
	}
}
