namespace Kampai.UI.View
{
	public class PopupConfirmationView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.UI.Text title;

		public global::UnityEngine.UI.Text description;

		public global::Kampai.UI.View.ButtonView Accept;

		public global::Kampai.UI.View.ButtonView Decline;

		public global::Kampai.UI.View.LocalizeView LeftButton;

		public global::Kampai.UI.View.LocalizeView RightButton;

		public override void Init()
		{
			base.Init();
			base.Open();
		}
	}
}
