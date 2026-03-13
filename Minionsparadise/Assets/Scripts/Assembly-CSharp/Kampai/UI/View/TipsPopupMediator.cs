namespace Kampai.UI.View
{
	public class TipsPopupMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.TipsPopupView>
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.closeButton.ClickedSignal.AddListener(Close);
			base.view.gameObject.SetActive(false);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.closeButton.ClickedSignal.RemoveListener(Close);
		}

		protected override void Close()
		{
			hideSkrim.Dispatch("DidYouKnowSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_Tip");
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			string arg = args.Get<string>();
			string text = string.Format("<b>{0}</b> <color=black>{1}</color>", localizationService.GetString("DidYouKnow"), arg);
			base.view.Display(text);
			soundFXSignal.Dispatch("Play_menu_popUp_02");
		}
	}
}
