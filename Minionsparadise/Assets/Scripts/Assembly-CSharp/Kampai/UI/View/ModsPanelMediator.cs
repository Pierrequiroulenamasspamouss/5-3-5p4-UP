using System.Collections.Generic;
using Kampai.Game;
using Kampai.Main;
using Kampai.Util;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Kampai.UI.View
{
	public class ModsPanelMediator : Mediator
	{
		[Inject]
		public ModsPanelView view { get; set; }

		[Inject]
		public ILocalizationService localService { get; set; }

		[Inject]
		public IDevicePrefsService prefs { get; set; }

		[Inject]
		public SaveDevicePrefsSignal saveDevicePrefsSignal { get; set; }

		[Inject]
		public LanguageChangedSignal languageChangedSignal { get; set; }

		[Inject]
		public IConfigurationsService configurationsService { get; set; }

		[Inject]
		public PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		private static readonly string[] ALL_LANGUAGES = new string[] { "en", "fr", "de", "es", "it", "pt", "nl", "ko", "ru", "ja", "zh-cn", "zh-tw", "tr", "id", "lolcat", "minion" };

		private List<string> m_availableLanguages;

		public override void OnRegister()
		{
			Debug.Log("[ModsMediator] Registering Mods Panel Logic...");

			m_availableLanguages = new List<string>(ALL_LANGUAGES);
			if (configurationsService.GetConfigurations() == null || !configurationsService.GetConfigurations().AprilsFool)
			{
				m_availableLanguages.Remove("lolcat");
				m_availableLanguages.Remove("minion");
			}

			BindButton(view.languageButton);
			BindButton(view.nightToggleButton);

			if (view.languageButton != null)
			{
				view.languageButton.ClickedSignal.AddListener(OnLanguageButtonClicked);
			}

			if (view.nightToggleButton != null)
			{
				view.nightToggleButton.ClickedSignal.AddListener(OnNightToggleClicked);
			}

			UpdateLanguageText();
			UpdateNightToggleText();
		}

		private void BindButton(ButtonView bv)
		{
			if (bv == null) return;
			Button btn = bv.GetComponent<Button>();
			if (btn != null)
			{
				Debug.Log(string.Format("[ModsMediator] Programmatically binding onClick for {0}", bv.name));
				btn.onClick.RemoveAllListeners();
				btn.onClick.AddListener(bv.OnClickEvent);
			}
			else
			{
				Debug.LogWarning(string.Format("[ModsMediator] {0} is missing a native Button component!", bv.name));
			}
		}

		public override void OnRemove()
		{
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
			Debug.Log("[ModsMediator] Language Button Clicked!");
			string language = prefs.GetDevicePrefs().Language;
			if (string.IsNullOrEmpty(language))
			{
				language = Native.GetDeviceLanguage();
			}
			language = language.ToLower();
			int num = m_availableLanguages.IndexOf(language);
			if (num == -1)
			{
				num = 0;
			}
			num = (num + 1) % m_availableLanguages.Count;
			string nextLang = m_availableLanguages[num];
			
			Debug.Log(string.Format("[ModsMediator] Switching language from {0} to {1}", language, nextLang));
			
			prefs.GetDevicePrefs().Language = nextLang;
			saveDevicePrefsSignal.Dispatch();
			localService.Initialize(nextLang);
			localService.Update();
			UpdateLanguageText();
			languageChangedSignal.Dispatch();
			soundFXSignal.Dispatch("Play_minion_confirm_select_01");
		}

		private void OnNightToggleClicked()
		{
			Debug.Log("[ModsMediator] Night Toggle Clicked!");
			DayNightCycleManager manager = Object.FindObjectOfType<DayNightCycleManager>();
			if (manager != null)
			{
				manager.CycleNightMode();
				UpdateNightToggleText();
				Debug.Log(string.Format("[ModsMediator] Night mode now: {0}", manager.GetCurrentMode()));
				soundFXSignal.Dispatch("Play_minion_confirm_select_02");
			}
			else
			{
				Debug.LogError("[ModsMediator] Could not find DayNightCycleManager in scene!");
			}
		}

		private void UpdateNightToggleText()
		{
			if (view.nightToggleText != null)
			{
				DayNightCycleManager manager = Object.FindObjectOfType<DayNightCycleManager>();
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
