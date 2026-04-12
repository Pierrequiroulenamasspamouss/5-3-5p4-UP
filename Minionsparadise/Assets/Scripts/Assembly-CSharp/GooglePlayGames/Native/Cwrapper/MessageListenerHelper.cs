namespace GooglePlayGames.Native.Cwrapper
{
	internal static class MessageListenerHelper
	{
		internal delegate void OnMessageReceivedCallback(long arg0, string arg1, global::System.IntPtr arg2, global::System.UIntPtr arg3, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool arg4, global::System.IntPtr arg5);

		internal delegate void OnDisconnectedCallback(long arg0, string arg1, global::System.IntPtr arg2);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void MessageListenerHelper_SetOnMessageReceivedCallback(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.MessageListenerHelper.OnMessageReceivedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void MessageListenerHelper_SetOnDisconnectedCallback(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.MessageListenerHelper.OnDisconnectedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr MessageListenerHelper_Construct();

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void MessageListenerHelper_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
