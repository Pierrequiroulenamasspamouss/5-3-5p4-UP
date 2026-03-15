namespace Kampai.UI.View
{
	public class MinionPartyCountDownInfoPopupView : global::Kampai.UI.View.PopupMenuView, IGenericPopupView
	{
		public global::UnityEngine.UI.Text CountDownText;

		internal void UpdateCountDownText(string text)
		{
			CountDownText.text = text;
		}

		public void Init(global::Kampai.Main.ILocalizationService localizationService)
		{
			base.Init();
		}

		public void Display(global::UnityEngine.Vector3 itemCenter)
		{
			base.Init();
			base.transform.localPosition = global::UnityEngine.Vector3.zero;
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			rectTransform.anchorMin = itemCenter;
			rectTransform.anchorMax = itemCenter;
			base.Open();
		}
	}
}
