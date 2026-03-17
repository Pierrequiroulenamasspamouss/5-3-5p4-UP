namespace Kampai.Game
{
	public interface IQuestController
	{
		int ID { get; }

		global::Kampai.Game.Quest Quest { get; }

		int StepCount { get; }

		int QuestIconTrackedInstanceID { get; }

		bool AreAllStepsComplete { get; }

		global::Kampai.Game.QuestDefinition Definition { get; }

		global::Kampai.Game.QuestState State { get; }

		bool AutoGrantReward { get; set; }

		global::Kampai.Game.IQuestStepController GetStepController(int stepIndex);

		int GetIdleTime();

		global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> GetRequiredQuantityItems();

		void SetUpTracking();

		bool IsTrackingThisBuilding(int buildingID, global::Kampai.Game.QuestStepType StepType);

		int IsTrackingOneOffCraftable(int itemDefinitionID);

		void DeleteQuest();

		void OnQuestScriptComplete(global::Kampai.Game.QuestScriptInstance questScriptInstance);

		void RushQuestStep(int stepIndex);

		void UpdateTask(global::Kampai.Game.QuestStepType stepType, global::Kampai.Game.QuestTaskTransition questTaskTransition = global::Kampai.Game.QuestTaskTransition.Start, global::Kampai.Game.Building building = null, int buildingDefId = 0, int itemDefId = 0);

		void GoToQuestState(global::Kampai.Game.QuestState targetState);

		void CheckAndUpdateQuestCompleteState();

		void ProcessAutomaticQuest();

		void Debug_SetQuestToInProgressIfNotAlready();
	}
}
