namespace GooglePlayGames.Android
{
	internal class TokenResult : global::Google.Developers.JavaObjWrapper, global::Com.Google.Android.Gms.Common.Api.Result
	{
		public TokenResult(global::System.IntPtr ptr)
			: base(ptr)
		{
		}

		public global::Com.Google.Android.Gms.Common.Api.Status getStatus()
		{
			global::System.IntPtr ptr = InvokeCall<global::System.IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", new object[0]);
			return new global::Com.Google.Android.Gms.Common.Api.Status(ptr);
		}

		public string getAccessToken()
		{
			return InvokeCall<string>("getAccessToken", "()Ljava/lang/String;", new object[0]);
		}

		public string getEmail()
		{
			return InvokeCall<string>("getEmail", "()Ljava/lang/String;", new object[0]);
		}

		public string getIdToken()
		{
			return InvokeCall<string>("getIdToken", "()Ljava/lang/String;", new object[0]);
		}
	}
}
