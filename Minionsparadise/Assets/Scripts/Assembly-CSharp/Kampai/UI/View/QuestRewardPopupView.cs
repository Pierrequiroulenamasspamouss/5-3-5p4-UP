namespace Kampai.UI.View
{
	public class QuestRewardPopupView : global::strange.extensions.mediation.impl.View
	{
		public global::Kampai.UI.View.ButtonView ConfirmButton;

		public global::UnityEngine.Animator animator;

		public global::UnityEngine.RectTransform rewardsList;

		private bool isOpened;

		internal void Init(global::System.Collections.Generic.List<global::Kampai.Game.DisplayableDefinition> itemDefs, global::Kampai.Main.ILocalizationService localService)
		{
			foreach (global::UnityEngine.Transform rewards in rewardsList)
			{
				global::UnityEngine.Object.Destroy(rewards.gameObject);
			}
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_QuestPopupSlider") as global::UnityEngine.GameObject;
			for (int i = 0; i < itemDefs.Count; i++)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
				global::Kampai.UI.View.QuestRewardSliderView component = gameObject.GetComponent<global::Kampai.UI.View.QuestRewardSliderView>();
				component.icon.sprite = UIUtils.LoadSpriteFromPath(itemDefs[i].Image);
				component.icon.maskSprite = UIUtils.LoadSpriteFromPath(itemDefs[i].Mask);
				component.description.text = localService.GetString(itemDefs[i].LocalizedKey);
				gameObject.transform.SetParent(rewardsList, false);
			}
		}

		internal void PlayAnim(bool open)
		{
			if (open)
			{
				animator.Play("anim_RewardsPopup_init");
				isOpened = true;
			}
			else if (isOpened)
			{
				animator.Play("anim_RewardsPopup_close");
				isOpened = false;
			}
		}
	}
}
