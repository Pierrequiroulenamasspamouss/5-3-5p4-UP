namespace GooglePlayGames.BasicApi.Multiplayer
{
	public interface ITurnBasedMultiplayerClient
	{
		void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback);

		void CreateQuickMatch(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitmask, global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback);

		void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback);

		void CreateWithInvitationScreen(uint minOpponents, uint maxOpponents, uint variant, global::System.Action<global::GooglePlayGames.BasicApi.UIStatus, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback);

		void GetAllInvitations(global::System.Action<global::GooglePlayGames.BasicApi.Multiplayer.Invitation[]> callback);

		void GetAllMatches(global::System.Action<global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch[]> callback);

		void AcceptFromInbox(global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback);

		void AcceptInvitation(string invitationId, global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback);

		void RegisterMatchDelegate(global::GooglePlayGames.BasicApi.Multiplayer.MatchDelegate del);

		void TakeTurn(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, string pendingParticipantId, global::System.Action<bool> callback);

		int GetMaxMatchDataSize();

		void Finish(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, byte[] data, global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome outcome, global::System.Action<bool> callback);

		void AcknowledgeFinished(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, global::System.Action<bool> callback);

		void Leave(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, global::System.Action<bool> callback);

		void LeaveDuringTurn(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, string pendingParticipantId, global::System.Action<bool> callback);

		void Cancel(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, global::System.Action<bool> callback);

		void Rematch(global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, global::System.Action<bool, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch> callback);

		void DeclineInvitation(string invitationId);
	}
}
