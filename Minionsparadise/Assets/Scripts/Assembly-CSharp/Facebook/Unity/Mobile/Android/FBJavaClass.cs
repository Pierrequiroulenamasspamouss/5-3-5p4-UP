namespace Discord.Unity.Mobile.Android
{
	internal class FBJavaClass : global::Discord.Unity.Mobile.Android.IAndroidJavaClass
	{
		private const string FacebookJavaClassName = "com.discord.unity.FB";

		private global::UnityEngine.AndroidJavaClass facebookJavaClass = new global::UnityEngine.AndroidJavaClass("com.discord.unity.FB");

		public T CallStatic<T>(string methodName)
		{
			return facebookJavaClass.CallStatic<T>(methodName, new object[0]);
		}

		public void CallStatic(string methodName, params object[] args)
		{
			facebookJavaClass.CallStatic(methodName, args);
		}
	}
}
