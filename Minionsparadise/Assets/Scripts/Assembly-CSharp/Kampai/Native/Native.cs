namespace Kampai.Util
{
	public static class Native
	{
		private enum LogLevel
		{
			ANDROID_LOG_VERBOSE = 2,
			ANDROID_LOG_DEBUG = 3,
			ANDROID_LOG_INFO = 4,
			ANDROID_LOG_WARN = 5,
			ANDROID_LOG_ERROR = 6,
			ANDROID_LOG_FATAL = 7
		}

		public class OnReceiveBroadcastListener : global::UnityEngine.AndroidJavaProxy
		{
			public delegate void OnReceiveBroadcast(global::UnityEngine.AndroidJavaObject context, global::UnityEngine.AndroidJavaObject intent);

			private static readonly string NATIVE_INTERFACE = string.Format("com.ea.gp.minions.app.{0}", typeof(global::Kampai.Util.Native.OnReceiveBroadcastListener).Name);

			private global::Kampai.Util.Native.OnReceiveBroadcastListener.OnReceiveBroadcast broadcastListener;

			public OnReceiveBroadcastListener(global::Kampai.Util.Native.OnReceiveBroadcastListener.OnReceiveBroadcast listener)
				: base(NATIVE_INTERFACE)
			{
				broadcastListener = listener;
			}

			public void onReceiveBroadcast(global::UnityEngine.AndroidJavaObject context, global::UnityEngine.AndroidJavaObject intent)
			{
				if (broadcastListener != null)
				{
					broadcastListener(context, intent);
					return;
				}
				context.Dispose();
				intent.Dispose();
			}
		}

		private const string MINIONS_LIB_NAME = "minions";

#if UNITY_ANDROID && !UNITY_EDITOR
		private static readonly INativePlatform impl = new NativeAndroid();
#elif UNITY_WEBPLAYER
		private static readonly INativePlatform impl = new NativeWebPlayer();
#else
		private static readonly INativePlatform impl = new NativeDefault();
#endif
		
		public static string StaticConfig { get { return impl.StaticConfig; } }
		public static string BundleVersion { get { return impl.BundleVersion; } }
		public static string BundleIdentifier { get { return impl.BundleIdentifier; } }

		public static bool IsUserMusicPlaying() { return impl.IsUserMusicPlaying(); }
		public static void LogError(string text) { impl.LogError(text); }
		public static void LogWarning(string text) { impl.LogWarning(text); }
		public static void LogInfo(string text) { impl.LogInfo(text); }
		public static void LogDebug(string text) { impl.LogDebug(text); }
		public static void LogVerbose(string text) { impl.LogVerbose(text); }

		public static string GetFilePath() { return impl.GetFilePath(); }
		public static string GetLastFilePath() { return impl.GetLastFilePath(); }
		public static string GetLogFilesContent(int maxsize) { return impl.GetLogFilesContent(maxsize); }
		public static uint GetMemoryUsage() { return impl.GetMemoryUsage(); }
		public static void Crash() { impl.Crash(); }

		public static void ScheduleLocalNotification(string type, int secondsFromNow, string title, string text, string stackedTitle, string stackedText, string action, string sound, string launchImage, int badgeNumber = 1)
		{
			impl.ScheduleLocalNotification(type, secondsFromNow, title, text, stackedTitle, stackedText, action, sound, launchImage, badgeNumber);
		}

		public static void CancelLocalNotification(string type) { impl.CancelLocalNotification(type); }
		public static void CancelAllLocalNotifications() { impl.CancelAllLocalNotifications(); }

		public static string GetDeviceLanguage() { return impl.GetDeviceLanguage(); }
		public static bool AutorotationIsOSAllowed() { return impl.AutorotationIsOSAllowed(); }

		public static bool GetBackupFlag(string path) { return impl.GetBackupFlag(path); }
		public static void SetBackupFlag(string path, bool shouldBackup) { impl.SetBackupFlag(path, shouldBackup); }

		public static void AndroidFileLog(string text) { impl.AndroidFileLog(text); }
		public static void CloseFileLog() { impl.CloseFileLog(); }

		public static int GetAndroidOSVersion() { return impl.GetAndroidOSVersion(); }

		public static string GetPersistentDataPath() { return impl.GetPersistentDataPath(); }
		public static ulong GetAvailableStorage(string path) { return impl.GetAvailableStorage(path); }
		public static bool CanShowNetworkSettings() { return impl.CanShowNetworkSettings(); }
		public static void OpenNetworkSettings() { impl.OpenNetworkSettings(); }

		public static byte[] GetStreamingAsset(string path) { return impl.GetStreamingAsset(path); }
		public static string GetStreamingTextAsset(string path) { return impl.GetStreamingTextAsset(path); }

		public static void Exit() { impl.Exit(); }
		public static bool AreNotificationsEnabled() { return impl.AreNotificationsEnabled(); }
		public static string GetDeviceHardwareModel() { return impl.GetDeviceHardwareModel(); }
		public static long GetAppStartupTime() { return impl.GetAppStartupTime(); }
		public static void OpenAppStoreLink(string appId) { impl.OpenAppStoreLink(appId); }
		public static bool IsAppInstalled(string appIdURL) { return impl.IsAppInstalled(appIdURL); }
		public static void LaunchApp(string appId) { impl.LaunchApp(appId); }
		public static bool CanOpenURL(string URL) { return impl.CanOpenURL(URL); }
		public static void OpenURL(string URL) { impl.OpenURL(URL); }
	}
}
