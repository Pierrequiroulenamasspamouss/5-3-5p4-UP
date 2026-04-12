namespace GooglePlayGames.Native.Cwrapper
{
	internal static class TurnBasedMatch
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint TurnBasedMatch_AutomatchingSlotsAvailable(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern ulong TurnBasedMatch_CreationTime(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMatch_Participants_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMatch_Participants_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint TurnBasedMatch_Version(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMatch_ParticipantResults(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus TurnBasedMatch_Status(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMatch_Description(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMatch_PendingParticipant(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint TurnBasedMatch_Variant(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool TurnBasedMatch_HasPreviousMatchData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMatch_Data(global::System.Runtime.InteropServices.HandleRef self, [global::System.Runtime.InteropServices.In][global::System.Runtime.InteropServices.Out] byte[] out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMatch_LastUpdatingParticipant(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool TurnBasedMatch_HasData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMatch_SuggestedNextParticipant(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMatch_PreviousMatchData(global::System.Runtime.InteropServices.HandleRef self, [global::System.Runtime.InteropServices.In][global::System.Runtime.InteropServices.Out] byte[] out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern ulong TurnBasedMatch_LastUpdateTime(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMatch_RematchId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint TurnBasedMatch_Number(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool TurnBasedMatch_HasRematchId(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool TurnBasedMatch_Valid(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMatch_CreatingParticipant(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMatch_Id(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMatch_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
