namespace Kampai.UI.View
{
	public class FAQPanelMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.FAQPanelView>
	{
		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		public override void OnRegister()
		{
			base.view.closeButton.ClickedSignal.AddListener(CloseButton);
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.view.closeButton.ClickedSignal.RemoveListener(CloseButton);
			base.OnRemove();
		}

		private void CloseButton()
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			Close();
		}

		protected override void Close()
		{
			base.gameObject.SetActive(false);
		}
	}
}
