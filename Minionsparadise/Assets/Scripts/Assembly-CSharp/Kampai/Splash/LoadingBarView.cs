namespace Kampai.Splash
{
	public class LoadingBarView : global::strange.extensions.mediation.impl.View
	{
		internal global::UnityEngine.GameObject meter_fill;

		internal global::UnityEngine.UI.Text txt_progress;

		public void Init()
		{
			meter_fill = base.gameObject.FindChild("meter_fill");
			txt_progress = base.gameObject.FindChild("txt_progressCounter").GetComponent<global::UnityEngine.UI.Text>();
		}

		public void SetText(string text)
		{
			txt_progress.text = text;
		}

		public void SetMeterFill(float fill)
		{
			meter_fill.GetComponent<global::UnityEngine.RectTransform>().anchorMax = new global::UnityEngine.Vector2(fill / 100f, 1f);
		}
	}
}
