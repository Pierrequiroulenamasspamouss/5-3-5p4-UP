namespace Kampai.UI.View
{
	public class CreditsPanelView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.ScrollRect scrollRect;

		public global::UnityEngine.UI.Image overlayImage;

		public global::Kampai.UI.View.ButtonView closeButton;

		public global::UnityEngine.UI.Text creditText;

		internal global::System.Collections.Generic.List<global::UnityEngine.UI.Text> textList = new global::System.Collections.Generic.List<global::UnityEngine.UI.Text>();

		internal float scrolledOffsetY;

		private bool initialized;

		internal void SetupDivisions(float firstTextHeight)
		{
			float num = firstTextHeight;
			foreach (global::UnityEngine.UI.Text text in textList)
			{
				float num2 = elementHeight(text.gameObject);
				text.rectTransform.anchoredPosition = new global::UnityEngine.Vector2(0f, 0f - num);
				num += num2;
				text.gameObject.SetActive(false);
			}
			scrollRect.content.sizeDelta = new global::UnityEngine.Vector2(scrollRect.content.sizeDelta.x, num);
			scrolledOffsetY = 0.01f;
		}

		public void Update()
		{
			float y = scrollRect.content.anchoredPosition.y;
			if (y == scrolledOffsetY && initialized)
			{
				return;
			}
			initialized = true;
			scrolledOffsetY = y;
			float num = elementHeight(base.gameObject);
			foreach (global::UnityEngine.UI.Text text in textList)
			{
				global::UnityEngine.RectTransform rectTransform = text.gameObject.transform as global::UnityEngine.RectTransform;
				float num2 = rectTransform.offsetMax.y + scrolledOffsetY;
				float num3 = rectTransform.offsetMin.y + scrolledOffsetY;
				text.gameObject.SetActive(num3 < 0f && num2 > 0f - num);
			}
		}

		internal void Cleanup()
		{
			textList.Clear();
		}

		private float elementHeight(global::UnityEngine.GameObject go)
		{
			return (go.transform as global::UnityEngine.RectTransform).rect.height;
		}
	}
}
