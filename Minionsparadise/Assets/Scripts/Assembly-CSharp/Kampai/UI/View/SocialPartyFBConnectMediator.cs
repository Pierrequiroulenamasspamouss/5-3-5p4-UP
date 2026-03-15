namespace Kampai.UI.View
{
	public class SocialPartyFBConnectMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.SocialPartyFBConnectView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialPartyFBConnectMediator") as global::Kampai.Util.IKampaiLogger;

		private global::System.Action<bool> returnAction;

		private bool loginSucceeded;

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFX { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginSuccessSignal loginSuccess { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginFailureSignal loginFailure { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginSignal socialLoginSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLogoutSignal socialLogoutSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.Init();
			base.view.connectButton.ClickedSignal.AddListener(ConnectButton);
			base.view.quitButton.ClickedSignal.AddListener(QuitButton);
			base.view.OnMenuClose.AddListener(CloseAnimationComplete);
			loginSuccess.AddListener(OnFBLoginSuccess);
			loginFailure.AddListener(OnFBLoginFailure);
			base.view.connectButtonText.text = localService.GetString("socialpartyfbconnectbutton");
			base.view.txtHeadline.text = localService.GetString("socialpartyfbconnecttitle");
			base.view.txtDescription.text = localService.GetString("socialpartyfbconnectdetails");
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.OnMenuClose.RemoveListener(CloseAnimationComplete);
			base.view.connectButton.ClickedSignal.RemoveListener(ConnectButton);
			base.view.quitButton.ClickedSignal.RemoveListener(QuitButton);
			loginSuccess.RemoveListener(OnFBLoginSuccess);
			loginFailure.RemoveListener(OnFBLoginFailure);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			returnAction = args.Get<global::System.Action<bool>>();
		}

		public void ConnectButton()
		{
			if (!facebookService.isLoggedIn)
			{
				socialLoginSignal.Dispatch(facebookService, new global::Kampai.Util.Boxed<global::System.Action>(null));
			}
			else
			{
				socialLogoutSignal.Dispatch(facebookService);
			}
		}

		public void QuitButton()
		{
			loginSucceeded = false;
			Close();
		}

		public void OnFBLoginSuccess(global::Kampai.Game.ISocialService socialService)
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "FB Login Success");
			loginSucceeded = true;
			Close();
		}

		public void OnFBLoginFailure(global::Kampai.Game.ISocialService socialService)
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "FB Login Failure");
		}

		protected override void Close()
		{
			globalSFX.Dispatch("Play_menu_disappear_01");
			base.view.Close();
		}

		public void CloseAnimationComplete()
		{
			hideSignal.Dispatch("StageSkrimFB");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_SocialParty_FBConnect");
			if (returnAction != null)
			{
				returnAction(loginSucceeded);
			}
		}
	}
}
