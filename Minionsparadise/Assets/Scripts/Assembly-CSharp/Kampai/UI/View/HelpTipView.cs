namespace Kampai.UI.View
{
	public class HelpTipView : global::Kampai.UI.View.PopupMenuView, IGenericPopupView
	{
		public global::UnityEngine.UI.Text Title;

		public global::UnityEngine.UI.Text Message;

		public global::UnityEngine.GameObject Pointer;

		public global::UnityEngine.GameObject GlassCanvas;

		private global::Kampai.Main.ILocalizationService localizationService;

		public void Init(global::Kampai.Main.ILocalizationService localizationService)
		{
			base.Init();
			this.localizationService = localizationService;
		}

		public void SetTip(global::Kampai.Game.HelpTipDefinition tip)
		{
			Title.text = localizationService.GetString(tip.Title);
			Message.text = localizationService.GetString(tip.Message);
		}

		public void SetUICanvas(global::UnityEngine.GameObject glassCanvas)
		{
			GlassCanvas = glassCanvas;
		}

		public void Display(global::UnityEngine.Vector3 itemCenter)
		{
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			rectTransform.anchoredPosition3D = global::UnityEngine.Vector3.zero;
			float num = 0.01f;
			float num2 = 0f;
			float num3 = (float)global::UnityEngine.Screen.width * num;
			float num4 = itemCenter.x * (float)global::UnityEngine.Screen.width;
			float num5 = 1f;
			if (GlassCanvas != null)
			{
				global::UnityEngine.RectTransform rectTransform2 = GlassCanvas.transform as global::UnityEngine.RectTransform;
				if (rectTransform2 != null)
				{
					num5 = (float)global::UnityEngine.Screen.width / rectTransform2.sizeDelta.x;
				}
			}
			float num6 = rectTransform.offsetMax.x * num5 + num3;
			float num7 = num4 + num6;
			if (num7 > (float)global::UnityEngine.Screen.width)
			{
				num2 = num7 - (float)global::UnityEngine.Screen.width;
			}
			else
			{
				float num8 = rectTransform.offsetMin.x - num3;
				float num9 = num4 + num8;
				if (num9 < 0f)
				{
					num2 = num9;
				}
			}
			itemCenter.x -= num2 / (float)global::UnityEngine.Screen.width;
			global::UnityEngine.Vector3 localPosition = Pointer.gameObject.transform.localPosition;
			localPosition.x += num2 / num5;
			Pointer.gameObject.transform.localPosition = localPosition;
			rectTransform.anchorMin = itemCenter;
			rectTransform.anchorMax = itemCenter;
			base.Open();
		}
	}
}
