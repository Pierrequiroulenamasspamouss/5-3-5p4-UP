namespace GooglePlayGames.Native.Cwrapper
{
	internal static class RealTimeRoomConfig
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr RealTimeRoomConfig_PlayerIdsToInvite_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr RealTimeRoomConfig_PlayerIdsToInvite_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint RealTimeRoomConfig_Variant(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern long RealTimeRoomConfig_ExclusiveBitMask(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool RealTimeRoomConfig_Valid(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint RealTimeRoomConfig_MaximumAutomatchingPlayers(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint RealTimeRoomConfig_MinimumAutomatchingPlayers(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeRoomConfig_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
