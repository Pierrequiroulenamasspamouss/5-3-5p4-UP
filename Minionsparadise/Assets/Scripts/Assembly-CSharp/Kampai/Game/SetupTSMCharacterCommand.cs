namespace Kampai.Game
{
	public class SetupTSMCharacterCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupTSMCharacterCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDefinitionService DefinitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService TimeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.CheckTriggersSignal TriggersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CreateNamedCharacterViewSignal CreateNamedCharacterViewSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateQuestWorldIconsSignal UpdateQuestWorldIconsSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner RoutineRunner { get; set; }

		public override void Execute()
		{
			if (PlayerService.GetHighestFtueCompleted() < 9)
			{
				logger.Info("Ignoring setup tsm signal since we are still in the ftue!");
				return;
			}
			global::Kampai.Game.TSMCharacterDefinition tSMCharacterDefinition = DefinitionService.Get<global::Kampai.Game.TSMCharacterDefinition>(70008);
			if (tSMCharacterDefinition == null)
			{
				logger.Error("Failed to find TSM Character Definition");
				return;
			}
			global::Kampai.Game.TSMCharacter firstInstanceByDefinitionId = PlayerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.TSMCharacter>(70008);
			if (firstInstanceByDefinitionId == null)
			{
				logger.Error("Failed to find TSM Character in player inventory");
				return;
			}
			global::Kampai.Game.Quest firstInstanceByDefinitionId2 = PlayerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Quest>(77777);
			if (firstInstanceByDefinitionId2 == null)
			{
				TimeEventService.AddEvent(firstInstanceByDefinitionId.ID, firstInstanceByDefinitionId.PreviousTaskUTCTime, firstInstanceByDefinitionId.Definition.CooldownInSeconds, TriggersSignal);
			}
			else
			{
				RoutineRunner.StartCoroutine(ShowTSMCharacter(firstInstanceByDefinitionId));
			}
		}

		private global::System.Collections.IEnumerator ShowTSMCharacter(global::Kampai.Game.TSMCharacter existingCharacter)
		{
			yield return null;
			global::Kampai.Game.Quest quest = PlayerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Quest>(77777);
			if (quest != null && !existingCharacter.Created)
			{
				CreateNamedCharacterViewSignal.Dispatch(existingCharacter);
				UpdateQuestWorldIconsSignal.Dispatch(quest);
			}
			else
			{
				logger.Warning("Quest id: {0} was not found in player inventory or TSM character is already created!");
			}
		}
	}
}
