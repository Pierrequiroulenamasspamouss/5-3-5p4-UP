#if UNITY_ANDROID && !UNITY_EDITOR
using Kampai.Util;
using UnityEngine;

namespace Kampai.Util
{
	public class NativeAndroid : INativePlatform
	{
		private static string bundleVersion = null;
		private static string bundleIdentifier = null;
		private static global::System.IO.StreamWriter sw = null;
		private static int androidOSVersion = 0;
		private static readonly long APP_START_TIME = getAppStartTime();

		[global::System.Runtime.InteropServices.DllImport("minions")]
		private static extern int Minions_Util_Native_Log(int logLevel, string tag, string msg);

		private static global::UnityEngine.AndroidJavaObject GetCurrentActivity()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				return androidJavaClass.GetStatic<global::UnityEngine.AndroidJavaObject>("currentActivity");
			}
		}

		private static global::UnityEngine.AndroidJavaClass GetNotificationManagerHelper()
		{
			return new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.notifications.NotificationManagerHelper");
		}

		private static long getAppStartTime()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.MinionsApplication"))
			{
				return androidJavaClass.GetStatic<long>("APP_START_TIME");
			}
		}

		public string StaticConfig
		{
			get
			{
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
			}
		}

		public string BundleVersion
		{
			get
			{
				if (bundleVersion == null)
				{
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
				}
				return bundleVersion;
			}
		}

		public string BundleIdentifier
		{
			get
			{
				if (bundleIdentifier == null)
				{
					using (global::UnityEngine.AndroidJavaObject androidJavaObject = GetCurrentActivity())
					{
						bundleIdentifier = androidJavaObject.Call<string>("getPackageName", new object[0]);
					}
				}
				return bundleIdentifier;
			}
		}

		public bool IsUserMusicPlaying()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.MainActivity"))
			{
				return androidJavaClass.CallStatic<bool>("isUserMusicPlaying", new object[0]);
			}
		}

		public void LogError(string text)
		{
			Minions_Util_Native_Log(6, "Minions", text);
			AndroidFileLog(text);
		}

		public void LogWarning(string text)
		{
			Minions_Util_Native_Log(5, "Minions", text);
			AndroidFileLog(text);
		}

		public void LogInfo(string text)
		{
			Minions_Util_Native_Log(4, "Minions", text);
			AndroidFileLog(text);
		}

		public void LogDebug(string text)
		{
			Minions_Util_Native_Log(3, "Minions", text);
			AndroidFileLog(text);
		}

		public void LogVerbose(string text)
		{
			Minions_Util_Native_Log(2, "Minions", text);
			AndroidFileLog(text);
		}

		public string GetFilePath()
		{
			return global::Kampai.Util.GameConstants.PERSISTENT_DATA_PATH + "/log.txt";
		}

		public string GetLastFilePath()
		{
			return global::Kampai.Util.GameConstants.PERSISTENT_DATA_PATH + "/log-last.txt";
		}

		public string GetLogFilesContent(int maxsize)
		{
			string result = string.Empty;
			string lastFilePath = GetLastFilePath();
			if (global::System.IO.File.Exists(lastFilePath))
			{
				string text = global::System.IO.File.ReadAllText(lastFilePath);
				result = ((text.Length <= maxsize) ? text : text.Substring(text.Length - maxsize));
			}
			return result;
		}

		public uint GetMemoryUsage()
		{
			return global::UnityEngine.Profiler.usedHeapSize;
		}

		public void Crash()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.Misc"))
			{
				androidJavaClass.CallStatic("crash");
			}
		}

		public void ScheduleLocalNotification(string type, int secondsFromNow, string title, string text, string stackedTitle, string stackedText, string action, string sound, string launchImage, int badgeNumber)
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = GetNotificationManagerHelper())
			{
				androidJavaClass.CallStatic("scheduleNotification", (long)global::System.TimeSpan.FromSeconds(secondsFromNow).TotalMilliseconds, type, launchImage, sound, title, text, stackedTitle, stackedText, badgeNumber);
			}
		}

		public void CancelLocalNotification(string type)
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = GetNotificationManagerHelper())
			{
				androidJavaClass.CallStatic("cancelNotification", type, false);
			}
		}

		public void CancelAllLocalNotifications()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = GetNotificationManagerHelper())
			{
				androidJavaClass.CallStatic("cancelAllNotifications");
			}
		}

		public string GetDeviceLanguage()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("java.util.Locale"))
			{
				using (global::UnityEngine.AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<global::UnityEngine.AndroidJavaObject>("getDefault", new object[0]))
				{
					return androidJavaObject.Call<string>("toString", new object[0]);
				}
			}
		}

		public bool AutorotationIsOSAllowed()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.Misc"))
			{
				int num = androidJavaClass.CallStatic<int>("getAutorotateSetting", new object[0]);
				return num != 0;
			}
		}

		public bool GetBackupFlag(string path)
		{
			return false;
		}

		public void SetBackupFlag(string path, bool shouldBackup)
		{
		}

		public void AndroidFileLog(string text)
		{
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
		}

		public void CloseFileLog()
		{
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
		}

		public int GetAndroidOSVersion()
		{
			if (androidOSVersion == 0)
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("android.os.Build$VERSION"))
				{
					androidOSVersion = androidJavaClass.GetStatic<int>("SDK_INT");
				}
			}
			return androidOSVersion;
		}

		public string GetPersistentDataPath()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.FileUtils"))
			{
				return androidJavaClass.CallStatic<string>("getPersistentDataPath", new object[0]);
			}
		}

		public ulong GetAvailableStorage(string path)
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.FileUtils"))
			{
				return (ulong)((!string.IsNullOrEmpty(path)) ? androidJavaClass.CallStatic<long>("getAvailableStorage", new object[1] { path }) : androidJavaClass.CallStatic<long>("getAvailableInternalStorage", new object[0]));
			}
		}

		public bool CanShowNetworkSettings()
		{
			return true;
		}

		public void OpenNetworkSettings()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.Misc"))
			{
				androidJavaClass.CallStatic("openNetworkSettings");
			}
		}

		public byte[] GetStreamingAsset(string path)
		{
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
					global::UnityEngine.Debug.LogError("Error opening asset: " + ex.ToString());
				}
			}
			return result;
		}

		public string GetStreamingTextAsset(string path)
		{
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
					global::UnityEngine.Debug.LogError("Error opening text asset: " + ex.ToString());
				}
			}
			return result;
		}

		public void Exit()
		{
			using (global::UnityEngine.AndroidJavaObject androidJavaObject = GetCurrentActivity())
			{
				androidJavaObject.Call<bool>("moveTaskToBack", new object[1] { true });
			}
		}

		public bool AreNotificationsEnabled()
		{
			if (GetAndroidOSVersion() >= 19)
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = GetNotificationManagerHelper())
				{
					return androidJavaClass.CallStatic<bool>("isNotificationEnabled", new object[0]);
				}
			}
			return true;
		}

		public string GetDeviceHardwareModel()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("android.os.Build"))
			{
				return androidJavaClass.GetStatic<string>("HARDWARE");
			}
		}

		public long GetAppStartupTime()
		{
			return APP_START_TIME;
		}

		public void OpenAppStoreLink(string appId)
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.ea.gp.minions.utils.Misc"))
			{
				androidJavaClass.CallStatic("OpenAppStoreLink", appId);
			}
		}

		public bool IsAppInstalled(string appIdURL)
		{
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
			return false;
		}

		public void LaunchApp(string appId)
		{
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
		}

		public bool CanOpenURL(string URL)
		{
			return false;
		}

		public void OpenURL(string URL)
		{
			global::UnityEngine.Application.OpenURL(URL);
		}
	}
}
#endif
