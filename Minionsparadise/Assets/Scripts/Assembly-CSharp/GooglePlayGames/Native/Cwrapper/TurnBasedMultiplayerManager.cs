namespace GooglePlayGames.Native.Cwrapper
{
	internal static class TurnBasedMultiplayerManager
	{
		internal delegate void TurnBasedMatchCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void MultiplayerStatusCallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus arg0, global::System.IntPtr arg1);

		internal delegate void TurnBasedMatchesCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void MatchInboxUICallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void PlayerSelectUICallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_ShowPlayerSelectUI(global::System.Runtime.InteropServices.HandleRef self, uint minimum_players, uint maximum_players, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool allow_automatch, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.PlayerSelectUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_CancelMatch(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr match, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.MultiplayerStatusCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_DismissMatch(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr match);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_ShowMatchInboxUI(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.MatchInboxUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_SynchronizeData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_Rematch(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr match, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_DismissInvitation(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr invitation);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_FetchMatch(global::System.Runtime.InteropServices.HandleRef self, string match_id, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_DeclineInvitation(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr invitation);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_FinishMatchDuringMyTurn(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr match, byte[] match_data, global::System.UIntPtr match_data_size, global::System.IntPtr results, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_FetchMatches(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMatchesCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_CreateTurnBasedMatch(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr config, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_AcceptInvitation(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr invitation, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_TakeMyTurn(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr match, byte[] match_data, global::System.UIntPtr match_data_size, global::System.IntPtr results, global::System.IntPtr next_participant, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_ConfirmPendingCompletion(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr match, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.TurnBasedMatchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_LeaveMatchDuringMyTurn(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr match, global::System.IntPtr next_participant, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.MultiplayerStatusCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_LeaveMatchDuringTheirTurn(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr match, global::GooglePlayGames.Native.Cwrapper.TurnBasedMultiplayerManager.MultiplayerStatusCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_TurnBasedMatchResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus TurnBasedMultiplayerManager_TurnBasedMatchResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMultiplayerManager_TurnBasedMatchResponse_GetMatch(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_TurnBasedMatchesResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetInvitations_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetMyTurnMatches_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetTheirTurnMatches_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMultiplayerManager_TurnBasedMatchesResponse_GetCompletedMatches_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_MatchInboxUIResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus TurnBasedMultiplayerManager_MatchInboxUIResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr TurnBasedMultiplayerManager_MatchInboxUIResponse_GetMatch(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void TurnBasedMultiplayerManager_PlayerSelectUIResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMinimumAutomatchingPlayers(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern uint TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMaximumAutomatchingPlayers(global::System.Runtime.InteropServices.HandleRef self);
	}
}
