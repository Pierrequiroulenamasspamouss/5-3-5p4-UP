namespace GooglePlayGames.Native.PInvoke
{
	internal class NearbyConnectionsManager : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		private static readonly string sServiceId = ReadServiceId();

		public string AppBundleId
		{
			get
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					global::UnityEngine.AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<global::UnityEngine.AndroidJavaObject>("currentActivity");
					return androidJavaObject.Call<string>("getPackageName", new object[0]);
				}
			}
		}

		public static string ServiceId
		{
			get
			{
				return sServiceId;
			}
		}

		internal NearbyConnectionsManager(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_Dispose(selfPointer);
		}

		internal void SendUnreliable(string remoteEndpointId, byte[] payload)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_SendUnreliableMessage(SelfPtr(), remoteEndpointId, payload, new global::System.UIntPtr((ulong)payload.Length));
		}

		internal void SendReliable(string remoteEndpointId, byte[] payload)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_SendReliableMessage(SelfPtr(), remoteEndpointId, payload, new global::System.UIntPtr((ulong)payload.Length));
		}

		internal void StartAdvertising(string name, global::System.Collections.Generic.List<global::GooglePlayGames.Native.PInvoke.NativeAppIdentifier> appIds, long advertisingDuration, global::System.Action<long, global::GooglePlayGames.Native.PInvoke.NativeStartAdvertisingResult> advertisingCallback, global::System.Action<long, global::GooglePlayGames.Native.PInvoke.NativeConnectionRequest> connectionRequestCallback)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_StartAdvertising(SelfPtr(), name, global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(appIds, (global::GooglePlayGames.Native.PInvoke.NativeAppIdentifier id) => id.AsPointer())), new global::System.UIntPtr((ulong)appIds.Count), advertisingDuration, InternalStartAdvertisingCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(advertisingCallback, global::GooglePlayGames.Native.PInvoke.NativeStartAdvertisingResult.FromPointer), InternalConnectionRequestCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(connectionRequestCallback, global::GooglePlayGames.Native.PInvoke.NativeConnectionRequest.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.StartAdvertisingCallback))]
		private static void InternalStartAdvertisingCallback(long id, global::System.IntPtr result, global::System.IntPtr userData)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("NearbyConnectionsManager#InternalStartAdvertisingCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Permanent, id, result, userData);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionRequestCallback))]
		private static void InternalConnectionRequestCallback(long id, global::System.IntPtr result, global::System.IntPtr userData)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("NearbyConnectionsManager#InternalConnectionRequestCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Permanent, id, result, userData);
		}

		internal void StopAdvertising()
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_StopAdvertising(SelfPtr());
		}

		internal void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, global::System.Action<long, global::GooglePlayGames.Native.PInvoke.NativeConnectionResponse> callback, global::GooglePlayGames.Native.PInvoke.NativeMessageListenerHelper listener)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_SendConnectionRequest(SelfPtr(), name, remoteEndpointId, payload, new global::System.UIntPtr((ulong)payload.Length), InternalConnectResponseCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.NativeConnectionResponse.FromPointer), listener.AsPointer());
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponseCallback))]
		private static void InternalConnectResponseCallback(long localClientId, global::System.IntPtr response, global::System.IntPtr userData)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("NearbyConnectionManager#InternalConnectResponseCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, localClientId, response, userData);
		}

		internal void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, global::GooglePlayGames.Native.PInvoke.NativeMessageListenerHelper listener)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_AcceptConnectionRequest(SelfPtr(), remoteEndpointId, payload, new global::System.UIntPtr((ulong)payload.Length), listener.AsPointer());
		}

		internal void DisconnectFromEndpoint(string remoteEndpointId)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_Disconnect(SelfPtr(), remoteEndpointId);
		}

		internal void StopAllConnections()
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_Stop(SelfPtr());
		}

		internal void StartDiscovery(string serviceId, long duration, global::GooglePlayGames.Native.PInvoke.NativeEndpointDiscoveryListenerHelper listener)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_StartDiscovery(SelfPtr(), serviceId, duration, listener.AsPointer());
		}

		internal void StopDiscovery(string serviceId)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_StopDiscovery(SelfPtr(), serviceId);
		}

		internal void RejectConnectionRequest(string remoteEndpointId)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_RejectConnectionRequest(SelfPtr(), remoteEndpointId);
		}

		internal void Shutdown()
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_Stop(SelfPtr());
		}

		internal string LocalEndpointId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_GetLocalEndpointId(SelfPtr(), out_arg, out_size));
		}

		internal string LocalDeviceId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnections.NearbyConnections_GetLocalDeviceId(SelfPtr(), out_arg, out_size));
		}

		internal static string ReadServiceId()
		{
			global::UnityEngine.Debug.Log("Initializing ServiceId property!!!!");
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (global::UnityEngine.AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<global::UnityEngine.AndroidJavaObject>("currentActivity"))
				{
					string text = androidJavaObject.Call<string>("getPackageName", new object[0]);
					global::UnityEngine.AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<global::UnityEngine.AndroidJavaObject>("getPackageManager", new object[0]);
					global::UnityEngine.AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<global::UnityEngine.AndroidJavaObject>("getApplicationInfo", new object[2] { text, 128 });
					global::UnityEngine.AndroidJavaObject androidJavaObject4 = androidJavaObject3.Get<global::UnityEngine.AndroidJavaObject>("metaData");
					string text2 = androidJavaObject4.Call<string>("getString", new object[1] { "com.google.android.gms.nearby.connection.SERVICE_ID" });
					global::UnityEngine.Debug.Log("SystemId from Manifest: " + text2);
					return text2;
				}
			}
		}
	}
}
