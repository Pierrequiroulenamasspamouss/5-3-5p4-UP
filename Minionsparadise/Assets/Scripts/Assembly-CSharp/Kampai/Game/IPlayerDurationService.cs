namespace Kampai.Game
{
	public interface IPlayerDurationService
	{
		int TotalSecondsSinceLevelUp { get; }

		int GameplaySecondsSinceLevelUp { get; }

		int TotalGamePlaySeconds { get; }

		void InitLevelUpUTC();

		void MarkLevelUpUTC();

		void SetLastGameStartUTC();

		void UpdateGameplayDuration();

		void OnAppPause();

		void OnAppResume();

		int GetGameTimeDuration(global::Kampai.Game.IGameTimeTracker gameTimeTracker);
	}
}
