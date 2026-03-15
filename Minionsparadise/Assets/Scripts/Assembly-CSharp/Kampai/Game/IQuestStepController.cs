namespace Kampai.Game
{
	public interface IQuestStepController
	{
		string DeliverButtonLocKey { get; }

		global::Kampai.Game.QuestStepType StepType { get; }

		global::Kampai.Game.QuestStepState StepState { get; }

		int StepInstanceTrackedID { get; }

		int ItemDefinitionID { get; }

		bool NeedActiveDeliverButton { get; }

		bool NeedActiveProgressBar { get; }

		bool NeedGoToButton { get; }

		int AmountNeeded { get; }

		int ProgressBarAmount { get; }

		int ProgressBarTotal { get; }

		string GetStepAction(global::Kampai.Main.ILocalizationService localService);

		string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService);

		void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite);

		void SetupTracking();

		int IsTrackingOneOffCraftable(int itemDefinitionID);

		void GoToNextState(bool isTaskComplete = false);

		void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId);
	}
}
