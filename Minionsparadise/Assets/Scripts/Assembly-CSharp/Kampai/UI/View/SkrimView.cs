namespace Kampai.UI.View
{
	public class SkrimView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.ButtonView ClickButton;

		public global::UnityEngine.GameObject DarkSkrim;

		public global::UnityEngine.UI.Image DarkSkrimImage;

		public bool singleSkrimClose { get; set; }

		public bool genericPopupSkrim { get; set; }

		internal void Init(float alpha, bool fadeIn)
		{
			(base.transform as global::UnityEngine.RectTransform).offsetMax = global::UnityEngine.Vector2.zero;
			(base.transform as global::UnityEngine.RectTransform).offsetMin = global::UnityEngine.Vector2.zero;
			ClickButton.PlaySoundOnClick = false;
			global::UnityEngine.Color color = DarkSkrimImage.color;
			color.a = alpha;
			DarkSkrimImage.color = color;
			if (fadeIn)
			{
				DarkSkrimImage.CrossFadeAlpha(0f, 0f, true);
			}
		}

		public void EnableSkrimButton(bool enable)
		{
			ClickButton.GetComponent<global::UnityEngine.UI.Button>().interactable = enable;
		}

		internal void SetDarkSkrimActive(bool enabled, float duration)
		{
			DarkSkrim.SetActive(enabled);
			if (duration != 0f)
			{
				FadeDarkSkrim(1f, duration);
			}
		}

		internal void FadeDarkSkrim(float alpha, float duration)
		{
			DarkSkrimImage.CrossFadeAlpha(alpha, duration, false);
		}
	}
}
