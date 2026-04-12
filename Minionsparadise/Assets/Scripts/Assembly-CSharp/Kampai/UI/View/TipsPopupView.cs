namespace Kampai.UI.View
{
	public class TipsPopupView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text TipsText;

		public global::Kampai.UI.View.ButtonView closeButton;

		internal void Display(string text)
		{
			TipsText.text = text;
			base.gameObject.SetActive(true);
		}
	}
}
