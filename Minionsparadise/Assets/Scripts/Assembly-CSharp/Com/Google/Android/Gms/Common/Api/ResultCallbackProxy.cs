namespace Com.Google.Android.Gms.Common.Api
{
	public abstract class ResultCallbackProxy<R> : global::Google.Developers.JavaInterfaceProxy, global::Com.Google.Android.Gms.Common.Api.ResultCallback<R> where R : global::Com.Google.Android.Gms.Common.Api.Result
	{
		private const string CLASS_NAME = "com/google/android/gms/common/api/ResultCallback";

		public ResultCallbackProxy()
			: base("com/google/android/gms/common/api/ResultCallback")
		{
		}

		public abstract void OnResult(R arg_Result_1);

		public void onResult(R arg_Result_1)
		{
			OnResult(arg_Result_1);
		}

		public void onResult(global::UnityEngine.AndroidJavaObject arg_Result_1)
		{
			global::System.IntPtr rawObject = arg_Result_1.GetRawObject();
			global::System.Reflection.ConstructorInfo constructor = typeof(R).GetConstructor(new global::System.Type[1] { rawObject.GetType() });
			R val;
			if (constructor != null)
			{
				val = (R)constructor.Invoke(new object[1] { rawObject });
			}
			else
			{
				global::System.Reflection.ConstructorInfo constructor2 = typeof(R).GetConstructor(new global::System.Type[0]);
				val = (R)constructor2.Invoke(new object[0]);
				global::System.Runtime.InteropServices.Marshal.PtrToStructure(rawObject, val);
			}
			OnResult(val);
		}
	}
}
