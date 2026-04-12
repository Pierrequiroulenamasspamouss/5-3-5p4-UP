namespace GooglePlayGames.BasicApi.Multiplayer
{
	public interface RealTimeMultiplayerListener
	{
		void OnRoomSetupProgress(float percent);

		void OnRoomConnected(bool success);

		void OnLeftRoom();

		void OnParticipantLeft(global::GooglePlayGames.BasicApi.Multiplayer.Participant participant);

		void OnPeersConnected(string[] participantIds);

		void OnPeersDisconnected(string[] participantIds);

		void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data);
	}
}
