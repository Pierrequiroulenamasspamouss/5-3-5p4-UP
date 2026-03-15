namespace Kampai.UI.View
{
	public class NotificationsPanelMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.NotificationsPanelView>
	{
		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDevicePrefsService service { get; set; }

		[Inject]
		public global::Kampai.Game.CancelAllNotificationSignal cancelAllNotificationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CancelNotificationSignal cancelNotificationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SaveDevicePrefsSignal saveDevicePrefsSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			if (base.view != null)
			{
				Start();
			}
		}

		private void Start()
		{
			base.view.skrimButton.ClickedSignal.AddListener(CloseButton);
			base.view.closeButton.ClickedSignal.AddListener(CloseButton);
			base.view.toggleAllButton.ClickedSignal.AddListener(ToggleAll);
			base.view.checkBoxes[0].isOn = service.GetDevicePrefs().ConstructionNotif;
			base.view.checkBoxes[1].isOn = service.GetDevicePrefs().BlackMarketNotif;
			base.view.checkBoxes[2].isOn = service.GetDevicePrefs().MinionsParadiseNotif;
			base.view.checkBoxes[3].isOn = service.GetDevicePrefs().BaseResourceNotif;
			base.view.checkBoxes[4].isOn = service.GetDevicePrefs().CraftingNotif;
			base.view.checkBoxes[5].isOn = service.GetDevicePrefs().EventNotif;
			base.view.checkBoxes[6].isOn = service.GetDevicePrefs().MarketPlaceNotif;
			base.view.checkBoxes[7].isOn = service.GetDevicePrefs().SocialEventNotif;
			SubscribeOnCheckboxEvents(true);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.view.skrimButton.ClickedSignal.RemoveListener(CloseButton);
			base.view.closeButton.ClickedSignal.RemoveListener(CloseButton);
			base.view.toggleAllButton.ClickedSignal.RemoveListener(ToggleAll);
			SubscribeOnCheckboxEvents(false);
		}

		private void CloseButton()
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			Close();
		}

		protected override void Close()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.view.gameObject.SetActive(false);
				SaveNotificationSettings();
			}
		}

		private void CancelPendingNotifications()
		{
			bool flag = false;
			global::UnityEngine.UI.Toggle[] checkBoxes = base.view.checkBoxes;
			foreach (global::UnityEngine.UI.Toggle toggle in checkBoxes)
			{
				if (toggle.isOn)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				cancelAllNotificationSignal.Dispatch();
				return;
			}
			if (!service.GetDevicePrefs().ConstructionNotif)
			{
				cancelNotificationSignal.Dispatch(global::Kampai.Game.NotificationType.BuildingConstructionComplete.ToString());
			}
			if (!service.GetDevicePrefs().BlackMarketNotif)
			{
				cancelNotificationSignal.Dispatch(global::Kampai.Game.NotificationType.BlackMarket.ToString());
			}
			if (!service.GetDevicePrefs().MinionsParadiseNotif)
			{
				cancelNotificationSignal.Dispatch(global::Kampai.Game.NotificationType.Reengage.ToString());
			}
			if (!service.GetDevicePrefs().BaseResourceNotif)
			{
				cancelNotificationSignal.Dispatch(global::Kampai.Game.NotificationType.MinionTasksComplete.ToString());
			}
			if (!service.GetDevicePrefs().CraftingNotif)
			{
				cancelNotificationSignal.Dispatch(global::Kampai.Game.NotificationType.CraftingComplete.ToString());
			}
			if (!service.GetDevicePrefs().EventNotif)
			{
				cancelNotificationSignal.Dispatch(global::Kampai.Game.NotificationType.QuestWarning.ToString());
			}
			if (!service.GetDevicePrefs().MarketPlaceNotif)
			{
				cancelNotificationSignal.Dispatch(global::Kampai.Game.NotificationType.MarketplaceSaleComplete.ToString());
			}
			if (!service.GetDevicePrefs().SocialEventNotif)
			{
				cancelNotificationSignal.Dispatch(global::Kampai.Game.NotificationType.SocialEventComplete.ToString());
				cancelNotificationSignal.Dispatch(global::Kampai.Game.NotificationType.SocialEventStarted.ToString());
			}
		}

		private void ToggleAll()
		{
			SubscribeOnCheckboxEvents(false);
			soundFXSignal.Dispatch("Play_button_click_01");
			bool flag = false;
			bool flag2 = false;
			global::UnityEngine.UI.Toggle[] checkBoxes = base.view.checkBoxes;
			foreach (global::UnityEngine.UI.Toggle toggle in checkBoxes)
			{
				if (!toggle.isOn)
				{
					flag2 = true;
				}
				else if (toggle.isOn)
				{
					flag = true;
				}
			}
			global::UnityEngine.UI.Toggle[] checkBoxes2 = base.view.checkBoxes;
			foreach (global::UnityEngine.UI.Toggle toggle2 in checkBoxes2)
			{
				toggle2.isOn = !flag || (flag && flag2);
			}
			SaveNotificationSettings(true);
			SubscribeOnCheckboxEvents(true);
		}

		private void SaveNotificationSettings(bool allToggled = false)
		{
			SendTelemetryFirst(allToggled);
			SendTelemetrySecond(allToggled);
			service.GetDevicePrefs().ConstructionNotif = base.view.checkBoxes[0].isOn;
			service.GetDevicePrefs().BlackMarketNotif = base.view.checkBoxes[1].isOn;
			service.GetDevicePrefs().MinionsParadiseNotif = base.view.checkBoxes[2].isOn;
			service.GetDevicePrefs().BaseResourceNotif = base.view.checkBoxes[3].isOn;
			service.GetDevicePrefs().CraftingNotif = base.view.checkBoxes[4].isOn;
			service.GetDevicePrefs().EventNotif = base.view.checkBoxes[5].isOn;
			service.GetDevicePrefs().MarketPlaceNotif = base.view.checkBoxes[6].isOn;
			service.GetDevicePrefs().SocialEventNotif = base.view.checkBoxes[7].isOn;
			saveDevicePrefsSignal.Dispatch();
			CancelPendingNotifications();
		}

		private void SendTelemetryFirst(bool allToggled)
		{
			string sourceName = "SettingsMenu";
			if (allToggled)
			{
				telemetryService.Send_Telemetry_EVT_NOTE_SETTING_CHANGE("AllNotif", (!base.view.checkBoxes[0].isOn) ? "Disabled" : "Enabled", "SettingsMenu");
			}
			if (!allToggled && service.GetDevicePrefs().ConstructionNotif != base.view.checkBoxes[0].isOn)
			{
				telemetryService.Send_Telemetry_EVT_NOTE_SETTING_CHANGE("ConstructionNotif", (!base.view.checkBoxes[0].isOn) ? "Disabled" : "Enabled", sourceName);
			}
			if (!allToggled && service.GetDevicePrefs().BlackMarketNotif != base.view.checkBoxes[1].isOn)
			{
				telemetryService.Send_Telemetry_EVT_NOTE_SETTING_CHANGE("BlackMarketNotif", (!base.view.checkBoxes[1].isOn) ? "Disabled" : "Enabled", sourceName);
			}
			if (!allToggled && service.GetDevicePrefs().MinionsParadiseNotif != base.view.checkBoxes[2].isOn)
			{
				telemetryService.Send_Telemetry_EVT_NOTE_SETTING_CHANGE("BlackMarketNotif", (!base.view.checkBoxes[2].isOn) ? "Disabled" : "Enabled", sourceName);
			}
		}

		private void SendTelemetrySecond(bool allToggled)
		{
			string sourceName = "SettingsMenu";
			if (!allToggled && service.GetDevicePrefs().BaseResourceNotif != base.view.checkBoxes[3].isOn)
			{
				telemetryService.Send_Telemetry_EVT_NOTE_SETTING_CHANGE("BaseResourceNotif", (!base.view.checkBoxes[3].isOn) ? "Disabled" : "Enabled", sourceName);
			}
			if (!allToggled && service.GetDevicePrefs().CraftingNotif != base.view.checkBoxes[4].isOn)
			{
				telemetryService.Send_Telemetry_EVT_NOTE_SETTING_CHANGE("CraftingNotif", (!base.view.checkBoxes[4].isOn) ? "Disabled" : "Enabled", sourceName);
			}
			if (!allToggled && service.GetDevicePrefs().EventNotif != base.view.checkBoxes[5].isOn)
			{
				telemetryService.Send_Telemetry_EVT_NOTE_SETTING_CHANGE("EventNotif", (!base.view.checkBoxes[5].isOn) ? "Disabled" : "Enabled", sourceName);
			}
			if (!allToggled && service.GetDevicePrefs().MarketPlaceNotif != base.view.checkBoxes[6].isOn)
			{
				telemetryService.Send_Telemetry_EVT_NOTE_SETTING_CHANGE("MarketPlaceNotif", (!base.view.checkBoxes[6].isOn) ? "Disabled" : "Enabled", sourceName);
			}
			if (!allToggled && service.GetDevicePrefs().SocialEventNotif != base.view.checkBoxes[7].isOn)
			{
				telemetryService.Send_Telemetry_EVT_NOTE_SETTING_CHANGE("SocialEventNotif", (!base.view.checkBoxes[7].isOn) ? "Disabled" : "Enabled", sourceName);
			}
		}

		private void SubscribeOnCheckboxEvents(bool enabled)
		{
			if (enabled)
			{
				global::UnityEngine.UI.Toggle[] checkBoxes = base.view.checkBoxes;
				foreach (global::UnityEngine.UI.Toggle toggle in checkBoxes)
				{
					toggle.onValueChanged.AddListener(CheckBoxChecked);
				}
			}
			else
			{
				global::UnityEngine.UI.Toggle[] checkBoxes2 = base.view.checkBoxes;
				foreach (global::UnityEngine.UI.Toggle toggle2 in checkBoxes2)
				{
					toggle2.onValueChanged.RemoveAllListeners();
				}
			}
		}

		private void CheckBoxChecked(bool value)
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			SaveNotificationSettings();
		}
	}
}
