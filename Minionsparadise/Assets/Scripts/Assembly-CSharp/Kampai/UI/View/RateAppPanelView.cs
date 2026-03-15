namespace Kampai.UI.View
{
	public class RateAppPanelView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.ButtonView closeButton;

		public global::Kampai.UI.View.ButtonView rateButton;

		public global::Kampai.UI.View.ButtonView notNowButton;

		public global::Kampai.UI.View.ButtonView neverButton;

		protected override void Start()
		{
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			rectTransform.anchoredPosition = global::UnityEngine.Vector2.zero;
			base.Start();
		}
	}
}
