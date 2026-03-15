namespace Kampai.UI.View
{
	public class AboutPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("AboutPanelMediator") as global::Kampai.Util.IKampaiLogger;

		private bool usageSharingEnabled;

		[Inject]
		public global::Kampai.UI.View.AboutPanelView view { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IClientVersion clientVersion { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService loc { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UsageSharingAcceptedSignal usageSharingAccepted { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistanceService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService defService { get; set; }

		public override void OnRegister()
		{
			Init();
			view.usageSharingToggleButton.ClickedSignal.AddListener(UsageSharingClicked);
			view.termsOfServiceButton.ClickedSignal.AddListener(TermsOfServiceClicked);
			view.privacyPolicyButton.ClickedSignal.AddListener(PrivacyPolicyClicked);
			view.eulaButton.ClickedSignal.AddListener(EulaClicked);
			view.creditsButton.ClickedSignal.AddListener(CreditsClicked);
			usageSharingAccepted.AddListener(UsageSharingClosed);
		}

		public override void OnRemove()
		{
			view.usageSharingToggleButton.ClickedSignal.RemoveListener(UsageSharingClicked);
			view.termsOfServiceButton.ClickedSignal.RemoveListener(TermsOfServiceClicked);
			view.privacyPolicyButton.ClickedSignal.RemoveListener(PrivacyPolicyClicked);
			view.eulaButton.ClickedSignal.RemoveListener(EulaClicked);
			view.creditsButton.ClickedSignal.RemoveListener(CreditsClicked);
			usageSharingAccepted.RemoveListener(UsageSharingClosed);
		}

		private void Init()
		{
			view.aboutTitleText.text = string.Format("© {0}\n{1}{2}", loc.GetString("title-copyright-info"), loc.GetString("version-info"), clientVersion.GetClientVersion());
			usageSharingEnabled = telemetryService.SharingUsageEnabled();
			view.privacyPolicyText.text = loc.GetString("PrivacyPolicyLabel");
			view.creditsText.text = loc.GetString("CreditsLabel");
			SetUsageText();
			view.termsOfServiceText.text = loc.GetString("TermsOfServiceLabel");
			view.eulaText.text = loc.GetString("EULALabel");
		}

		private void SetUsageText()
		{
			if (usageSharingEnabled)
			{
				view.usageSharingText.text = loc.GetString("DisableUsageSharing");
			}
			else
			{
				view.usageSharingText.text = loc.GetString("EnableUsageSharing");
			}
		}

		private void UsageSharingClicked()
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			view.usageSharingPanel.SetActive(true);
		}

		private void UsageSharingClosed(bool change)
		{
			view.usageSharingPanel.SetActive(false);
			if (change)
			{
				ToggleUsageSharing();
			}
		}

		private void ToggleUsageSharing()
		{
			usageSharingEnabled = !usageSharingEnabled;
			if (!usageSharingEnabled)
			{
				telemetryService.Send_Telemetry_EVT_USER_TRACKING_OPTOUT();
			}
			telemetryService.SharingUsage(usageSharingEnabled);
			SetUsageText();
			logger.Info("Usage Sharing: {0}", (!usageSharingEnabled) ? "DISABLED" : "ENABLED");
		}

		private void CreditsClicked()
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			view.creditsPanel.SetActive(true);
		}

		private void TermsOfServiceClicked()
		{
			localPersistanceService.PutData("ExternalLinkOpened", "True");
			global::Kampai.Util.LegalDocuments.TermsOfServiceClicked(loc, soundFXSignal, logger, defService);
		}

		private void PrivacyPolicyClicked()
		{
			localPersistanceService.PutData("ExternalLinkOpened", "True");
			global::Kampai.Util.LegalDocuments.PrivacyPolicyClicked(loc, soundFXSignal, logger, defService);
		}

		private void EulaClicked()
		{
			localPersistanceService.PutData("ExternalLinkOpened", "True");
			global::Kampai.Util.LegalDocuments.EulaClicked(loc, soundFXSignal, logger, defService);
		}
	}
}
