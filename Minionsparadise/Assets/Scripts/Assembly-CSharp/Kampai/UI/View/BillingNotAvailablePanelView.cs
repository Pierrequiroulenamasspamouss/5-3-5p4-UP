namespace Kampai.UI.View
{
	public class BillingNotAvailablePanelView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.ButtonView okButton;

		protected override void Start()
		{
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			rectTransform.anchoredPosition = global::UnityEngine.Vector2.zero;
			base.Start();
		}
	}
}
