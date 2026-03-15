namespace GooglePlayGames.Native.Cwrapper
{
	internal static class LeaderboardManager
	{
		internal delegate void FetchCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void FetchAllCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void FetchScorePageCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void FetchScoreSummaryCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void FetchAllScoreSummariesCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void ShowAllUICallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus arg0, global::System.IntPtr arg1);

		internal delegate void ShowUICallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchAll(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchAllCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchScoreSummary(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, string leaderboard_id, global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardTimeSpan time_span, global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardCollection collection, global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScoreSummaryCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr LeaderboardManager_ScorePageToken(global::System.Runtime.InteropServices.HandleRef self, string leaderboard_id, global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardStart start, global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardTimeSpan time_span, global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardCollection collection);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_ShowAllUI(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.ShowAllUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchScorePage(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, global::System.IntPtr token, uint max_results, global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchScorePageCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchAllScoreSummaries(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, string leaderboard_id, global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchAllScoreSummariesCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_ShowUI(global::System.Runtime.InteropServices.HandleRef self, string leaderboard_id, global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardTimeSpan time_span, global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.ShowUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_Fetch(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, string leaderboard_id, global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.FetchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_SubmitScore(global::System.Runtime.InteropServices.HandleRef self, string leaderboard_id, ulong score, string metadata);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus LeaderboardManager_FetchResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr LeaderboardManager_FetchResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchAllResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus LeaderboardManager_FetchAllResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr LeaderboardManager_FetchAllResponse_GetData_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr LeaderboardManager_FetchAllResponse_GetData_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchScorePageResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus LeaderboardManager_FetchScorePageResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr LeaderboardManager_FetchScorePageResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchScoreSummaryResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus LeaderboardManager_FetchScoreSummaryResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr LeaderboardManager_FetchScoreSummaryResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void LeaderboardManager_FetchAllScoreSummariesResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus LeaderboardManager_FetchAllScoreSummariesResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr LeaderboardManager_FetchAllScoreSummariesResponse_GetData_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr LeaderboardManager_FetchAllScoreSummariesResponse_GetData_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);
	}
}
