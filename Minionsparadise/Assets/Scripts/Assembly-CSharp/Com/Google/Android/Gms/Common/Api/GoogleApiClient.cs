namespace Com.Google.Android.Gms.Common.Api
{
	public class GoogleApiClient : global::Google.Developers.JavaObjWrapper
	{
		private const string CLASS_NAME = "com/google/android/gms/common/api/GoogleApiClient";

		public GoogleApiClient(global::System.IntPtr ptr)
			: base(ptr)
		{
		}

		public GoogleApiClient()
			: base("com.google.android.gms.common.api.GoogleApiClient")
		{
		}

		public object getContext()
		{
			return InvokeCall<object>("getContext", "()Landroid/content/Context;", new object[0]);
		}

		public void connect()
		{
			InvokeCallVoid("connect", "()V");
		}

		public void disconnect()
		{
			InvokeCallVoid("disconnect", "()V");
		}

		public void dump(string arg_string_1, object arg_object_2, object arg_object_3, string[] arg_string_4)
		{
			InvokeCallVoid("dump", "(Ljava/lang/String;Ljava/io/FileDescriptor;Ljava/io/PrintWriter;[Ljava/lang/String;)V", arg_string_1, arg_object_2, arg_object_3, arg_string_4);
		}

		public global::Com.Google.Android.Gms.Common.ConnectionResult blockingConnect(long arg_long_1, object arg_object_2)
		{
			return InvokeCall<global::Com.Google.Android.Gms.Common.ConnectionResult>("blockingConnect", "(JLjava/util/concurrent/TimeUnit;)Lcom/google/android/gms/common/ConnectionResult;", new object[2] { arg_long_1, arg_object_2 });
		}

		public global::Com.Google.Android.Gms.Common.ConnectionResult blockingConnect()
		{
			return InvokeCall<global::Com.Google.Android.Gms.Common.ConnectionResult>("blockingConnect", "()Lcom/google/android/gms/common/ConnectionResult;", new object[0]);
		}

		public global::Com.Google.Android.Gms.Common.Api.PendingResult<global::Com.Google.Android.Gms.Common.Api.Status> clearDefaultAccountAndReconnect()
		{
			return InvokeCall<global::Com.Google.Android.Gms.Common.Api.PendingResult<global::Com.Google.Android.Gms.Common.Api.Status>>("clearDefaultAccountAndReconnect", "()Lcom/google/android/gms/common/api/PendingResult;", new object[0]);
		}

		public global::Com.Google.Android.Gms.Common.ConnectionResult getConnectionResult(object arg_object_1)
		{
			return InvokeCall<global::Com.Google.Android.Gms.Common.ConnectionResult>("getConnectionResult", "(Lcom/google/android/gms/common/api/Api;)Lcom/google/android/gms/common/ConnectionResult;", new object[1] { arg_object_1 });
		}

		public int getSessionId()
		{
			return InvokeCall<int>("getSessionId", "()I", new object[0]);
		}

		public bool isConnecting()
		{
			return InvokeCall<bool>("isConnecting", "()Z", new object[0]);
		}

		public bool isConnectionCallbacksRegistered(object arg_object_1)
		{
			return InvokeCall<bool>("isConnectionCallbacksRegistered", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)Z", new object[1] { arg_object_1 });
		}

		public bool isConnectionFailedListenerRegistered(object arg_object_1)
		{
			return InvokeCall<bool>("isConnectionFailedListenerRegistered", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)Z", new object[1] { arg_object_1 });
		}

		public void reconnect()
		{
			InvokeCallVoid("reconnect", "()V");
		}

		public void registerConnectionCallbacks(object arg_object_1)
		{
			InvokeCallVoid("registerConnectionCallbacks", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)V", arg_object_1);
		}

		public void registerConnectionFailedListener(object arg_object_1)
		{
			InvokeCallVoid("registerConnectionFailedListener", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)V", arg_object_1);
		}

		public void stopAutoManage(object arg_object_1)
		{
			InvokeCallVoid("stopAutoManage", "(Landroid/support/v4/app/FragmentActivity;)V", arg_object_1);
		}

		public void unregisterConnectionCallbacks(object arg_object_1)
		{
			InvokeCallVoid("unregisterConnectionCallbacks", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)V", arg_object_1);
		}

		public void unregisterConnectionFailedListener(object arg_object_1)
		{
			InvokeCallVoid("unregisterConnectionFailedListener", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)V", arg_object_1);
		}

		public bool hasConnectedApi(object arg_object_1)
		{
			return InvokeCall<bool>("hasConnectedApi", "(Lcom/google/android/gms/common/api/Api;)Z", new object[1] { arg_object_1 });
		}

		public object getLooper()
		{
			return InvokeCall<object>("getLooper", "()Landroid/os/Looper;", new object[0]);
		}

		public bool isConnected()
		{
			return InvokeCall<bool>("isConnected", "()Z", new object[0]);
		}
	}
}
