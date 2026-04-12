namespace Kampai.UI.View
{
	public class SocialPartyFillOrderButtonView : global::Kampai.UI.View.ButtonView
	{
		public global::UnityEngine.GameObject orderClosedPanel;

		public global::UnityEngine.GameObject orderOpenPanel;

		public FillOrderButtonView fillOrderButton;

		public global::Kampai.UI.View.KampaiImage orderOpenImageIngredient;

		public global::UnityEngine.UI.Text orderOpenTextAmount;

		public global::UnityEngine.UI.Image orderOpenImageCheck;

		public global::Kampai.UI.View.KampaiImage grindImage;

		public global::Kampai.UI.View.KampaiImage xpImage;

		public global::UnityEngine.UI.Text xpReward;

		public global::UnityEngine.UI.Text grindReward;

		public global::UnityEngine.UI.Image orderClosedImagePicture;

		public global::UnityEngine.UI.Image orderClosedImageCheck;

		public global::Kampai.UI.View.KampaiImage orderClosedNoLoginImageCheck;

		public global::UnityEngine.GameObject xpIcon;

		public global::UnityEngine.GameObject funIcon;

		internal global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> missingItems;

		public void CreatFillOrderPopupIndicator(string rewardImagePath, string rewardMaskPath)
		{
			orderOpenImageIngredient.sprite = UIUtils.LoadSpriteFromPath(rewardImagePath);
			orderOpenImageIngredient.maskSprite = UIUtils.LoadSpriteFromPath(rewardMaskPath);
		}

		public void SetButtonInteractable(bool isInteractable)
		{
			fillOrderButton.GetComponent<global::UnityEngine.UI.Button>().interactable = isInteractable;
		}
	}
}
