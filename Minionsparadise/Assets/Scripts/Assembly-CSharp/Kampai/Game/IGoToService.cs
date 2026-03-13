namespace Kampai.Game
{
	public interface IGoToService
	{
		void GoToClicked(global::Kampai.Game.QuestStep step, global::Kampai.Game.QuestStepDefinition stepDefinition, global::Kampai.Game.IQuestController questController, int stepNumber);

		void GoToClicked(global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition taskDefinition);

		void GoToBuildingFromItem(int itemDefID);

		void OpenStoreFromAnywhere(int buildingDefinitionID);
	}
}
