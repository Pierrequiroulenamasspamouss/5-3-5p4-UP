namespace Kampai.UI.View
{
	public class FacebookConnectPopupMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.FacebookConnectPopupView>
	{
		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginSignal socialLoginSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.Init();
			base.view.ConnectButton.ClickedSignal.AddListener(ConnectButtonPressed);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.ConnectButton.ClickedSignal.RemoveListener(ConnectButtonPressed);
		}

		public void ConnectButtonPressed()
		{
			if (!facebookService.isLoggedIn)
			{
				socialLoginSignal.Dispatch(facebookService, new global::Kampai.Util.Boxed<global::System.Action>(null));
			}
			Close();
		}

		protected override void Close()
		{
			hideSkrim.Dispatch("StageSkrimFB");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_SocialParty_FBConnect");
		}
	}
}
