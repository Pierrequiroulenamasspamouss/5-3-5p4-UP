namespace GooglePlayGames.Native.Cwrapper
{
	internal static class StatsManager
	{
		internal delegate void FetchForPlayerCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void StatsManager_FetchForPlayer(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, global::GooglePlayGames.Native.Cwrapper.StatsManager.FetchForPlayerCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void StatsManager_FetchForPlayerResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus StatsManager_FetchForPlayerResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr StatsManager_FetchForPlayerResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);
	}
}
