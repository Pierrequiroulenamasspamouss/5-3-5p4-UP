namespace GooglePlayGames.Native.Cwrapper
{
	internal static class TurnBasedMatchConfig
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMatchConfig_PlayerIdsToInvite_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMatchConfig_PlayerIdsToInvite_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint TurnBasedMatchConfig_Variant(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern long TurnBasedMatchConfig_ExclusiveBitMask(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool TurnBasedMatchConfig_Valid(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint TurnBasedMatchConfig_MaximumAutomatchingPlayers(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint TurnBasedMatchConfig_MinimumAutomatchingPlayers(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
