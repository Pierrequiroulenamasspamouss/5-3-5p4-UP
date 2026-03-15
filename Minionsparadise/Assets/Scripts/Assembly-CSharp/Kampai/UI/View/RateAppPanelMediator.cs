namespace Kampai.UI.View
{
	public class RateAppPanelMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.RateAppPanelView>
	{
		private bool? userAccepted;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenRateAppPageSignal openRateAppPageSignal { get; set; }

		[Inject]
		public ILocalPersistanceService persistService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.closeButton.ClickedSignal.AddListener(Close);
			base.view.rateButton.ClickedSignal.AddListener(Rate);
			base.view.notNowButton.ClickedSignal.AddListener(NotNow);
			base.view.neverButton.ClickedSignal.AddListener(NeverRate);
			userAccepted = null;
			soundFXSignal.Dispatch("Play_menu_popUp_01");
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.closeButton.ClickedSignal.RemoveListener(Close);
			base.view.rateButton.ClickedSignal.RemoveListener(Rate);
			base.view.notNowButton.ClickedSignal.RemoveListener(NotNow);
			base.view.neverButton.ClickedSignal.RemoveListener(NeverRate);
			hideSkrimSignal.Dispatch("RateAppSkrim");
		}

		protected override void Close()
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "RateAppPanel");
			telemetryService.Send_Telemetry_EVT_RATE_MY_APP("Game Prompt", userAccepted);
		}

		private void Rate()
		{
			userAccepted = true;
			persistService.PutDataPlayer("RateApp", "Disabled");
			Close();
			openRateAppPageSignal.Dispatch();
		}

		private void NotNow()
		{
			userAccepted = false;
			Close();
		}

		private void NeverRate()
		{
			userAccepted = false;
			persistService.PutDataPlayer("RateApp", "Disabled");
			Close();
		}
	}
}
