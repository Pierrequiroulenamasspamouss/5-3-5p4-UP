namespace Kampai.Splash
{
	public class LogoPanelView : global::Kampai.Util.KampaiView
	{
		private global::UnityEngine.GameObject progressBar;

		private global::UnityEngine.GameObject wiFiPopup;

		public void SetupRefs()
		{
			progressBar = base.gameObject.FindChild("meter_bar");
		}

		public void ShowNoWiFi(bool show)
		{
			progressBar.SetActive(!show);
			wiFiPopup.SetActive(show);
		}

		public void SetNoWifiPanel(global::UnityEngine.GameObject wiFiPopup)
		{
			this.wiFiPopup = wiFiPopup;
		}
	}
}
