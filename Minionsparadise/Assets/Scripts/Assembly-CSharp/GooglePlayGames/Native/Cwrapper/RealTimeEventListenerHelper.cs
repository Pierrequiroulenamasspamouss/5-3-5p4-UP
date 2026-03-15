namespace GooglePlayGames.Native.Cwrapper
{
	internal static class RealTimeEventListenerHelper
	{
		internal delegate void OnRoomStatusChangedCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void OnRoomConnectedSetChangedCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void OnP2PConnectedCallback(global::System.IntPtr arg0, global::System.IntPtr arg1, global::System.IntPtr arg2);

		internal delegate void OnP2PDisconnectedCallback(global::System.IntPtr arg0, global::System.IntPtr arg1, global::System.IntPtr arg2);

		internal delegate void OnParticipantStatusChangedCallback(global::System.IntPtr arg0, global::System.IntPtr arg1, global::System.IntPtr arg2);

		internal delegate void OnDataReceivedCallback(global::System.IntPtr arg0, global::System.IntPtr arg1, global::System.IntPtr arg2, global::System.UIntPtr arg3, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool arg4, global::System.IntPtr arg5);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnParticipantStatusChangedCallback(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnParticipantStatusChangedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr RealTimeEventListenerHelper_Construct();

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnP2PDisconnectedCallback(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnP2PDisconnectedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnDataReceivedCallback(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnDataReceivedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnRoomStatusChangedCallback(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnRoomStatusChangedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnP2PConnectedCallback(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnP2PConnectedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_SetOnRoomConnectedSetChangedCallback(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeEventListenerHelper_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
