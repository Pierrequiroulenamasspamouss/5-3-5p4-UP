namespace GooglePlayGames.Native.Cwrapper
{
	internal static class PlayerManager
	{
		internal delegate void FetchSelfCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void FetchCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void FetchListCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void PlayerManager_FetchInvitable(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, global::GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void PlayerManager_FetchConnected(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, global::GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void PlayerManager_Fetch(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, string player_id, global::GooglePlayGames.Native.Cwrapper.PlayerManager.FetchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void PlayerManager_FetchRecentlyPlayed(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, global::GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void PlayerManager_FetchSelf(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, global::GooglePlayGames.Native.Cwrapper.PlayerManager.FetchSelfCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void PlayerManager_FetchSelfResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus PlayerManager_FetchSelfResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr PlayerManager_FetchSelfResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void PlayerManager_FetchResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus PlayerManager_FetchResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr PlayerManager_FetchResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void PlayerManager_FetchListResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus PlayerManager_FetchListResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr PlayerManager_FetchListResponse_GetData_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr PlayerManager_FetchListResponse_GetData_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);
	}
}
