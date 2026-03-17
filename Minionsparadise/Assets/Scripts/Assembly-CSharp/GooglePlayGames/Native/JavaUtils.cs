namespace GooglePlayGames.Native
{
	internal static class JavaUtils
	{
		private static global::System.Reflection.ConstructorInfo IntPtrConstructor = typeof(global::UnityEngine.AndroidJavaObject).GetConstructor(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.NonPublic, null, new global::System.Type[1] { typeof(global::System.IntPtr) }, null);

		internal static global::UnityEngine.AndroidJavaObject JavaObjectFromPointer(global::System.IntPtr jobject)
		{
			if (jobject == global::System.IntPtr.Zero)
			{
				return null;
			}
			return (global::UnityEngine.AndroidJavaObject)IntPtrConstructor.Invoke(new object[1] { jobject });
		}

		internal static global::UnityEngine.AndroidJavaObject NullSafeCall(this global::UnityEngine.AndroidJavaObject target, string methodName, params object[] args)
		{
			try
			{
				return target.Call<global::UnityEngine.AndroidJavaObject>(methodName, args);
			}
			catch (global::System.Exception ex)
			{
				if (ex.Message.Contains("null"))
				{
					return null;
				}
				global::GooglePlayGames.OurUtils.Logger.w("CallObjectMethod exception: " + ex);
				return null;
			}
		}
	}
}
