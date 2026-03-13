namespace Kampai.Splash
{
	public class SavedTipsModel
	{
		public const char delimeter = '\u001d';

		public global::System.Collections.Generic.IList<global::Kampai.Splash.TipToShow> Tips = new global::System.Collections.Generic.List<global::Kampai.Splash.TipToShow>();

		public string TipsLocale = string.Empty;

		public SavedTipsModel()
		{
			InitTips();
		}

		private void InitTips()
		{
			if (global::UnityEngine.PlayerPrefs.HasKey("SavedLocTipsWithTimes"))
			{
				string text = global::UnityEngine.PlayerPrefs.GetString("SavedLocTipsWithTimes");
				string[] array = text.Split('\u001d');
				for (int i = 0; i < array.Length; i += 2)
				{
					float result = 0f;
					float.TryParse(array[i + 1], out result);
					global::Kampai.Splash.TipToShow item = new global::Kampai.Splash.TipToShow(array[i], result);
					Tips.Add(item);
				}
			}
			if (global::UnityEngine.PlayerPrefs.HasKey("LastSavedTipsLocale"))
			{
				TipsLocale = global::UnityEngine.PlayerPrefs.GetString("LastSavedTipsLocale");
			}
		}

		public void SaveTipsToDevice(global::System.Collections.Generic.IList<global::Kampai.Splash.TipToShow> tips, string locale)
		{
			string text = string.Empty;
			for (int i = 0; i < tips.Count; i++)
			{
				global::Kampai.Splash.TipToShow tipToShow = tips[i];
				if (i > 0)
				{
					text += '\u001d';
				}
				text += tipToShow.Text;
				text += '\u001d';
				text += tipToShow.Time;
			}
			global::UnityEngine.PlayerPrefs.SetString("SavedLocTipsWithTimes", text);
			global::UnityEngine.PlayerPrefs.SetString("LastSavedTipsLocale", locale);
		}
	}
}
