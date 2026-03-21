namespace Discord.Unity.Mobile.Android
{
	internal class FBJavaClass : global::Discord.Unity.Mobile.Android.IAndroidJavaClass
	{
		private const string FacebookJavaClassName = "com.discord.unity.FB";

		private global::UnityEngine.AndroidJavaClass _facebookJavaClass;

		private global::UnityEngine.AndroidJavaClass facebookJavaClass
		{
			get
			{
				if (_facebookJavaClass == null)
				{
					try
					{
						_facebookJavaClass = new global::UnityEngine.AndroidJavaClass("com.discord.unity.FB");
					}
					catch (global::System.Exception ex)
					{
						global::UnityEngine.Debug.LogWarning("Failed to initialize Facebook Java class: " + ex.Message);
					}
				}
				return _facebookJavaClass;
			}
		}

		public T CallStatic<T>(string methodName)
		{
			if (facebookJavaClass == null) return default(T);
			return facebookJavaClass.CallStatic<T>(methodName, new object[0]);
		}

		public void CallStatic(string methodName, params object[] args)
		{
			if (facebookJavaClass == null) return;
			facebookJavaClass.CallStatic(methodName, args);
		}
	}
}
