namespace Kampai.UI.View
{
	public class GenericRushDialogView : global::Kampai.UI.View.RushDialogView
	{
		public global::UnityEngine.UI.Text PurchaseDescriptionText;

		public global::UnityEngine.UI.Text HeadlineTitle;

		public global::Kampai.UI.View.ButtonView AdVideoButton;

		internal override void SetupDialog(global::Kampai.UI.View.RushDialogView.RushDialogType type, bool showPurchaseButton)
		{
			switch (type)
			{
			case global::Kampai.UI.View.RushDialogView.RushDialogType.DEFAULT:
				Title.text = localService.GetString("NotEnough");
				PurchaseDescriptionText.text = localService.GetString("NotEnoughResources");
				break;
			case global::Kampai.UI.View.RushDialogView.RushDialogType.SOCIAL:
				Title.text = localService.GetString("SocialPartyNotEnough");
				PurchaseDescriptionText.text = localService.GetString("SocialPartyNotEnoughResources");
				break;
			default:
				Title.text = localService.GetString("YouNeed");
				PurchaseDescriptionText.text = localService.GetString("YouNeedResources");
				break;
			}
			base.SetupDialog(type, showPurchaseButton);
		}

		public void EnableRewardedAdRushButton(bool adButtonEnabled)
		{
			AdVideoButton.gameObject.SetActive(adButtonEnabled);
		}
	}
}
