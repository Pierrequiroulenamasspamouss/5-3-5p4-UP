namespace Kampai.UI.View
{
	public class BillingNotAvailablePanelMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.BillingNotAvailablePanelView>
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.okButton.ClickedSignal.AddListener(Close);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.okButton.ClickedSignal.RemoveListener(Close);
		}

		protected override void Close()
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			hideSkrimSignal.Dispatch("BillingSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "BillingNotAvailablePanel");
		}
	}
}
