namespace Kampai.ForcedUpgrade.View
{
	public class ForcedUpgradeMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ForcedUpgradeMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.ForcedUpgrade.View.ForcedUpgradeView view { get; set; }

		[Inject]
		public global::Kampai.Main.InitLocalizationServiceSignal initLocalizationServiceSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		public override void OnRegister()
		{
			initLocalizationServiceSignal.Dispatch();
			view.titleText.text = localizationService.GetString("ClientUpgradeTitle");
			view.messageText.text = localizationService.GetString("ForcedClientUpgradeMessage");
			view.buttonText.text = localizationService.GetString("Update");
			view.storeButton.ClickedSignal.AddListener(OnStoreClick);
		}

		public override void OnRemove()
		{
			view.storeButton.ClickedSignal.RemoveListener(OnStoreClick);
		}

		private void OnStoreClick()
		{
			logger.Debug("Going to the store!");
			global::UnityEngine.Application.OpenURL("market://details?id=com.ea.gp.minions");
		}
	}
}
