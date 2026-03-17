namespace Kampai.UI.View
{
	public class WatchRewardedAdModalView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.UI.Text Headline1;

		public global::Kampai.UI.View.KampaiImage RewardItemImage;

		public global::UnityEngine.UI.Text RewardAmountText;

		public global::UnityEngine.UI.Button YesButton;

		public global::UnityEngine.UI.Button NoButton;

		public global::UnityEngine.UI.Text YesButtonText;

		public global::UnityEngine.UI.Text NoButtonText;

		private global::Kampai.Main.ILocalizationService localService;

		internal void Init(global::Kampai.Main.ILocalizationService localService)
		{
			base.Init();
			this.localService = localService;
			Localize();
			base.Open();
		}

		private void Localize()
		{
			Headline1.text = localService.GetString("RewardedAdWatchVideoHeadline1");
			YesButtonText.text = localService.GetString("RewardedAdWatch");
			NoButtonText.text = localService.GetString("RewardedAdNoThanks");
		}
	}
}
