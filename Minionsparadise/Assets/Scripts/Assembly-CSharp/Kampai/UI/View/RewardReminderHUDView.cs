namespace Kampai.UI.View
{
	public class RewardReminderHUDView : global::strange.extensions.mediation.impl.View
	{
		public global::Kampai.UI.View.KampaiImage rewardImage;

		public global::Kampai.UI.View.ButtonView imageButton;

		internal global::Kampai.Game.PendingRewardDefinition pendingRewardDef;

		public void Init(global::Kampai.UI.IPositionService positionService)
		{
			positionService.AddHUDElementToAvoid(base.gameObject);
		}

		public void SetImage(string imageName, string maskName)
		{
			rewardImage.sprite = UIUtils.LoadSpriteFromPath(imageName);
			rewardImage.maskSprite = UIUtils.LoadSpriteFromPath(maskName);
		}

		public void SetArguments(global::Kampai.Game.PendingRewardDefinition prd)
		{
			SetImage(prd.hudReminderImage, prd.hudReminderMask);
			pendingRewardDef = prd;
		}

		public void ShowReminder(bool show)
		{
			imageButton.gameObject.SetActive(show);
		}
	}
}
