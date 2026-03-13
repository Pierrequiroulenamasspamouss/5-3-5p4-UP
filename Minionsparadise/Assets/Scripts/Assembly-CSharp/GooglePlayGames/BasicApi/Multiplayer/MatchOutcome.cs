namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class MatchOutcome
	{
		public enum ParticipantResult
		{
			Unset = -1,
			None = 0,
			Win = 1,
			Loss = 2,
			Tie = 3
		}

		public const uint PlacementUnset = 0u;

		private global::System.Collections.Generic.List<string> mParticipantIds = new global::System.Collections.Generic.List<string>();

		private global::System.Collections.Generic.Dictionary<string, uint> mPlacements = new global::System.Collections.Generic.Dictionary<string, uint>();

		private global::System.Collections.Generic.Dictionary<string, global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult> mResults = new global::System.Collections.Generic.Dictionary<string, global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult>();

		public global::System.Collections.Generic.List<string> ParticipantIds
		{
			get
			{
				return mParticipantIds;
			}
		}

		public void SetParticipantResult(string participantId, global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult result, uint placement)
		{
			if (!mParticipantIds.Contains(participantId))
			{
				mParticipantIds.Add(participantId);
			}
			mPlacements[participantId] = placement;
			mResults[participantId] = result;
		}

		public void SetParticipantResult(string participantId, global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult result)
		{
			SetParticipantResult(participantId, result, 0u);
		}

		public void SetParticipantResult(string participantId, uint placement)
		{
			SetParticipantResult(participantId, global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult.Unset, placement);
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult GetResultFor(string participantId)
		{
			return (!mResults.ContainsKey(participantId)) ? global::GooglePlayGames.BasicApi.Multiplayer.MatchOutcome.ParticipantResult.Unset : mResults[participantId];
		}

		public uint GetPlacementFor(string participantId)
		{
			return mPlacements.ContainsKey(participantId) ? mPlacements[participantId] : 0u;
		}

		public override string ToString()
		{
			string text = "[MatchOutcome";
			foreach (string mParticipantId in mParticipantIds)
			{
				text += string.Format(" {0}->({1},{2})", mParticipantId, GetResultFor(mParticipantId), GetPlacementFor(mParticipantId));
			}
			return text + "]";
		}
	}
}
