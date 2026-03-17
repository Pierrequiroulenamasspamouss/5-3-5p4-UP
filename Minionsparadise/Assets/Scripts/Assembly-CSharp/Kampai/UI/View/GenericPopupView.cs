namespace Kampai.UI.View
{
	public class GenericPopupView : global::Kampai.UI.View.PopupMenuView, IGenericPopupView
	{
		public global::UnityEngine.UI.Text itemName;

		public global::UnityEngine.UI.Text itemDuration;

		public global::UnityEngine.UI.Text itemOrigin;

		public global::Kampai.UI.View.KampaiImage itemDurationIcon;

		public ScrollableButtonView gotoButton;

		public float offsetValue = 1.8f;

		private global::Kampai.Main.ILocalizationService localizationService;

		public void Init(global::Kampai.Main.ILocalizationService localizationService)
		{
			base.Init();
			this.localizationService = localizationService;
		}

		public void Display(global::UnityEngine.Vector3 itemCenter)
		{
			base.Init();
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			global::UnityEngine.Vector3 anchoredPosition3D = rectTransform.sizeDelta / offsetValue;
			anchoredPosition3D.x = 0f;
			rectTransform.anchoredPosition3D = anchoredPosition3D;
			rectTransform.anchorMin = itemCenter;
			rectTransform.anchorMax = itemCenter;
			base.Open();
		}

		internal void SetName(string localizedName)
		{
			if (itemName != null)
			{
				itemName.text = localizedName;
			}
		}

		internal void SetTime(int duration)
		{
			if (itemDuration != null)
			{
				itemDuration.text = UIUtils.FormatTime(duration, localizationService);
			}
		}

		internal void SetItemOrigin(string localizedOrigin)
		{
			if (itemOrigin != null)
			{
				itemOrigin.text = localizedOrigin;
			}
		}

		internal void ShowGotoButton()
		{
			gotoButton.gameObject.SetActive(true);
		}

		internal void DisableDurationInfo()
		{
			itemDuration.enabled = false;
			itemDurationIcon.enabled = false;
		}
	}
}
