namespace Kampai.Splash
{
	public class TipToShow
	{
		public string Text { get; set; }

		public float Time { get; set; }

		public TipToShow()
		{
			Text = string.Empty;
			Time = 0f;
		}

		public TipToShow(string text, float time)
		{
			Text = text;
			Time = time;
		}
	}
}
