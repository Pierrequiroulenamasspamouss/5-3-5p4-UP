namespace GooglePlayGames.Native.Cwrapper
{
	internal static class NearbyConnections
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_StartDiscovery(global::System.Runtime.InteropServices.HandleRef self, string service_id, long duration, global::System.IntPtr helper);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_RejectConnectionRequest(global::System.Runtime.InteropServices.HandleRef self, string remote_endpoint_id);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_Disconnect(global::System.Runtime.InteropServices.HandleRef self, string remote_endpoint_id);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_SendUnreliableMessage(global::System.Runtime.InteropServices.HandleRef self, string remote_endpoint_id, byte[] payload, global::System.UIntPtr payload_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr NearbyConnections_GetLocalDeviceId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_StopAdvertising(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr NearbyConnections_GetLocalEndpointId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_SendReliableMessage(global::System.Runtime.InteropServices.HandleRef self, string remote_endpoint_id, byte[] payload, global::System.UIntPtr payload_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_StopDiscovery(global::System.Runtime.InteropServices.HandleRef self, string service_id);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_SendConnectionRequest(global::System.Runtime.InteropServices.HandleRef self, string name, string remote_endpoint_id, byte[] payload, global::System.UIntPtr payload_size, global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionResponseCallback callback, global::System.IntPtr callback_arg, global::System.IntPtr helper);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_StartAdvertising(global::System.Runtime.InteropServices.HandleRef self, string name, global::System.IntPtr[] app_identifiers, global::System.UIntPtr app_identifiers_size, long duration, global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.StartAdvertisingCallback start_advertising_callback, global::System.IntPtr start_advertising_callback_arg, global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.ConnectionRequestCallback request_callback, global::System.IntPtr request_callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_Stop(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_AcceptConnectionRequest(global::System.Runtime.InteropServices.HandleRef self, string remote_endpoint_id, byte[] payload, global::System.UIntPtr payload_size, global::System.IntPtr helper);
	}
}
