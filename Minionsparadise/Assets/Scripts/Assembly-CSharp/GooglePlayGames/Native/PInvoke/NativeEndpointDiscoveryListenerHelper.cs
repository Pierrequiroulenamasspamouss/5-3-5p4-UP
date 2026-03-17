namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeEndpointDiscoveryListenerHelper : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeEndpointDiscoveryListenerHelper()
			: base(global::GooglePlayGames.Native.Cwrapper.EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_Construct())
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_Dispose(selfPointer);
		}

		internal void SetOnEndpointFound(global::System.Action<long, global::GooglePlayGames.Native.PInvoke.NativeEndpointDetails> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_SetOnEndpointFoundCallback(SelfPtr(), InternalOnEndpointFoundCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.NativeEndpointDetails.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.EndpointDiscoveryListenerHelper.OnEndpointFoundCallback))]
		private static void InternalOnEndpointFoundCallback(long id, global::System.IntPtr data, global::System.IntPtr userData)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("NativeEndpointDiscoveryListenerHelper#InternalOnEndpointFoundCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Permanent, id, data, userData);
		}

		internal void SetOnEndpointLostCallback(global::System.Action<long, string> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_SetOnEndpointLostCallback(SelfPtr(), InternalOnEndpointLostCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.EndpointDiscoveryListenerHelper.OnEndpointLostCallback))]
		private static void InternalOnEndpointLostCallback(long id, string lostEndpointId, global::System.IntPtr userData)
		{
			global::System.Action<long, string> action = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToPermanentCallback<global::System.Action<long, string>>(userData);
			if (action == null)
			{
				return;
			}
			try
			{
				action(id, lostEndpointId);
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing NativeEndpointDiscoveryListenerHelper#InternalOnEndpointLostCallback. Smothering to avoid passing exception into Native: " + ex);
			}
		}
	}
}
