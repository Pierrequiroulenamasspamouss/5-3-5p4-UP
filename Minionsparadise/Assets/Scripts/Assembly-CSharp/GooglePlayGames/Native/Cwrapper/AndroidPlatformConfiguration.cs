namespace GooglePlayGames.Native.Cwrapper
{
	internal static class AndroidPlatformConfiguration
	{
		internal delegate void IntentHandler(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void OnLaunchedWithSnapshotCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void OnLaunchedWithQuestCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AndroidPlatformConfiguration_SetOnLaunchedWithSnapshot(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.OnLaunchedWithSnapshotCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr AndroidPlatformConfiguration_Construct();

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.IntentHandler intent_handler, global::System.IntPtr intent_handler_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AndroidPlatformConfiguration_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool AndroidPlatformConfiguration_Valid(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AndroidPlatformConfiguration_SetActivity(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr android_app_activity);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AndroidPlatformConfiguration_SetOnLaunchedWithQuest(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.OnLaunchedWithQuestCallback callback, global::System.IntPtr callback_arg);
	}
}
