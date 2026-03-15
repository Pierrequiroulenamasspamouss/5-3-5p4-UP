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
			Init();
			setServer(ServerEnv);
			setBuild(clientVersion.GetClientVersion());
			view.MusicSlider.value = ((!global::Kampai.Audio.AudioSettingsModel.MusicMuted) ? prefs.GetDevicePrefs().MusicVolume : 0f);
			view.SFXSlider.value = prefs.GetDevicePrefs().SFXVolume;
			view.musicValue.text = ((int)(100f * view.MusicSlider.value)).ToString();
			view.soundValue.text = ((int)(100f * view.SFXSlider.value)).ToString();
			view.volumeSliderChangedSignal.AddListener(OnVolumeChanged);
		}

		private void OnDisable()
		{
			view.notificationsButton.ClickedSignal.RemoveListener(NotificationsButton);
			view.notificationsOffButton.ClickedSignal.RemoveListener(NotificationsOffButton);
			view.DLCButton.ClickedSignal.RemoveListener(DLCButton);
			view.volumeSliderChangedSignal.RemoveListener(OnVolumeChanged);
			view.doubleConfirmButton.ClickedSignal.RemoveListener(OnDoubleConfirm);
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
	}
}
