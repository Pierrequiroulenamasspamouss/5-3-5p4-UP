namespace Kampai.Game
{
	public interface IMasterPlanQuestService
	{
		global::Kampai.Game.Quest GetQuestByInstanceId(int id);

		global::System.Collections.Generic.List<global::Kampai.Game.Quest> GetQuests();

		global::Kampai.Game.QuestLine ConvertComponentToQuestLine(global::Kampai.Game.MasterPlanComponent component);

		global::Kampai.Game.QuestLine ConvertMasterPlanToQuestLine(global::Kampai.Game.MasterPlan masterPlan);

		global::Kampai.Game.Quest ConvertMasterPlanToQuest(global::Kampai.Game.MasterPlan masterPlan);

		global::Kampai.Game.Quest ConvertMasterPlanComponentToQuest(global::Kampai.Game.MasterPlanComponent component);

		global::Kampai.Game.Quest ConvertMasterPlanComponentToQuest(global::Kampai.Game.MasterPlanComponent component, bool buildTask);

		global::Kampai.Game.QuestStep ConvertMasterPlanComponentTaskToQuestStep(global::Kampai.Game.MasterPlanComponentTask task);

		global::Kampai.Game.QuestStepDefinition ConvertMasterPlanComponentTaskDefToQuestStepDef(global::Kampai.Game.MasterPlanComponentTaskDefinition task);

		global::Kampai.Game.QuestDefinition ConvertMasterPlanComponentDefToQuestDef(global::Kampai.Game.MasterPlanComponent component, int questLineId);
	}
}
