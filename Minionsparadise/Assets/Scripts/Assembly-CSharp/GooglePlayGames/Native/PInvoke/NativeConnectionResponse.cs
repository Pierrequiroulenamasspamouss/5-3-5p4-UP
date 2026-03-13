namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeConnectionResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeConnectionResponse(global::System.IntPtr pointer)
			: base(pointer)
		{
		}

		internal string RemoteEndpointId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_GetRemoteEndpointId(SelfPtr(), out_arg, out_size));
		}

		internal global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_ResponseCode ResponseCode()
		{
			return global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_GetStatus(SelfPtr());
		}

		internal byte[] Payload()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToArray((byte[] out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_GetPayload(SelfPtr(), out_arg, out_size));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse AsResponse(long localClientId)
		{
			switch (ResponseCode())
			{
			case global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_ResponseCode.ACCEPTED:
				return global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Accepted(localClientId, RemoteEndpointId(), Payload());
			case global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_ResponseCode.ERROR_ENDPOINT_ALREADY_CONNECTED:
				return global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.AlreadyConnected(localClientId, RemoteEndpointId());
			case global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_ResponseCode.REJECTED:
				return global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.Rejected(localClientId, RemoteEndpointId());
			case global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_ResponseCode.ERROR_ENDPOINT_NOT_CONNECTED:
				return global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.EndpointNotConnected(localClientId, RemoteEndpointId());
			case global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_ResponseCode.ERROR_NETWORK_NOT_CONNECTED:
				return global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.NetworkNotConnected(localClientId, RemoteEndpointId());
			case global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_ResponseCode.ERROR_INTERNAL:
				return global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse.InternalError(localClientId, RemoteEndpointId());
			default:
				throw new global::System.InvalidOperationException("Found connection response of unknown type: " + ResponseCode());
			}
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeConnectionResponse FromPointer(global::System.IntPtr pointer)
		{
			if (pointer == global::System.IntPtr.Zero)
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeConnectionResponse(pointer);
		}
	}
}
