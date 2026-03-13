namespace Kampai.Game
{
	public class CheckSystemNotificationSettingsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public ILocalPersistanceService localPersistence { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void Execute()
		{
			bool value = global::Kampai.Util.Native.AreNotificationsEnabled();
			int num = global::System.Convert.ToInt32(value);
			if (!localPersistence.HasKey("NotificationSettings"))
			{
				localPersistence.PutDataInt("NotificationSettings", num);
				return;
			}
			int dataInt = localPersistence.GetDataInt("NotificationSettings");
			if (dataInt != num)
			{
				telemetryService.Send_Telemetry_EVT_NOTE_SETTING_CHANGE("SystemNotifications", (num != 1) ? "Disabled" : "Enabled", "System");
				localPersistence.PutDataInt("NotificationSettings", num);
			}
		}
	}
}
