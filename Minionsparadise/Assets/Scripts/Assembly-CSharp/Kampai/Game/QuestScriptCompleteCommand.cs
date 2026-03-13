namespace Kampai.Game
{
	public class QuestScriptCompleteCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("QuestScriptCompleteCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.QuestScriptInstance questScriptInstance { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.IQuestController questControllerByDefinitionID = questService.GetQuestControllerByDefinitionID(questScriptInstance.QuestID);
			if (questControllerByDefinitionID != null)
			{
				questControllerByDefinitionID.OnQuestScriptComplete(questScriptInstance);
				return;
			}
			logger.Error("Quest Controller with definition Id {0} doesn't exist in quest map", questScriptInstance.QuestID);
		}
	}
}
