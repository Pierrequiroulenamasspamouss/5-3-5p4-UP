namespace Facebook.Unity.Mobile.Android
{
	internal class FBJavaClass : global::Facebook.Unity.Mobile.Android.IAndroidJavaClass
	{
		private const string FacebookJavaClassName = "com.facebook.unity.FB";

		private global::UnityEngine.AndroidJavaClass facebookJavaClass = new global::UnityEngine.AndroidJavaClass("com.facebook.unity.FB");

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
