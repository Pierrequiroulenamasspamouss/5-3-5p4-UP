namespace GooglePlayGames.Native.Cwrapper
{
	internal static class NearbyConnectionsBuilder
	{
		internal delegate void OnInitializationFinishedCallback(global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus arg0, global::System.IntPtr arg1);

		internal delegate void OnLogCallback(global::GooglePlayGames.Native.Cwrapper.Types.LogLevel arg0, string arg1, global::System.IntPtr arg2);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_Builder_SetOnInitializationFinished(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsBuilder.OnInitializationFinishedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr NearbyConnections_Builder_Construct();

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_Builder_SetClientId(global::System.Runtime.InteropServices.HandleRef self, long client_id);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_Builder_SetOnLog(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsBuilder.OnLogCallback callback, global::System.IntPtr callback_arg, global::GooglePlayGames.Native.Cwrapper.Types.LogLevel min_level);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_Builder_SetDefaultOnLog(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.LogLevel min_level);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr NearbyConnections_Builder_Create(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr platform);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void NearbyConnections_Builder_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
