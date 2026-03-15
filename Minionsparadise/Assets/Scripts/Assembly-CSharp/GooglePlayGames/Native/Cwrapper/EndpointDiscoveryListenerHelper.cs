namespace GooglePlayGames.Native.Cwrapper
{
	internal static class EndpointDiscoveryListenerHelper
	{
		internal delegate void OnEndpointFoundCallback(long arg0, global::System.IntPtr arg1, global::System.IntPtr arg2);

		internal delegate void OnEndpointLostCallback(long arg0, string arg1, global::System.IntPtr arg2);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr EndpointDiscoveryListenerHelper_Construct();

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void EndpointDiscoveryListenerHelper_SetOnEndpointLostCallback(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.EndpointDiscoveryListenerHelper.OnEndpointLostCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void EndpointDiscoveryListenerHelper_SetOnEndpointFoundCallback(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.EndpointDiscoveryListenerHelper.OnEndpointFoundCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void EndpointDiscoveryListenerHelper_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
