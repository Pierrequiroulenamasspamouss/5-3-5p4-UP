namespace Kampai.UI.View
{
	public class SocialPartyEventCompletedMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.SocialPartyEventCompletedView>
	{
		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.Init();
			base.view.TitleText.text = localService.GetString("socialpartycompletedtitle");
			base.view.MessageText.text = localService.GetString("socialpartycompleteddescription");
			base.view.YesButtonText.text = localService.GetString("socialpartycompletedbutton");
			base.view.YesButton.ClickedSignal.AddListener(OkButtonPressed);
			base.view.OnMenuClose.AddListener(Close);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.YesButton.ClickedSignal.RemoveListener(OkButtonPressed);
			base.view.OnMenuClose.RemoveListener(Close);
		}

		public void OkButtonPressed()
		{
			base.view.Close();
		}

		protected override void Close()
		{
			hideSkrim.Dispatch("SocialCompleteSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_SocialParty_EventCompleted");
		}
	}
}
