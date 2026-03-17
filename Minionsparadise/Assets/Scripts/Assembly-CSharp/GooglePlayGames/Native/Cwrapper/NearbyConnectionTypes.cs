namespace GooglePlayGames.Native.Cwrapper
{
	internal static class NearbyConnectionTypes
	{
		internal enum ConnectionResponse_ResponseCode
		{
			ACCEPTED = 1,
			REJECTED = 2,
			ERROR_INTERNAL = -1,
			ERROR_NETWORK_NOT_CONNECTED = -2,
			ERROR_ENDPOINT_ALREADY_CONNECTED = -3,
			ERROR_ENDPOINT_NOT_CONNECTED = -4
		}

		internal delegate void ConnectionRequestCallback(long arg0, global::System.IntPtr arg1, global::System.IntPtr arg2);

		internal delegate void StartAdvertisingCallback(long arg0, global::System.IntPtr arg1, global::System.IntPtr arg2);

		internal delegate void ConnectionResponseCallback(long arg0, global::System.IntPtr arg1, global::System.IntPtr arg2);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AppIdentifier_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr AppIdentifier_GetIdentifier(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void StartAdvertisingResult_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I4)]
		internal static extern int StartAdvertisingResult_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr StartAdvertisingResult_GetLocalEndpointName(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void EndpointDetails_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr EndpointDetails_GetEndpointId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr EndpointDetails_GetDeviceId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr EndpointDetails_GetName(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr EndpointDetails_GetServiceId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void ConnectionRequest_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr ConnectionRequest_GetRemoteEndpointId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr ConnectionRequest_GetRemoteDeviceId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr ConnectionRequest_GetRemoteEndpointName(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr ConnectionRequest_GetPayload(global::System.Runtime.InteropServices.HandleRef self, [global::System.Runtime.InteropServices.In][global::System.Runtime.InteropServices.Out] byte[] out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void ConnectionResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr ConnectionResponse_GetRemoteEndpointId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponse_ResponseCode ConnectionResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr ConnectionResponse_GetPayload(global::System.Runtime.InteropServices.HandleRef self, [global::System.Runtime.InteropServices.In][global::System.Runtime.InteropServices.Out] byte[] out_arg, global::System.UIntPtr out_size);
	}
}
