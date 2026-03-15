namespace Kampai.UI.View
{
	public class NotificationsView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.UI.Text MessageText;

		public global::UnityEngine.UI.Text buttonText;

		public global::Kampai.UI.View.ButtonView confirmButton;

		private global::Kampai.Main.ILocalizationService localService;

		internal void Init(global::Kampai.Main.ILocalizationService locService, string message)
		{
			base.Init();
			localService = locService;
			MessageText.text = message;
			buttonText.text = localService.GetString("socialpartycompletedbutton");
			base.Open();
		}
	}
}
