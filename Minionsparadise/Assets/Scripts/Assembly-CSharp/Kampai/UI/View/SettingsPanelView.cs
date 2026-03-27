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
	}
}
