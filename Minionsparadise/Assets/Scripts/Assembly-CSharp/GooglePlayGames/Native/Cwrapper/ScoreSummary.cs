namespace GooglePlayGames.Native.Cwrapper
{
	internal static class ScoreSummary
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern ulong ScoreSummary_ApproximateNumberOfScores(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardTimeSpan ScoreSummary_TimeSpan(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr ScoreSummary_LeaderboardId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardCollection ScoreSummary_Collection(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool ScoreSummary_Valid(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr ScoreSummary_CurrentPlayerScore(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void ScoreSummary_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
