namespace Kampai.UI.View
{
	public class KampaiProgressBar : global::strange.extensions.mediation.impl.View
	{
		public global::UnityEngine.RectTransform FillImage;

		public void SetProgress(float ratio)
		{
			FillImage.anchorMax = new global::UnityEngine.Vector2(ratio, FillImage.anchorMax.y);
		}
	}
}
