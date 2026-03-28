namespace Kampai.UI.View
{
	public class SettingsPanelView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.ButtonView DLCButton;

		public global::Kampai.UI.View.ButtonView notificationsButton;

		public global::Kampai.UI.View.ButtonView notificationsOffButton;

		public global::Kampai.UI.View.ButtonView doubleConfirmButton;

		public global::UnityEngine.GameObject notificationsPanel;

		public global::UnityEngine.UI.Text musicValue;

		public global::UnityEngine.UI.Text soundValue;

		public global::UnityEngine.UI.Text server;

		public global::UnityEngine.UI.Text buildNumber;

		public global::UnityEngine.UI.Text DLCText;

		public global::UnityEngine.UI.Text doubleConfirmText;

		public global::UnityEngine.UI.Text notificationsText;

		public global::UnityEngine.UI.Slider MusicSlider;

		public global::UnityEngine.UI.Slider SFXSlider;

		public global::Kampai.UI.View.ButtonView languageButton;

		public global::UnityEngine.UI.Text languageText;

		public global::Kampai.UI.View.ButtonView nightToggleButton;

		public global::UnityEngine.UI.Text nightToggleText;

		public global::strange.extensions.signal.impl.Signal<bool> volumeSliderChangedSignal = new global::strange.extensions.signal.impl.Signal<bool>();

		public void OnMusicSliderChanged()
		{
			volumeSliderChangedSignal.Dispatch(true);
		}

		public void OnSFXSliderChanged()
		{
			volumeSliderChangedSignal.Dispatch(false);
		}

		internal void ToggleNotificationsOn(bool enable)
		{
			notificationsButton.GetComponent<global::UnityEngine.UI.Button>().interactable = enable;
			notificationsOffButton.gameObject.SetActive(!enable);
		}

		public void LayoutButtons()
		{
			if (DLCButton == null || languageButton == null || notificationsButton == null || nightToggleButton == null) return;

			// Disable Layout Group on parent
			global::UnityEngine.UI.LayoutGroup layoutGroup = DLCButton.transform.parent.GetComponent<global::UnityEngine.UI.LayoutGroup>();
			if (layoutGroup != null) layoutGroup.enabled = false;

			float btnWidth = 350f; 
			float btnHeight = 85f;
			float spacingX = 20f;
			float spacingY = 15f;
			
			// Position: FAR Left and Bottom
			// Moving startPos from -380 to -540 to push it to the very edge.
			global::UnityEngine.Vector2 startPos = new global::UnityEngine.Vector2(-640f, -145f); 
			
			SetupButton(DLCButton, new global::UnityEngine.Vector2(startPos.x, startPos.y + btnHeight + spacingY), btnWidth, btnHeight);
			SetupButton(languageButton, new global::UnityEngine.Vector2(startPos.x + btnWidth + spacingX, startPos.y + btnHeight + spacingY), btnWidth, btnHeight);
			SetupButton(notificationsButton, new global::UnityEngine.Vector2(startPos.x, startPos.y), btnWidth, btnHeight);
			SetupButton(notificationsOffButton, new global::UnityEngine.Vector2(startPos.x, startPos.y), btnWidth, btnHeight);
			SetupButton(nightToggleButton, new global::UnityEngine.Vector2(startPos.x + btnWidth + spacingX, startPos.y), btnWidth, btnHeight);
		}

		private void SetupButton(global::Kampai.UI.View.ButtonView btn, global::UnityEngine.Vector2 pos, float w, float h)
		{
			if (btn == null) return;
			global::UnityEngine.RectTransform rect = btn.GetComponent<global::UnityEngine.RectTransform>();
			if (rect != null)
			{
				rect.pivot = new global::UnityEngine.Vector2(0f, 0.5f); // Left pivot
				rect.anchorMin = new global::UnityEngine.Vector2(0.5f, 0.5f);
				rect.anchorMax = new global::UnityEngine.Vector2(0.5f, 0.5f);
				rect.sizeDelta = new global::UnityEngine.Vector2(w, h);
				rect.anchoredPosition = pos;
			}
			
			// Icon Scaling
			global::UnityEngine.UI.Image[] images = btn.GetComponentsInChildren<global::UnityEngine.UI.Image>(true);
			foreach (global::UnityEngine.UI.Image img in images)
			{
				if (img.gameObject != btn.gameObject)
				{
					global::UnityEngine.RectTransform imgRect = img.GetComponent<global::UnityEngine.RectTransform>();
					if (imgRect != null)
					{
						imgRect.localScale = new global::UnityEngine.Vector3(0.7f, 0.7f, 1f);
						imgRect.anchoredPosition = global::UnityEngine.Vector2.zero; 
					}
				}
			}
		}
	}
}
