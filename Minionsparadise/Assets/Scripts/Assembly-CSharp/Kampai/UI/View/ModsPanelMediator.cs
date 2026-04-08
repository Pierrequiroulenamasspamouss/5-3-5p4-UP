namespace Kampai.UI.View
{
	public class ModsPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.ModsPanelView view { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.IDevicePrefsService prefs { get; set; }

		[Inject]
		public global::Kampai.Game.SaveDevicePrefsSignal saveDevicePrefsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.LanguageChangedSignal languageChangedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		private static readonly string[] ALL_LANGUAGES = new string[] { "en", "fr", "de", "es", "it", "pt", "nl", "ko", "ru", "ja", "zh-cn", "zh-tw", "tr", "id", "lolcat", "minion" };

		private global::System.Collections.Generic.List<string> m_availableLanguages;

		public override void OnRegister()
		{
			base.OnRegister();
			
			if (view.languageButton != null)
			{
				view.languageButton.ClickedSignal.AddListener(OnLanguageButtonClicked);
			}

			if (view.nightToggleButton != null)
			{
				view.nightToggleButton.ClickedSignal.AddListener(OnNightToggleClicked);
				UpdateNightToggleText();
			}

			m_availableLanguages = new global::System.Collections.Generic.List<string>(ALL_LANGUAGES);
			if (configurationsService.GetConfigurations() == null || !configurationsService.GetConfigurations().AprilsFool)
			{
				m_availableLanguages.Remove("lolcat");
				m_availableLanguages.Remove("minion");
			}

			UpdateLanguageText();
		}

		public override void OnRemove()
		{
			base.OnRemove();

			if (view.languageButton != null)
			{
				view.languageButton.ClickedSignal.RemoveListener(OnLanguageButtonClicked);
			}

			if (view.nightToggleButton != null)
			{
				view.nightToggleButton.ClickedSignal.RemoveListener(OnNightToggleClicked);
			}
		}

		private void UpdateLanguageText()
		{
			if (view.languageText != null)
			{
				view.languageText.text = localService.GetLanguage().ToUpper();
			}
		}

		private void OnLanguageButtonClicked()
		{
			string language = prefs.GetDevicePrefs().Language;
			if (string.IsNullOrEmpty(language))
			{
				language = global::Kampai.Util.Native.GetDeviceLanguage();
			}
			language = language.ToLower();
			int num = m_availableLanguages.IndexOf(language);
			if (num == -1)
			{
				num = 0;
			}
			num = (num + 1) % m_availableLanguages.Count;
			string nextLang = m_availableLanguages[num];
			prefs.GetDevicePrefs().Language = nextLang;
			saveDevicePrefsSignal.Dispatch();
			localService.Initialize(nextLang);
			localService.Update();
			UpdateLanguageText();
			languageChangedSignal.Dispatch();
		}

		private void OnNightToggleClicked()
		{
			global::Kampai.Game.DayNightCycleManager manager = global::UnityEngine.Object.FindObjectOfType<global::Kampai.Game.DayNightCycleManager>();
			if (manager != null)
			{
				manager.CycleNightMode();
				UpdateNightToggleText();
				soundFXSignal.Dispatch("Play_minion_confirm_select_02");
			}
		}

		private void UpdateNightToggleText()
		{
			if (view.nightToggleText != null)
			{
				global::Kampai.Game.DayNightCycleManager manager = global::UnityEngine.Object.FindObjectOfType<global::Kampai.Game.DayNightCycleManager>();
				if (manager != null)
				{
					view.nightToggleText.text = "MODE: " + manager.GetCurrentMode().ToString();
				}
				else
				{
					view.nightToggleText.text = "NIGHT: NO MGR";
				}
			}
		}
	}
}
