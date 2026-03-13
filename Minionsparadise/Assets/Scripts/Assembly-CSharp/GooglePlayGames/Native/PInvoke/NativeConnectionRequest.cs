namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeConnectionRequest : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeConnectionRequest(global::System.IntPtr pointer)
			: base(pointer)
		{
		}

		internal string RemoteEndpointId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointId(SelfPtr(), out_arg, out_size));
		}

		internal string RemoteDeviceId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionRequest_GetRemoteDeviceId(SelfPtr(), out_arg, out_size));
		}

		internal string RemoteEndpointName()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointName(SelfPtr(), out_arg, out_size));
		}

		internal byte[] Payload()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToArray((byte[] out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionRequest_GetPayload(SelfPtr(), out_arg, out_size));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionRequest_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.BasicApi.Nearby.ConnectionRequest AsRequest()
		{
			return new global::GooglePlayGames.BasicApi.Nearby.ConnectionRequest(RemoteEndpointId(), RemoteDeviceId(), RemoteEndpointName(), global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManager.ServiceId, Payload());
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeConnectionRequest FromPointer(global::System.IntPtr pointer)
		{
			if (pointer == global::System.IntPtr.Zero)
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeConnectionRequest(pointer);
		}
	}
}
