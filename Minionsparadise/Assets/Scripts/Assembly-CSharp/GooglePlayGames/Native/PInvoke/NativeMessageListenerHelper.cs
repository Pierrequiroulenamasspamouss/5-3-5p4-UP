namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeMessageListenerHelper : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal delegate void OnMessageReceived(long localClientId, string remoteEndpointId, byte[] data, bool isReliable);

		internal NativeMessageListenerHelper()
			: base(global::GooglePlayGames.Native.Cwrapper.MessageListenerHelper.MessageListenerHelper_Construct())
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.MessageListenerHelper.MessageListenerHelper_Dispose(selfPointer);
		}

		internal void SetOnMessageReceivedCallback(global::GooglePlayGames.Native.PInvoke.NativeMessageListenerHelper.OnMessageReceived callback)
		{
			global::GooglePlayGames.Native.Cwrapper.MessageListenerHelper.MessageListenerHelper_SetOnMessageReceivedCallback(SelfPtr(), InternalOnMessageReceivedCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.MessageListenerHelper.OnMessageReceivedCallback))]
		private static void InternalOnMessageReceivedCallback(long id, string name, global::System.IntPtr data, global::System.UIntPtr dataLength, bool isReliable, global::System.IntPtr userData)
		{
			global::GooglePlayGames.Native.PInvoke.NativeMessageListenerHelper.OnMessageReceived onMessageReceived = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToPermanentCallback<global::GooglePlayGames.Native.PInvoke.NativeMessageListenerHelper.OnMessageReceived>(userData);
			if (onMessageReceived == null)
			{
				return;
			}
			try
			{
				onMessageReceived(id, name, global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrAndSizeToByteArray(data, dataLength), isReliable);
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing NativeMessageListenerHelper#InternalOnMessageReceivedCallback. Smothering to avoid passing exception into Native: " + ex);
			}
		}

		internal void SetOnDisconnectedCallback(global::System.Action<long, string> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.MessageListenerHelper.MessageListenerHelper_SetOnDisconnectedCallback(SelfPtr(), InternalOnDisconnectedCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.MessageListenerHelper.OnDisconnectedCallback))]
		private static void InternalOnDisconnectedCallback(long id, string lostEndpointId, global::System.IntPtr userData)
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
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing NativeMessageListenerHelper#InternalOnDisconnectedCallback. Smothering to avoid passing exception into Native: " + ex);
			}
		}
	}
}
