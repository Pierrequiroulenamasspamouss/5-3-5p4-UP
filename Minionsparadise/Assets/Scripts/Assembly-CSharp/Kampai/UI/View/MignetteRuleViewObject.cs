namespace Kampai.UI.View
{
	public class MignetteRuleViewObject : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.UI.Text AmountLabel;

		public global::Kampai.UI.View.KampaiImage CauseImage;

		public global::Kampai.UI.View.KampaiImage EffectImage;

		public void RenderRule(MignetteRuleDefinition ruleDefinition)
		{
			StartCoroutine(Wait_thenRenderImages(ruleDefinition));
			AmountLabel.text = ruleDefinition.EffectAmount.ToString();
		}

		private global::System.Collections.IEnumerator Wait_thenRenderImages(MignetteRuleDefinition ruleDefinition)
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			if (!string.IsNullOrEmpty(ruleDefinition.CauseImageMask))
			{
				CauseImage.maskSprite = UIUtils.LoadSpriteFromPath(ruleDefinition.CauseImageMask);
			}
			CauseImage.sprite = UIUtils.LoadSpriteFromPath(ruleDefinition.CauseImage);
			if (!string.IsNullOrEmpty(ruleDefinition.EffectImageMask))
			{
				EffectImage.maskSprite = UIUtils.LoadSpriteFromPath(ruleDefinition.EffectImageMask);
			}
			EffectImage.sprite = UIUtils.LoadSpriteFromPath(ruleDefinition.EffectImage);
		}
	}
}
