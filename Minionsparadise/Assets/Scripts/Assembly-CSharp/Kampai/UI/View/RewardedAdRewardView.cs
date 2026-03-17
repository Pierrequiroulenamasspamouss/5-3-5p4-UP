namespace Kampai.UI.View
{
	public class RewardedAdRewardView : global::UnityEngine.MonoBehaviour
	{
		public global::Kampai.UI.View.KampaiImage RewardItemImage;

		public global::UnityEngine.UI.Text RewardAmountText;

		public global::UnityEngine.ParticleSystem FlashParticleSystem;

		private void Start()
		{
			global::UnityEngine.Transform transform = base.gameObject.transform;
			transform.localScale /= 3f;
			GoTween tween = new GoTween(transform, 1f, new GoTweenConfig().setEaseType(GoEaseType.CubicInOut).scale(1f));
			GoTweenFlow goTweenFlow = new GoTweenFlow();
			goTweenFlow.insert(0f, tween);
			goTweenFlow.play();
		}

		internal void Init(global::Kampai.Game.ItemDefinition rewardItem, int rewardAmount, bool playVFX = true)
		{
			RewardItemImage.sprite = UIUtils.LoadSpriteFromPath(rewardItem.Image);
			RewardItemImage.maskSprite = UIUtils.LoadSpriteFromPath(rewardItem.Mask);
			RewardAmountText.text = UIUtils.FormatLargeNumber(rewardAmount);
			if (!playVFX)
			{
				FlashParticleSystem.gameObject.SetActive(false);
			}
		}
	}
}
