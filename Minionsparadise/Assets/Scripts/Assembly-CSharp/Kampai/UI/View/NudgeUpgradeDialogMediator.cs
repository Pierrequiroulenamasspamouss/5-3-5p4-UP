namespace Kampai.UI.View
{
	public class NudgeUpgradeDialogMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.NudgeUpgradeDialogView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("NudgeUpgradeDialogMediator") as global::Kampai.Util.IKampaiLogger;

		private string storeUrl;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			storeUrl = args.Get<string>();
		}

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.titleText.text = "Update Available";
			base.view.messageText.text = localizationService.GetString("NudgeClientUpgradeMessage");
			base.view.upgradeText.text = "Update";
			base.view.cancelText.text = localizationService.GetString("CancelText");
			base.view.upgradeButton.ClickedSignal.AddListener(OnUpgradeClicked);
			base.view.cancelButton.ClickedSignal.AddListener(OnCancelClicked);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.upgradeButton.ClickedSignal.RemoveListener(OnUpgradeClicked);
			base.view.cancelButton.ClickedSignal.RemoveListener(OnCancelClicked);
		}

		private void OnUpgradeClicked()
		{
			GoToAppStore();
			Close();
		}

		private void OnCancelClicked()
		{
			Close();
		}

		private void GoToAppStore()
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Going to store to upgrade client!");
			if (!string.IsNullOrEmpty(storeUrl))
			{
				global::UnityEngine.Application.OpenURL(storeUrl);
			}
		}

		protected override void Close()
		{
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_NudgeUpgrade");
			hideSkrimSignal.Dispatch("ClientUpgradeSkrim");
		}
	}
}
