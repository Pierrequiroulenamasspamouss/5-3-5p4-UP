namespace Kampai.UI.View
{
	public class SettingsPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private float lastSoundPlayed;

		private global::UnityEngine.UI.Toggle doubleConfirmToggle;

		[Inject]
		public global::Kampai.UI.View.SettingsPanelView view { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateVolumeSignal updateVolumeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDevicePrefsService prefs { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDLCDialogSignal displayDialogSignal { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject("game.server.environment")]
		public string ServerEnv { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Util.IClientVersion clientVersion { get; set; }

		[Inject]
		public global::Kampai.Game.SaveDevicePrefsSignal saveDevicePrefsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayNotificationReminderSignal displayNotificationSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.LanguageChangedSignal languageChangedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		private static readonly string[] ALL_LANGUAGES = new string[] { "en", "fr", "de", "es", "it", "pt", "nl", "ko", "ru", "ja", "zh-cn", "zh-tw", "tr", "id", "lolcat", "minion" };

		private global::System.Collections.Generic.List<string> m_availableLanguages;

		private int buildNumberClickCount;

		private float lastBuildNumberClickTime;

		private void OnEnable()
		{
			if (view != null)
			{
				Start();
			}
		}

		private void Start()
		{
			view.notificationsButton.ClickedSignal.AddListener(NotificationsButton);
			view.notificationsOffButton.ClickedSignal.AddListener(NotificationsOffButton);
			view.DLCButton.ClickedSignal.AddListener(DLCButton);
			view.doubleConfirmButton.ClickedSignal.AddListener(OnDoubleConfirm);
			if (view.languageButton != null)
			{
				view.languageButton.ClickedSignal.AddListener(OnLanguageButtonClicked);
			}
			m_availableLanguages = new global::System.Collections.Generic.List<string>(ALL_LANGUAGES);
			if (configurationsService.GetConfigurations() == null || !configurationsService.GetConfigurations().AprilsFool)
			{
				m_availableLanguages.Remove("lolcat");
				m_availableLanguages.Remove("minion");
			}
			Init();
			setServer(ServerEnv);
			setBuild(clientVersion.GetClientVersion());
			view.MusicSlider.value = ((!global::Kampai.Audio.AudioSettingsModel.MusicMuted) ? prefs.GetDevicePrefs().MusicVolume : 0f);
			view.SFXSlider.value = prefs.GetDevicePrefs().SFXVolume;
			view.musicValue.text = ((int)(100f * view.MusicSlider.value)).ToString();
			view.soundValue.text = ((int)(100f * view.SFXSlider.value)).ToString();
			view.volumeSliderChangedSignal.AddListener(OnVolumeChanged);
			view.buildNumber.raycastTarget = true;
			global::UnityEngine.UI.Button button = view.buildNumber.GetComponent<global::UnityEngine.UI.Button>();
			if (button == null)
			{
				button = view.buildNumber.gameObject.AddComponent<global::UnityEngine.UI.Button>();
			}
			button.transition = global::UnityEngine.UI.Selectable.Transition.None;
			button.onClick.AddListener(OnBuildNumberClicked);
		}

		private void OnDisable()
		{
			view.notificationsButton.ClickedSignal.RemoveListener(NotificationsButton);
			view.notificationsOffButton.ClickedSignal.RemoveListener(NotificationsOffButton);
			view.DLCButton.ClickedSignal.RemoveListener(DLCButton);
			view.volumeSliderChangedSignal.RemoveListener(OnVolumeChanged);
			view.doubleConfirmButton.ClickedSignal.RemoveListener(OnDoubleConfirm);
			if (view.languageButton != null)
			{
				view.languageButton.ClickedSignal.RemoveListener(OnLanguageButtonClicked);
			}
		}

		private void Init()
		{
			string displayQualityLevel = dlcService.GetDisplayQualityLevel();
			if (displayQualityLevel.Equals("DLCHDPack"))
			{
				view.DLCText.text = localService.GetString("DLCSDPack");
			}
			else
			{
				view.DLCText.text = localService.GetString("DLCHDPack");
			}
			view.notificationsText.text = localService.GetString("NotificationsLabel");
			doubleConfirmToggle = view.doubleConfirmButton.GetComponent<global::UnityEngine.UI.Toggle>();
			if (localPersistService.HasKeyPlayer("DoublePurchaseConfirm"))
			{
				doubleConfirmToggle.isOn = localPersistService.GetDataIntPlayer("DoublePurchaseConfirm") != 0;
			}
			else
			{
				doubleConfirmToggle.isOn = true;
				localPersistService.PutDataIntPlayer("DoublePurchaseConfirm", 1);
			}
			if (!global::Kampai.Util.Native.AreNotificationsEnabled() || coppaService.Restricted())
			{
				view.ToggleNotificationsOn(false);
			}
			else
			{
				view.ToggleNotificationsOn(true);
			}
			view.doubleConfirmText.text = localService.GetString("DoubleConfirm");
			UpdateLanguageText();
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

		private void OnVolumeChanged(bool isMusicSlider)
		{
			float value = view.MusicSlider.value;
			if (value != 0.9876f && prefs.GetDevicePrefs().MusicVolume != value)
			{
				if (global::Kampai.Audio.AudioSettingsModel.MusicMuted)
				{
					global::Kampai.Audio.AudioSettingsModel.MuteIfBackgoundMusic = false;
					global::Kampai.Audio.AudioSettingsModel.MusicMuted = false;
				}
				prefs.GetDevicePrefs().MusicVolume = value;
				view.musicValue.text = ((int)(100f * value)).ToString();
			}
			float value2 = view.SFXSlider.value;
			if (value2 != 0.9876f && prefs.GetDevicePrefs().SFXVolume != value2)
			{
				prefs.GetDevicePrefs().SFXVolume = value2;
				view.soundValue.text = ((int)(100f * value2)).ToString();
			}
			float num = (float)timeService.AppTime() - lastSoundPlayed;
			if (num >= 0.17f)
			{
				if (value2 > 0f)
				{
					if (isMusicSlider)
					{
						soundFXSignal.Dispatch("Play_minion_confirm_select_02");
					}
					else
					{
						soundFXSignal.Dispatch("Play_minion_confirm_select_01");
					}
					lastSoundPlayed = timeService.AppTime();
				}
				saveDevicePrefsSignal.Dispatch();
			}
			updateVolumeSignal.Dispatch();
		}

		private void NotificationsButton()
		{
			if (!localPersistService.HasKeyPlayer("InitialSettings"))
			{
				localPersistService.PutDataPlayer("InitialSettings", "true");
				prefs.GetDevicePrefs().ConstructionNotif = true;
				prefs.GetDevicePrefs().BlackMarketNotif = true;
				prefs.GetDevicePrefs().MinionsParadiseNotif = true;
				prefs.GetDevicePrefs().BaseResourceNotif = true;
				prefs.GetDevicePrefs().CraftingNotif = true;
				prefs.GetDevicePrefs().EventNotif = true;
				prefs.GetDevicePrefs().MarketPlaceNotif = true;
				prefs.GetDevicePrefs().SocialEventNotif = true;
			}
			view.notificationsPanel.SetActive(true);
		}

		private void NotificationsOffButton()
		{
			if (!coppaService.Restricted())
			{
				closeSignal.Dispatch(null);
				displayNotificationSignal.Dispatch(localService.GetString("NotificationEnableSettings"), true);
			}
		}

		private void DLCButton()
		{
			displayDialogSignal.Dispatch(localService.GetString("DLCConfirmationDialog"));
		}

		private void OnDoubleConfirm()
		{
			localPersistService.PutDataIntPlayer("DoublePurchaseConfirm", doubleConfirmToggle.isOn ? 1 : 0);
		}

		private void setServer(string serverString)
		{
			view.server.text = localService.GetString("server") + serverString;
		}

		private void setBuild(string buildID)
		{
			view.buildNumber.text = localService.GetString("buildNumber") + buildID;
		}

		private void OnBuildNumberClicked()
		{
			if (global::UnityEngine.Time.time - lastBuildNumberClickTime > 2f)
			{
				buildNumberClickCount = 0;
			}
			buildNumberClickCount++;
			lastBuildNumberClickTime = global::UnityEngine.Time.time;
			if (buildNumberClickCount >= 7)
			{
				int num = global::UnityEngine.PlayerPrefs.GetInt("DebugConsoleEnabled", 0);
				int num2 = (num == 0) ? 1 : 0;
				global::UnityEngine.PlayerPrefs.SetInt("DebugConsoleEnabled", num2);
				global::UnityEngine.PlayerPrefs.Save();
				buildNumberClickCount = 0;
				popupMessageSignal.Dispatch("Debug Console " + ((num2 == 1) ? "Enabled" : "Disabled") + "\nPlease re-open settings menu.", global::Kampai.UI.View.PopupMessageType.NORMAL);
			}
		}
	}
}
