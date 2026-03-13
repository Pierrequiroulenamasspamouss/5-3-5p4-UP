namespace Kampai.Game
{
	public interface IAchievementService
	{
		void ShowAchievements();

		void UpdateIncrementalAchievement(int defID, int stepsCompleted);
	}
}
