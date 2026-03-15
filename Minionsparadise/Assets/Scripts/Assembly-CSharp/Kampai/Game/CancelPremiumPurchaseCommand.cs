namespace Kampai.Game
{
	public class CancelPremiumPurchaseCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CancelPremiumPurchaseCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public string ExternalIdentifier { get; set; }

		[Inject]
		public uint errorCode { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowBillingNotAvailablePanelSignal showBillingNotAvailablePanelSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SavePlayerSignal savePlayerSignal { get; set; }

		public override void Execute()
		{
			logger.Debug("[NCS] CancelPremiumPurchaseCommand.Execute: ExternalIdentifier = {0}, cancel Kampai pending tr-n", ExternalIdentifier);
			playerService.CancelPendingTransaction(ExternalIdentifier);
			switch (errorCode)
			{
			case 20000u:
				logger.Debug("[NCS] CancelPremiumPurchaseCommand.Execute: show billing not available UI, error {0}", errorCode);
				showBillingNotAvailablePanelSignal.Dispatch();
				break;
			case 20006u:
			case 20019u:
			case 30001u:
				logger.Debug("[NCS] CancelPremiumPurchaseCommand.Execute: skip error UI dialog on error: {0}", errorCode);
				break;
			default:
			{
				logger.Debug("[NCS] CancelPremiumPurchaseCommand.Execute: show cancel tr-n UI, errorCode: {0}", errorCode);
				string type = localService.GetString("CancelTransaction");
				popupMessageSignal.Dispatch(type, global::Kampai.UI.View.PopupMessageType.QUEUED);
				break;
			}
			}
			savePlayerSignal.Dispatch(new global::Kampai.Util.Tuple<global::Kampai.Game.SaveLocation, string, bool>(global::Kampai.Game.SaveLocation.REMOTE, string.Empty, false));
		}
	}
}
