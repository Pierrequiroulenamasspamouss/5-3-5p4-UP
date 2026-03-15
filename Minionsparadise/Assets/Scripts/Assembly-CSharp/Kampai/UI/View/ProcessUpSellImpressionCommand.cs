namespace Kampai.UI.View
{
	public class ProcessUpSellImpressionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ProcessUpSellImpressionCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int salePackInstanceID { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.UI.View.StartUpSellImpressionSignal startUpSellImpressionSignal { get; set; }

		public override void Execute()
		{
			if (salePackInstanceID == 0)
			{
				logger.Error("sale instance id is 0 for this impression, returning.");
				return;
			}
			global::Kampai.Game.Sale byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Sale>(salePackInstanceID);
			if (byInstanceId == null)
			{
				return;
			}
			global::Kampai.Game.SalePackDefinition definition = byInstanceId.Definition;
			if (definition.Impressions != 0)
			{
				byInstanceId.Impressions++;
				if (byInstanceId.Impressions < byInstanceId.Definition.Impressions)
				{
					byInstanceId.UTCLastImpressionTime = timeService.CurrentTime();
					timeEventService.AddEvent(definition.ID, byInstanceId.UTCLastImpressionTime, definition.ImpressionInterval, startUpSellImpressionSignal);
				}
			}
		}
	}
}
