namespace Kampai.UI.View
{
	public class SettingsMenuMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.SettingsMenuView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SettingsMenuMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.UI.View.SettingsMenuPanel? currentPanel;

		private bool isTempHidden;

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.GOOGLEPLAY)]
		public global::Kampai.Game.ISocialService googleService { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowSocialPartyFBConnectSignal showFacebookPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginSignal socialLoginSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLogoutSignal socialLogoutSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateFacebookStateSignal facebookStateSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SaveDevicePrefsSignal saveSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginSuccessSignal loginSuccess { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginFailureSignal loginFailure { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenRateAppPageSignal openRateAppPageSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.TogglePopupForHUDSignal togglePopupSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestScriptService questService { get; set; }

		[Inject]
		public global::Kampai.Common.IVideoService videoService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.UI.View.TempHideSettingsMenuSignal tempHidden { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDebugButtonSignal displayDebugButtonSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IAchievementService achievementService { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDisco3DElements displayDisco3DElements { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.facebookButton.ClickedSignal.AddListener(FacebookButton);
			base.view.googleButton.ClickedSignal.AddListener(GoogleButton);
			base.view.rateAppButton.ClickedSignal.AddListener(RateAppButton);
			base.view.closeButton.ClickedSignal.AddListener(CloseButton);
			base.view.achievementButton.ClickedSignal.AddListener(AchievementButton);
			facebookStateSignal.AddListener(setFacebookStatus);
			setFacebookStatus(facebookService.isLoggedIn);
			closeSignal.AddListener(Close);
			loginSuccess.AddListener(LoginSuccess);
			loginFailure.AddListener(LoginFailure);
			base.view.settings.ClickedSignal.AddListener(ShowSettings);
			base.view.about.ClickedSignal.AddListener(ShowAbout);
			base.view.help.ClickedSignal.AddListener(ShowHelp);
			base.view.playMovieButton.ClickedSignal.AddListener(PlayMovieButton);
			tempHidden.AddListener(TempHide);
			init();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.facebookButton.ClickedSignal.RemoveListener(FacebookButton);
			base.view.rateAppButton.ClickedSignal.RemoveListener(RateAppButton);
			base.view.googleButton.ClickedSignal.RemoveListener(GoogleButton);
			base.view.closeButton.ClickedSignal.RemoveListener(CloseButton);
			base.view.achievementButton.ClickedSignal.RemoveListener(AchievementButton);
			closeSignal.RemoveListener(Close);
			loginSuccess.RemoveListener(LoginSuccess);
			loginFailure.RemoveListener(LoginFailure);
			base.view.settings.ClickedSignal.RemoveListener(ShowSettings);
			base.view.about.ClickedSignal.RemoveListener(ShowAbout);
			base.view.help.ClickedSignal.RemoveListener(ShowHelp);
			facebookStateSignal.RemoveListener(setFacebookStatus);
			base.view.playMovieButton.ClickedSignal.RemoveListener(PlayMovieButton);
			tempHidden.RemoveListener(TempHide);
		}

		private void init()
		{
			base.view.RateUsText.text = localService.GetString("RateUsMenu");
			base.view.playMovieText.text = localService.GetString("PlayMovie");
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (base.view != null)
			{
				Start();
			}
		}

		private void Start()
		{
			togglePopupSignal.Dispatch(true);
			logger.Info("facebook killswitch : {0}", facebookService.isKillSwitchEnabled);
			logger.Info("google+ killswitch : {0}", googleService.isKillSwitchEnabled);
			base.view.facebookButton.gameObject.SetActive(!coppaService.Restricted() && !facebookService.isKillSwitchEnabled);
			SetupButtons();
			showStoreSignal.Dispatch(false);
			global::Kampai.UI.View.SettingsMenuPanel? settingsMenuPanel = currentPanel;
			if (!settingsMenuPanel.HasValue || !isTempHidden)
			{
				ShowSettings();
			}
			isTempHidden = false;
			if (playerService.GetHighestFtueCompleted() < 9)
			{
				questService.PauseQuestScripts();
			}
			global::Kampai.Util.ScreenUtils.ToggleAutoRotation(true);
		}

		private void SetupButtons()
		{
			UpdateLoginButtonText();
			base.view.googleButton.gameObject.SetActive(!coppaService.Restricted() && !googleService.isKillSwitchEnabled);
			base.view.achievementButton.gameObject.SetActive(!coppaService.Restricted() && !googleService.isKillSwitchEnabled);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			showStoreSignal.Dispatch(true);
		}

		private void TempHide()
		{
			isTempHidden = true;
		}

		private void CloseButton()
		{
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			Close();
		}

		protected override void Close()
		{
			if (base.view.gameObject.activeInHierarchy)
			{
				base.view.gameObject.SetActive(false);
				saveSignal.Dispatch();
				togglePopupSignal.Dispatch(false);
				if (playerService.GetHighestFtueCompleted() < 9 && !isTempHidden)
				{
					questService.ResumeQuestScripts();
				}
				model.ForceDisabled = false;
				displayDisco3DElements.Dispatch(true);
			}
			displayDebugButtonSignal.Dispatch(false);
		}

		private void AchievementButton()
		{
			global::Kampai.Game.ISocialService socialService = null;
			socialService = googleService;
			if (!ShouldLogin(socialService))
			{
				achievementService.ShowAchievements();
			}
		}

		private bool ShouldLogin(global::Kampai.Game.ISocialService socialService)
		{
			if (socialService != null && !socialService.isLoggedIn)
			{
				socialLoginSignal.Dispatch(socialService, new global::Kampai.Util.Boxed<global::System.Action>(AchievementButton));
				return true;
			}
			return false;
		}

		private void Close(global::UnityEngine.GameObject ignore)
		{
			Close();
		}

		private void LoginSuccess(global::Kampai.Game.ISocialService socialService)
		{
			switch (socialService.type)
			{
			case global::Kampai.Game.SocialServices.FACEBOOK:
				popupMessageSignal.Dispatch(localService.GetString("fbLoginSuccess"), global::Kampai.UI.View.PopupMessageType.NORMAL);
				break;
			case global::Kampai.Game.SocialServices.GOOGLEPLAY:
				popupMessageSignal.Dispatch(localService.GetString("googleplayloginsuccess"), global::Kampai.UI.View.PopupMessageType.NORMAL);
				break;
			}
			SetupButtons();
		}

		private void LoginFailure(global::Kampai.Game.ISocialService socialService)
		{
			switch (socialService.type)
			{
			case global::Kampai.Game.SocialServices.FACEBOOK:
				popupMessageSignal.Dispatch(localService.GetString("fbLoginFailure"), global::Kampai.UI.View.PopupMessageType.NORMAL);
				break;
			case global::Kampai.Game.SocialServices.GOOGLEPLAY:
				popupMessageSignal.Dispatch(localService.GetString("GooglePlayLoginFailure"), global::Kampai.UI.View.PopupMessageType.NORMAL);
				break;
			}
			SetupButtons();
		}

		private void UpdateLoginButtonText()
		{
			base.view.facebookButtonText.text = localService.GetString((!facebookService.isLoggedIn) ? "facebooklogin" : "facebooklogout");
			base.view.googleButtonText.text = localService.GetString((coppaService.Restricted() || !googleService.isLoggedIn) ? "googleplaylogin" : "googleplaylogout");
		}

		private void FacebookButton()
		{
			Close();
			SocialButton(facebookService, base.view.facebookButtonText, "facebooklogin");
		}

		private void GoogleButton()
		{
			SocialButton(googleService, base.view.googleButtonText, "googleplaylogin");
		}

		private void SocialButton(global::Kampai.Game.ISocialService service, global::UnityEngine.UI.Text buttonTextView, string loggedInKey)
		{
			if (service.isLoggedIn)
			{
				socialLogoutSignal.Dispatch(service);
				buttonTextView.text = localService.GetString(loggedInKey);
			}
			else if (service.type == global::Kampai.Game.SocialServices.FACEBOOK)
			{
				facebookService.LoginSource = "Settings";
				showFacebookPopupSignal.Dispatch(delegate
				{
				});
			}
			else
			{
				socialLoginSignal.Dispatch(service, new global::Kampai.Util.Boxed<global::System.Action>(null));
			}
		}

		private void PlayMovieButton()
		{
			videoService.playIntro(false, true);
		}

		private void RateAppButton()
		{
			Close();
			openRateAppPageSignal.Dispatch();
		}

		private void setFacebookStatus(bool loggedOn)
		{
			base.view.facebookButtonText.text = localService.GetString((!loggedOn) ? "facebooklogin" : "facebooklogout");
		}

		private void ShowSettings()
		{
			ShowPanel(global::Kampai.UI.View.SettingsMenuPanel.SETTINGS);
		}

		private void ShowAbout()
		{
			ShowPanel(global::Kampai.UI.View.SettingsMenuPanel.ABOUT);
		}

		public void ShowHelp()
		{
			ShowPanel(global::Kampai.UI.View.SettingsMenuPanel.HELP);
		}

		private void ShowPanel(global::Kampai.UI.View.SettingsMenuPanel panel)
		{
			if (currentPanel != panel)
			{
				bool flag = panel == global::Kampai.UI.View.SettingsMenuPanel.SETTINGS;
				base.view.settingsPanel.SetActive(flag);
				base.view.settingClicked.SetActive(flag);
				bool active = panel == global::Kampai.UI.View.SettingsMenuPanel.ABOUT;
				base.view.aboutPanel.SetActive(active);
				base.view.aboutClicked.SetActive(active);
				bool active2 = panel == global::Kampai.UI.View.SettingsMenuPanel.HELP;
				base.view.helpPanel.SetActive(active2);
				base.view.helpClicked.SetActive(active2);
				displayDebugButtonSignal.Dispatch(flag);
				currentPanel = panel;
			}
		}
	}
}
