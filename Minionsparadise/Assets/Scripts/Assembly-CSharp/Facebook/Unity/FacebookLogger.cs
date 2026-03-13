namespace Facebook.Unity
{
	internal static class FacebookLogger
	{
		private class CustomLogger : global::Facebook.Unity.IFacebookLogger
		{
			private global::Facebook.Unity.IFacebookLogger logger;

			public CustomLogger()
			{
				logger = new global::Facebook.Unity.FacebookLogger.AndroidLogger();
			}

			public void Log(string msg)
			{
				if (global::UnityEngine.Debug.isDebugBuild)
				{
					global::UnityEngine.Debug.Log(msg);
					logger.Log(msg);
				}
			}

			public void Info(string msg)
			{
				global::UnityEngine.Debug.Log(msg);
				logger.Info(msg);
			}

			public void Warn(string msg)
			{
				global::UnityEngine.Debug.LogWarning(msg);
				logger.Warn(msg);
			}

			public void Error(string msg)
			{
				global::UnityEngine.Debug.LogError(msg);
				logger.Error(msg);
			}
		}

		private class AndroidLogger : global::Facebook.Unity.IFacebookLogger
		{
			public void Log(string msg)
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("v", new object[2] { "Facebook.Unity.FBDebug", msg });
				}
			}

			public void Info(string msg)
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("i", new object[2] { "Facebook.Unity.FBDebug", msg });
				}
			}

			public void Warn(string msg)
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("w", new object[2] { "Facebook.Unity.FBDebug", msg });
				}
			}

			public void Error(string msg)
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("e", new object[2] { "Facebook.Unity.FBDebug", msg });
				}
			}
		}

		private const string UnityAndroidTag = "Facebook.Unity.FBDebug";

		internal static global::Facebook.Unity.IFacebookLogger Instance { private get; set; }

		static FacebookLogger()
		{
			Instance = new global::Facebook.Unity.FacebookLogger.CustomLogger();
		}

		public static void Log(string msg)
		{
			Instance.Log(msg);
		}

		public static void Log(string format, params string[] args)
		{
			Log(string.Format(format, args));
		}

		public static void Info(string msg)
		{
			Instance.Info(msg);
		}

		public static void Info(string format, params string[] args)
		{
			Info(string.Format(format, args));
		}

		public static void Warn(string msg)
		{
			Instance.Warn(msg);
		}

		public static void Warn(string format, params string[] args)
		{
			Warn(string.Format(format, args));
		}

		public static void Error(string msg)
		{
			Instance.Error(msg);
		}

		public static void Error(string format, params string[] args)
		{
			Error(string.Format(format, args));
		}
	}
}
