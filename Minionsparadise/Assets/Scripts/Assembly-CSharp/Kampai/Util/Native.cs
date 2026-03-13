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

		private static string bundleVersion = null;

		private static string bundleIdentifier = null;

		private static global::System.IO.StreamWriter sw = null;

		private static int androidOSVersion = 0;

		private static readonly long APP_START_TIME = getAppStartTime();

		public static string StaticConfig
		{
			get
			{
#if UNITY_ANDROID && !UNITY_EDITOR
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					global::UnityEngine.AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<global::UnityEngine.AndroidJavaObject>("currentActivity");
					string text = androidJavaObject.Call<string>("getPackageName", new object[0]);
					global::UnityEngine.AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<global::UnityEngine.AndroidJavaObject>("getResources", new object[0]);
					int num = androidJavaObject2.Call<int>("getIdentifier", new object[3] { "config", "raw", text });
					global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
					global::UnityEngine.AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<global::UnityEngine.AndroidJavaObject>("openRawResource", new object[1] { num });
					global::UnityEngine.AndroidJavaObject androidJavaObject4 = new global::UnityEngine.AndroidJavaObject("java.io.InputStreamReader", androidJavaObject3);
					global::UnityEngine.AndroidJavaObject androidJavaObject5 = new global::UnityEngine.AndroidJavaObject("java.io.BufferedReader", androidJavaObject4);
					string value;
					while ((value = androidJavaObject5.Call<string>("readLine", new object[0])) != null)
					{
						stringBuilder.Append(value);
					}
					return stringBuilder.ToString();
				}
#else
				string path = global::System.IO.Path.Combine(global::UnityEngine.Application.streamingAssetsPath, "config.json");
#if !UNITY_WEBPLAYER
				if (global::System.IO.File.Exists(path))
				{
					return global::System.IO.File.ReadAllText(path).Trim();
				}
#else
				if (!path.Contains("://"))
				{
					path = "file://" + path;
				}
				global::UnityEngine.WWW www = new global::UnityEngine.WWW(path);
				while (!www.isDone) {}
				if (string.IsNullOrEmpty(www.error))
				{
					return www.text.Trim();
				}
#endif
				return string.Empty;
#endif
			}
		}

		public static string BundleVersion
		{
			get
			{
				if (bundleVersion == null)
				{
#if UNITY_ANDROID && !UNITY_EDITOR
					using (global::UnityEngine.AndroidJavaObject androidJavaObject = GetCurrentActivity())
					{
						using (global::UnityEngine.AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<global::UnityEngine.AndroidJavaObject>("getPackageManager", new object[0]))
						{
							using (global::UnityEngine.AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<global::UnityEngine.AndroidJavaObject>("getPackageInfo", new object[2] { BundleIdentifier, 0 }))
							{
								bundleVersion = androidJavaObject3.Get<string>("versionName");
							}
						}
					}
#else
					bundleVersion = "1.0.0";
#endif
				}
				return bundleVersion;
			}
		}

		public static string BundleIdentifier
		{
			get
			{
				if (bundleIdentifier == null)
				{
#if UNITY_ANDROID && !UNITY_EDITOR
					using (global::UnityEngine.AndroidJavaObject androidJavaObject = GetCurrentActivity())
					{
						bundleIdentifier = androidJavaObject.Call<string>("getPackageName", new object[0]);
					}
#else
					bundleIdentifier = global::UnityEngine.Application.bundleIdentifier;
#endif
				}
				return bundleIdentifier;
			}
		}

		[global::System.Runtime.InteropServices.DllImport("minions")]
		private static extern int Minions_Util_Native_Log(int logLevel, string tag, string msg);

		public static bool IsUserMusicPlaying()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.MainActivity"))
			{
				return androidJavaClass.CallStatic<bool>("isUserMusicPlaying", new object[0]);
			}
#else
			return false;
#endif
		}

		public static void LogError(string text)
		{
			Minions_Util_Native_Log(6, "Minions", text);
			AndroidFileLog(text);
		}

		public static void LogWarning(string text)
		{
			Minions_Util_Native_Log(5, "Minions", text);
			AndroidFileLog(text);
		}

		public static void LogInfo(string text)
		{
			Minions_Util_Native_Log(4, "Minions", text);
			AndroidFileLog(text);
		}

		public static void LogDebug(string text)
		{
			Minions_Util_Native_Log(3, "Minions", text);
			AndroidFileLog(text);
		}

		public static void LogVerbose(string text)
		{
			Minions_Util_Native_Log(2, "Minions", text);
			AndroidFileLog(text);
		}

		public static string GetFilePath()
		{
			string empty = string.Empty;
			return global::Kampai.Util.GameConstants.PERSISTENT_DATA_PATH + "/log.txt";
		}

		public static string GetLastFilePath()
		{
			return global::Kampai.Util.GameConstants.PERSISTENT_DATA_PATH + "/log-last.txt";
		}

		public static string GetLogFilesContent(int maxsize)
		{
			string result = string.Empty;
			string lastFilePath = GetLastFilePath();
#if !UNITY_WEBPLAYER
			if (global::System.IO.File.Exists(lastFilePath))
			{
				string text = global::System.IO.File.ReadAllText(lastFilePath);
				result = ((text.Length <= maxsize) ? text : text.Substring(text.Length - maxsize));
			}
#endif
			return result;
		}

		public static uint GetMemoryUsage()
		{
			return global::UnityEngine.Profiler.usedHeapSize;
		}

		public static void Crash()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.Misc"))
			{
				androidJavaClass.CallStatic("crash");
			}
#endif
		}

		public static void ScheduleLocalNotification(string type, int secondsFromNow, string title, string text, string stackedTitle, string stackedText, string action, string sound, string launchImage, int badgeNumber = 1)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = GetNotificationManagerHelper())
			{
				androidJavaClass.CallStatic("scheduleNotification", (long)global::System.TimeSpan.FromSeconds(secondsFromNow).TotalMilliseconds, type, launchImage, sound, title, text, stackedTitle, stackedText, badgeNumber);
			}
#endif
		}

		public static void CancelLocalNotification(string type)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = GetNotificationManagerHelper())
			{
				androidJavaClass.CallStatic("cancelNotification", type, false);
			}
#endif
		}

		public static void CancelAllLocalNotifications()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = GetNotificationManagerHelper())
			{
				androidJavaClass.CallStatic("cancelAllNotifications");
			}
#endif
		}

		public static string GetDeviceLanguage()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("java.util.Locale"))
			{
				using (global::UnityEngine.AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<global::UnityEngine.AndroidJavaObject>("getDefault", new object[0]))
				{
					return androidJavaObject.Call<string>("toString", new object[0]);
				}
			}
#else
			return global::UnityEngine.Application.systemLanguage.ToString();
#endif
		}

		public static bool AutorotationIsOSAllowed()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.Misc"))
			{
				int num = androidJavaClass.CallStatic<int>("getAutorotateSetting", new object[0]);
				return num != 0;
			}
#else
			return true;
#endif
		}

		public static bool GetBackupFlag(string path)
		{
			return false;
		}

		public static void SetBackupFlag(string path, bool shouldBackup)
		{
		}

		public static void AndroidFileLog(string text)
		{
#if !UNITY_WEBPLAYER
			string filePath = GetFilePath();
			if (sw == null)
			{
				if (!global::System.IO.File.Exists(filePath))
				{
					sw = global::System.IO.File.CreateText(filePath);
				}
				else
				{
					sw = global::System.IO.File.AppendText(filePath);
				}
			}
			if (sw != null)
			{
				sw.WriteLine(text + "\n");
			}
#endif
		}

		public static void CloseFileLog()
		{
#if !UNITY_WEBPLAYER
			if (sw != null)
			{
				sw.Close();
				sw = null;
			}
			if (global::System.IO.File.Exists(GetLastFilePath()))
			{
				global::System.IO.File.Delete(GetLastFilePath());
			}
			if (global::System.IO.File.Exists(GetFilePath()))
			{
				global::System.IO.File.Copy(GetFilePath(), GetLastFilePath());
			}
#endif
		}

		public static int GetAndroidOSVersion()
		{
			if (androidOSVersion == 0)
			{
#if UNITY_ANDROID && !UNITY_EDITOR
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("android.os.Build$VERSION"))
				{
					androidOSVersion = androidJavaClass.GetStatic<int>("SDK_INT");
				}
#else
				androidOSVersion = 19;
#endif
			}
			return androidOSVersion;
		}

		public static global::UnityEngine.AndroidJavaObject GetCurrentActivity()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				return androidJavaClass.GetStatic<global::UnityEngine.AndroidJavaObject>("currentActivity");
			}
#else
			return null;
#endif
		}

		public static global::UnityEngine.AndroidJavaClass GetNotificationManagerHelper()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.notifications.NotificationManagerHelper");
#else
			return null;
#endif
		}

		public static string GetPersistentDataPath()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.FileUtils"))
			{
				return androidJavaClass.CallStatic<string>("getPersistentDataPath", new object[0]);
			}
#else
			return global::UnityEngine.Application.persistentDataPath;
#endif
		}

		public static ulong GetAvailableStorage(string path)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.FileUtils"))
			{
				return (ulong)((!string.IsNullOrEmpty(path)) ? androidJavaClass.CallStatic<long>("getAvailableStorage", new object[1] { path }) : androidJavaClass.CallStatic<long>("getAvailableInternalStorage", new object[0]));
			}
#else
			return 1024uL * 1024uL * 1024uL;
#endif
		}

		public static bool CanShowNetworkSettings()
		{
			return true;
		}

		public static void OpenNetworkSettings()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.Misc"))
			{
				androidJavaClass.CallStatic("openNetworkSettings");
			}
#endif
		}

		public static byte[] GetStreamingAsset(string path)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			byte[] result = null;
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.FileUtils"))
			{
				try
				{
					result = androidJavaClass.CallStatic<byte[]>("openAsset", new object[1] { path });
				}
				catch (global::UnityEngine.AndroidJavaException ex)
				{
					global::UnityEngine.AndroidJavaClass androidJavaClass2 = new global::UnityEngine.AndroidJavaClass("java.io.FileNotFoundException");
					if (androidJavaClass2.GetType().IsInstanceOfType(ex))
					{
						throw new global::System.IO.FileNotFoundException(ex.ToString());
					}
					LogError("Error opening asset: " + ex.ToString());
				}
			}
			return result;
#else
			string fullPath = global::System.IO.Path.Combine(global::UnityEngine.Application.streamingAssetsPath, path);
#if !UNITY_WEBPLAYER
			if (global::System.IO.File.Exists(fullPath))
			{
				return global::System.IO.File.ReadAllBytes(fullPath);
			}
			throw new global::System.IO.FileNotFoundException(fullPath);
#else
			if (!fullPath.Contains("://"))
			{
				fullPath = "file://" + fullPath;
			}
			global::UnityEngine.WWW www = new global::UnityEngine.WWW(fullPath);
			while (!www.isDone) {}
			if (!string.IsNullOrEmpty(www.error))
			{
				throw new global::System.IO.FileNotFoundException(fullPath);
			}
			return www.bytes;
#endif
#endif
		}

		public static string GetStreamingTextAsset(string path)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			string result = null;
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.FileUtils"))
			{
				try
				{
					result = androidJavaClass.CallStatic<string>("openTextAsset", new object[1] { path });
				}
				catch (global::UnityEngine.AndroidJavaException ex)
				{
					global::UnityEngine.AndroidJavaClass androidJavaClass2 = new global::UnityEngine.AndroidJavaClass("java.io.FileNotFoundException");
					if (androidJavaClass2.GetType().IsInstanceOfType(ex))
					{
						throw new global::System.IO.FileNotFoundException(ex.ToString());
					}
					LogError("Error opening text asset: " + ex.ToString());
				}
			}
			return result;
#else
			string fullPath = global::System.IO.Path.Combine(global::UnityEngine.Application.streamingAssetsPath, path);
#if !UNITY_WEBPLAYER
			if (global::System.IO.File.Exists(fullPath))
			{
				return global::System.IO.File.ReadAllText(fullPath);
			}
			throw new global::System.IO.FileNotFoundException(fullPath);
#else
			if (!fullPath.Contains("://"))
			{
				fullPath = "file://" + fullPath;
			}
			global::UnityEngine.WWW www = new global::UnityEngine.WWW(fullPath);
			while (!www.isDone) {}
			if (!string.IsNullOrEmpty(www.error))
			{
				throw new global::System.IO.FileNotFoundException(fullPath);
			}
			return www.text;
#endif
#endif
		}

		public static void Exit()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaObject androidJavaObject = GetCurrentActivity())
			{
				androidJavaObject.Call<bool>("moveTaskToBack", new object[1] { true });
			}
#else
			global::UnityEngine.Application.Quit();
#endif
		}

		public static bool AreNotificationsEnabled()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			if (GetAndroidOSVersion() >= 19)
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = GetNotificationManagerHelper())
				{
					return androidJavaClass.CallStatic<bool>("isNotificationEnabled", new object[0]);
				}
			}
#endif
			return true;
		}

		public static string GetDeviceHardwareModel()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("android.os.Build"))
			{
				return androidJavaClass.GetStatic<string>("HARDWARE");
			}
#else
			return global::UnityEngine.SystemInfo.deviceModel;
#endif
		}

		private static long getAppStartTime()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.MinionsApplication"))
			{
				return androidJavaClass.GetStatic<long>("APP_START_TIME");
			}
#else
			return 0L;
#endif
		}

		public static long GetAppStartupTime()
		{
			return APP_START_TIME;
		}

		public static void OpenAppStoreLink(string appId)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.Misc"))
			{
				androidJavaClass.CallStatic("OpenAppStoreLink", appId);
			}
#endif
		}

		public static bool IsAppInstalled(string appIdURL)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
			global::UnityEngine.AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<global::UnityEngine.AndroidJavaObject>("currentActivity");
			global::UnityEngine.AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<global::UnityEngine.AndroidJavaObject>("getPackageManager", new object[0]);
			try
			{
				androidJavaObject2.Call<global::UnityEngine.AndroidJavaObject>("getLaunchIntentForPackage", new object[1] { appIdURL });
				return true;
			}
			catch (global::System.Exception)
			{
			}
#endif
			return false;
		}

		public static void LaunchApp(string appId)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
			global::UnityEngine.AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<global::UnityEngine.AndroidJavaObject>("currentActivity");
			global::UnityEngine.AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<global::UnityEngine.AndroidJavaObject>("getPackageManager", new object[0]);
			try
			{
				global::UnityEngine.AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<global::UnityEngine.AndroidJavaObject>("getLaunchIntentForPackage", new object[1] { appId });
				androidJavaObject.Call("startActivity", androidJavaObject3);
			}
			catch (global::System.Exception)
			{
				LogWarning("Error attempting to launch App.");
			}
#endif
		}

		public static bool CanOpenURL(string URL)
		{
			return false;
		}

		public static void OpenURL(string URL)
		{
			global::UnityEngine.Application.OpenURL(URL);
		}
	}
}
