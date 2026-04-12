namespace GooglePlayGames.BasicApi.Multiplayer
{
	public interface IRealTimeMultiplayerClient
	{
		void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener);

		void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitMask, global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener);

		void CreateWithInvitationScreen(uint minOpponents, uint maxOppponents, uint variant, global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener);

		void ShowWaitingRoomUI();

		void GetAllInvitations(global::System.Action<global::GooglePlayGames.BasicApi.Multiplayer.Invitation[]> callback);

		void AcceptFromInbox(global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener);

		void AcceptInvitation(string invitationId, global::GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener listener);

		void SendMessageToAll(bool reliable, byte[] data);

		void SendMessageToAll(bool reliable, byte[] data, int offset, int length);

		void SendMessage(bool reliable, string participantId, byte[] data);

		void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length);

		global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant> GetConnectedParticipants();

		global::GooglePlayGames.BasicApi.Multiplayer.Participant GetSelf();

		global::GooglePlayGames.BasicApi.Multiplayer.Participant GetParticipant(string participantId);

		global::GooglePlayGames.BasicApi.Multiplayer.Invitation GetInvitation();

		void LeaveRoom();

		bool IsRoomConnected();

		void DeclineInvitation(string invitationId);
	}
}
