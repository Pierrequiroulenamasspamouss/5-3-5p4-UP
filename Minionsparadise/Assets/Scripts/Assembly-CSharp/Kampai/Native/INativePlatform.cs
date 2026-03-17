namespace Kampai.Util
{
	public interface INativePlatform
	{
		string StaticConfig { get; }
		string BundleVersion { get; }
		string BundleIdentifier { get; }

		bool IsUserMusicPlaying();

		void LogError(string text);
		void LogWarning(string text);
		void LogInfo(string text);
		void LogDebug(string text);
		void LogVerbose(string text);

		string GetFilePath();
		string GetLastFilePath();
		string GetLogFilesContent(int maxsize);
		uint GetMemoryUsage();

		void Crash();
		void ScheduleLocalNotification(string type, int secondsFromNow, string title, string text, string stackedTitle, string stackedText, string action, string sound, string launchImage, int badgeNumber);
		void CancelLocalNotification(string type);
		void CancelAllLocalNotifications();

		string GetDeviceLanguage();
		bool AutorotationIsOSAllowed();

		bool GetBackupFlag(string path);
		void SetBackupFlag(string path, bool shouldBackup);

		void AndroidFileLog(string text);
		void CloseFileLog();

		int GetAndroidOSVersion();

		string GetPersistentDataPath();
		ulong GetAvailableStorage(string path);
		bool CanShowNetworkSettings();
		void OpenNetworkSettings();

		byte[] GetStreamingAsset(string path);
		string GetStreamingTextAsset(string path);

		void Exit();
		bool AreNotificationsEnabled();
		string GetDeviceHardwareModel();
		long GetAppStartupTime();
		void OpenAppStoreLink(string appId);
		bool IsAppInstalled(string appIdURL);
		void LaunchApp(string appId);
		bool CanOpenURL(string URL);
		void OpenURL(string URL);
	}
}
