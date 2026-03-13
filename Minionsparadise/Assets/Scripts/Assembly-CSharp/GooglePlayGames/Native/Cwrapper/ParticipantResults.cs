namespace GooglePlayGames.Native.Cwrapper
{
	internal static class ParticipantResults
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr ParticipantResults_WithResult(global::System.Runtime.InteropServices.HandleRef self, string participant_id, uint placing, global::GooglePlayGames.Native.Cwrapper.Types.MatchResult result);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool ParticipantResults_Valid(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.Types.MatchResult ParticipantResults_MatchResultForParticipant(global::System.Runtime.InteropServices.HandleRef self, string participant_id);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint ParticipantResults_PlaceForParticipant(global::System.Runtime.InteropServices.HandleRef self, string participant_id);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool ParticipantResults_HasResultsForParticipant(global::System.Runtime.InteropServices.HandleRef self, string participant_id);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void ParticipantResults_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
