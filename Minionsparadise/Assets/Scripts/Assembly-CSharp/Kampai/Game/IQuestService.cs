namespace Kampai.Game
{
	public interface IQuestService
	{
		void Initialize();

		void RushQuestStep(int questId, int step);

		void UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType type, global::Kampai.Game.QuestTaskTransition questTaskTransition = global::Kampai.Game.QuestTaskTransition.Start, global::Kampai.Game.Building building = null, int buildingDefId = 0, int item = 0);

		void UpdateMasterPlanQuestLine();

		int IsOneOffCraftableDisplayable(int questDefinitionId, int trackedItemDefinitionID);

		bool IsQuestCompleted(int questDefinitionID);

		void SetQuestLineState(int questLineId, global::Kampai.Game.QuestLineState targetState);

		bool IsBridgeQuestComplete(int bridgeDefId);

		int GetLongestIdleQuestDuration();

		global::Kampai.Game.IQuestController GetLongestIdleQuestController();

		int GetIdleQuestDuration(int questDefinitionID);

		global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.QuestLine> GetQuestLines();

		global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.IQuestController> GetQuestMap();

		global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<int>> GetQuestUnlockTree();

		global::Kampai.Game.IQuestController GetQuestControllerByDefinitionID(int questDefinitionId);

		global::Kampai.Game.IQuestController GetQuestControllerByInstanceID(int questInstanceId);

		global::Kampai.Game.IQuestStepController GetQuestStepController(int questDefinitionID, int questStepIndex);

		global::Kampai.Game.IQuestController AddQuest(global::Kampai.Game.Quest quest);

		bool ContainsQuest(int questInstanceId);

		global::Kampai.Game.IQuestController AddQuest(global::Kampai.Game.MasterPlanComponent componentQuest, bool isBuildQuest);

		global::Kampai.Game.Quest GetQuestByInstanceId(int id);

		global::Kampai.Game.IQuestController AddMasterPlanQuest(global::Kampai.Game.MasterPlan masterPlanQuest);

		void RemoveQuest(global::Kampai.Game.IQuestController questController);

		void RemoveQuest(int questDefinitionID);

		string GetQuestName(string key, params object[] args);

		string GetEventName(string key, params object[] args);

		void UnlockMinionParty(int QuestDefinitionID);

		bool HasActiveQuest(int surfaceId);

		void SetPulseMoveBuildingAccept(bool enablePulse);

		bool ShouldPulseMoveButtonAccept();
	}
}
