namespace Kampai.Game
{
	public class MinionPartyReadyCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MinionPartyReadyCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		public override void Execute()
		{
			logger.Debug("Minion Party is Ready");
			questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.ThrowParty);
		}
	}
}
