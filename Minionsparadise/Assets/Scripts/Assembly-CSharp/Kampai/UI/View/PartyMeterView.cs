namespace Kampai.UI.View
{
	public class PartyMeterView : global::strange.extensions.mediation.impl.View
	{
		public global::UnityEngine.GameObject PartyMeterPanel;

		public global::UnityEngine.UI.Text CountDownTimerText;

		public global::UnityEngine.UI.Text BuffText;

		public global::Kampai.UI.View.KampaiImage BuffIcon;

		internal void DisplayCooldownMeter(bool display)
		{
			PartyMeterPanel.SetActive(display);
		}

		internal void UpdateCountDownText(string text)
		{
			CountDownTimerText.text = text;
		}

		internal void UpdateBuffText(string text)
		{
			BuffText.text = text;
		}

		internal void UpdateBuffIcon(string path)
		{
			BuffIcon.maskSprite = UIUtils.LoadSpriteFromPath(path);
		}
	}
}
