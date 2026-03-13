namespace Kampai.UI.View
{
	public class SocialPartyInviteAlertView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.ButtonView acceptButton;

		public global::Kampai.UI.View.ButtonView declineButton;

		public global::Kampai.UI.View.ButtonView nextButton;

		public global::Kampai.UI.View.ButtonView previousButton;

		public global::Kampai.UI.View.ButtonView closeButton;

		public global::UnityEngine.UI.Text acceptButtonText;

		public global::UnityEngine.UI.Text declineButtonText;

		public global::UnityEngine.UI.Text socialPartyDescription;

		public global::UnityEngine.UI.Text recipesFilledDescription;

		public global::UnityEngine.UI.Text title;

		public global::UnityEngine.UI.Text playerName;

		public global::UnityEngine.UI.Image playerImage;

		public override void Init()
		{
			base.Init();
			base.Open();
		}
	}
}
