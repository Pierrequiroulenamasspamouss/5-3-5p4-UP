namespace GooglePlayGames.Native.Cwrapper
{
	internal static class InternalHooks
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void InternalHooks_ConfigureForUnityPlugin(global::System.Runtime.InteropServices.HandleRef builder);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr InternalHooks_GetApiClient(global::System.Runtime.InteropServices.HandleRef services);
	}
}
