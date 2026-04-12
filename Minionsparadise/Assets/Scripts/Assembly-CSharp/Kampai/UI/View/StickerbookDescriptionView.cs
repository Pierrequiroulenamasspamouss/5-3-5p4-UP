namespace Kampai.UI.View
{
	public class StickerbookDescriptionView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.UI.Text title;

		public global::UnityEngine.UI.Text description;

		public global::UnityEngine.UI.Text date;

		public global::UnityEngine.RectTransform downArrow;

		private float magicNumber = 0.02f;

		[Inject(global::Kampai.Main.MainElement.UI_GLASSCANVAS)]
		public global::UnityEngine.GameObject glassCanvas { get; set; }

		internal void Display(global::UnityEngine.Vector3 stickerCenter)
		{
			base.Init();
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			global::UnityEngine.Vector3 anchoredPosition3D = rectTransform.sizeDelta / 3f;
			anchoredPosition3D.x = 0f;
			rectTransform.anchoredPosition3D = anchoredPosition3D;
			rectTransform.anchorMin = stickerCenter;
			rectTransform.anchorMax = stickerCenter;
			float x = (glassCanvas.transform as global::UnityEngine.RectTransform).sizeDelta.x;
			float num = rectTransform.anchorMin.x - rectTransform.sizeDelta.x / x / 2f;
			if (num < 0f)
			{
				rectTransform.anchorMin = new global::UnityEngine.Vector2(stickerCenter.x - num + magicNumber, stickerCenter.y);
				rectTransform.anchorMax = new global::UnityEngine.Vector2(stickerCenter.x - num + magicNumber, stickerCenter.y);
				downArrow.anchoredPosition = new global::UnityEngine.Vector2(downArrow.anchoredPosition.x + num * x, downArrow.anchoredPosition.y);
			}
			else
			{
				float num2 = rectTransform.anchorMin.x + rectTransform.sizeDelta.x / x / 2f;
				if (num2 > 1f)
				{
					rectTransform.anchorMin = new global::UnityEngine.Vector2(stickerCenter.x - (num2 - 1f) - magicNumber, stickerCenter.y);
					rectTransform.anchorMax = new global::UnityEngine.Vector2(stickerCenter.x - (num2 - 1f) - magicNumber, stickerCenter.y);
					downArrow.anchoredPosition = new global::UnityEngine.Vector2(downArrow.anchoredPosition.x + (num2 - 1f) * x, downArrow.anchoredPosition.y);
				}
			}
			base.Open();
		}

		internal void SetTitle(string localizedString)
		{
			title.text = localizedString;
		}

		internal void SetDescription(bool locked, global::Kampai.Game.Sticker sticker, string localizedString, global::Kampai.Main.ILocalizationService localizationService)
		{
			if (!locked)
			{
				date.gameObject.SetActive(true);
				date.text = UIUtils.FormatDate(sticker.UTCTimeEarned, localizationService);
			}
			description.text = localizedString;
		}
	}
}
