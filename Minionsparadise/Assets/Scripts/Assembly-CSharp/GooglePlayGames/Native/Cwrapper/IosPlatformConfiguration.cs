namespace GooglePlayGames.Native.Cwrapper
{
	internal static class IosPlatformConfiguration
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr IosPlatformConfiguration_Construct();

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void IosPlatformConfiguration_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool IosPlatformConfiguration_Valid(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void IosPlatformConfiguration_SetClientID(global::System.Runtime.InteropServices.HandleRef self, string client_id);
	}
}
