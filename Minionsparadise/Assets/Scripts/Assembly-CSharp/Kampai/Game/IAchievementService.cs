namespace Kampai.Game
{
	public interface IAchievementService
	{
		void ShowAchievements();

		void UpdateIncrementalAchievement(int defID, int stepsCompleted);

		global::Kampai.Game.Achievement GetAchievementByDefinitionID(int definitionID);

	}
}
