namespace Kampai.Game
{
	public class AchievementService : global::Kampai.Game.IAchievementService
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.GOOGLEPLAY)]
		public global::Kampai.Game.ISocialService gpService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAchievementModalSignal showAchievementModalSignal { get; set; }

		public void ShowAchievements()
		{
			showAchievementModalSignal.Dispatch();
		}

		public global::Kampai.Game.Achievement GetAchievementByDefinitionID(int definitionID)
		{
			return playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Achievement>(definitionID);
		}

		public void UpdateIncrementalAchievement(int defID, int stepsCompleted)
		{
			global::Kampai.Game.AchievementDefinition achievementDefinitionFromDefinitionID = definitionService.GetAchievementDefinitionFromDefinitionID(defID);
			if (achievementDefinitionFromDefinitionID != null)
			{
				int steps = achievementDefinitionFromDefinitionID.Steps;
				int num = 0;
				global::Kampai.Game.Achievement firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Achievement>(achievementDefinitionFromDefinitionID.ID);
				if (firstInstanceByDefinitionId != null)
				{
					num = firstInstanceByDefinitionId.Progress;
				}
				playerService.CreateAndRunCustomTransaction(achievementDefinitionFromDefinitionID.ID, stepsCompleted, global::Kampai.Game.TransactionTarget.NO_VISUAL);
				num += stepsCompleted;
				float num2 = (float)num / (float)steps * 100f;
				if (num2 > 100f)
				{
					num2 = 100f;
				}
				global::Kampai.Game.AchievementID achievementID = achievementDefinitionFromDefinitionID.AchievementID;
				if (achievementID != null)
				{
					IncrementAchievement(achievementID, stepsCompleted, num2);
				}
			}
		}

		private void IncrementAchievement(global::Kampai.Game.AchievementID achievementID, int stepsCompleted, float percentComplete)
		{
			gpService.incrementAchievement(achievementID.GooglePlayID, stepsCompleted);
		}
	}
}
