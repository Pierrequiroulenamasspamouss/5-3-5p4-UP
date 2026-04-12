namespace Kampai.UI.View
{
	public class SkipPartyButtonView : global::strange.extensions.mediation.impl.View
	{
		public global::Kampai.UI.View.ButtonView SkipButton;

		public global::UnityEngine.GameObject SkipButtonCooldownMeter;

		public global::UnityEngine.RectTransform SkipButtonCooldownFillImage;

		private global::UnityEngine.Vector2 skipButtonCooldownFillAmount = new global::UnityEngine.Vector2(1f, 1f);

		internal void ShowSkipPartyButtonView(bool display)
		{
			SkipButton.gameObject.SetActive(display);
		}

		internal void UpdateSkipMeterTime(float timeRemaining, float totalTime)
		{
			float x = timeRemaining / totalTime;
			skipButtonCooldownFillAmount.x = x;
			SkipButtonCooldownFillImage.anchorMax = skipButtonCooldownFillAmount;
		}
	}
}
