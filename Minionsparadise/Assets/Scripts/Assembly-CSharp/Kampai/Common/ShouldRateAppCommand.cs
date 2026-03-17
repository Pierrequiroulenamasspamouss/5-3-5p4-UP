namespace Kampai.Common
{
	public class ShouldRateAppCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ShouldRateAppCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.ConfigurationDefinition.RateAppAfterEvent from { get; set; }

		[Inject]
		public ILocalPersistanceService persistService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowRateAppPanelSignal showRateAppPanelSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			if (!global::Kampai.Util.NetworkUtil.IsConnected())
			{
				return;
			}
			string dataPlayer = persistService.GetDataPlayer("RateApp");
			if (dataPlayer == "Disabled")
			{
				return;
			}
			if (configurationsService.GetConfigurations().rateAppAfter == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "No rate app configurations.");
				return;
			}
			bool value = false;
			if (!configurationsService.GetConfigurations().rateAppAfter.TryGetValue(from, out value))
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "No configuration value for key: {0}", from);
			}
			else
			{
				if (!value)
				{
					return;
				}
				uint quantity = playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
				if (playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) >= 7)
				{
					quantity -= 7;
					if (quantity % 2 == 0)
					{
						showRateAppPanelSignal.Dispatch();
					}
				}
			}
		}
	}
}
