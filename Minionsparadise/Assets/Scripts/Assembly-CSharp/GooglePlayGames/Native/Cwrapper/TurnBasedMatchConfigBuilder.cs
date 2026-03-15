namespace GooglePlayGames.Native.Cwrapper
{
	internal static class TurnBasedMatchConfigBuilder
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_PopulateFromPlayerSelectUIResponse(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr response);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_SetVariant(global::System.Runtime.InteropServices.HandleRef self, uint variant);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_AddPlayerToInvite(global::System.Runtime.InteropServices.HandleRef self, string player_id);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMatchConfig_Builder_Construct();

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_SetExclusiveBitMask(global::System.Runtime.InteropServices.HandleRef self, ulong exclusive_bit_mask);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_SetMaximumAutomatchingPlayers(global::System.Runtime.InteropServices.HandleRef self, uint maximum_automatching_players);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMatchConfig_Builder_Create(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_SetMinimumAutomatchingPlayers(global::System.Runtime.InteropServices.HandleRef self, uint minimum_automatching_players);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMatchConfig_Builder_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
