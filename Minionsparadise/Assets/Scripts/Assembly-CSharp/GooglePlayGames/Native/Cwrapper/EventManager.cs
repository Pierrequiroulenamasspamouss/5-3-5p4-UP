namespace GooglePlayGames.Native.Cwrapper
{
	internal static class EventManager
	{
		internal delegate void FetchAllCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void FetchCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void EventManager_FetchAll(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, global::GooglePlayGames.Native.Cwrapper.EventManager.FetchAllCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void EventManager_Fetch(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, string event_id, global::GooglePlayGames.Native.Cwrapper.EventManager.FetchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void EventManager_Increment(global::System.Runtime.InteropServices.HandleRef self, string event_id, uint steps);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void EventManager_FetchAllResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus EventManager_FetchAllResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr EventManager_FetchAllResponse_GetData(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr[] out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void EventManager_FetchResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus EventManager_FetchResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr EventManager_FetchResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);
	}
}
