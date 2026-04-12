namespace GooglePlayGames.Native.Cwrapper
{
	internal static class GameServices
	{
		internal delegate void FlushCallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.FlushStatus arg0, global::System.IntPtr arg1);

		internal delegate void FetchServerAuthCodeCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Flush(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.GameServices.FlushCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_FetchServerAuthCode(global::System.Runtime.InteropServices.HandleRef self, string server_client_id, global::GooglePlayGames.Native.Cwrapper.GameServices.FetchServerAuthCodeCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool GameServices_IsAuthorized(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_SignOut(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_StartAuthorizationUI(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_FetchServerAuthCodeResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus GameServices_FetchServerAuthCodeResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr GameServices_FetchServerAuthCodeResponse_GetCode(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);
	}
}
