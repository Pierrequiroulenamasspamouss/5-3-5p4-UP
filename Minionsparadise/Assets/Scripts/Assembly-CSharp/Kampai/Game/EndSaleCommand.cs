namespace Kampai.Game
{
	public class EndSaleCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("EndSaleCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateSaleBadgeSignal updateSaleBadgeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ReconcileSalesSignal reconcileSalesSignal { get; set; }

		[Inject]
		public int instanceId { get; set; }

		public override void Execute()
		{
			logger.Debug("Sale Ended: {0}", instanceId);
			global::Kampai.Game.Sale byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Sale>(instanceId);
			if (byInstanceId != null)
			{
				timeEventService.RemoveEvent(instanceId);
				playerService.AddUpsellToPurchased(byInstanceId.Definition.ID);
				byInstanceId.Finished = true;
				updateSaleBadgeSignal.Dispatch();
				reconcileSalesSignal.Dispatch(0);
			}
		}
	}
}
