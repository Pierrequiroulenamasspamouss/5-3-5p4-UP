namespace Kampai.Game
{
	public class QuestHarvestableCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("QuestHarvestableCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.UpdateQuestBookBadgeSignal updateBadgeSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateQuestWorldIconsSignal updateQuestWorldIconsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Quest quest { get; set; }

		public override void Execute()
		{
			if (quest.state != global::Kampai.Game.QuestState.Harvestable)
			{
				logger.Warning("Trying to harvest quest when state is: {0}", quest.state);
				return;
			}
			updateBadgeSignal.Dispatch();
			if (quest.GetActiveDefinition().SurfaceType == global::Kampai.Game.QuestSurfaceType.Automatic || (quest.AutoGrantReward && quest.GetActiveDefinition().SurfaceType != global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated))
			{
				uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShowQuestRewardSignal>().Dispatch(quest.ID);
			}
			else
			{
				updateQuestWorldIconsSignal.Dispatch(quest);
			}
		}
	}
}
