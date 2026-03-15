namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class TurnBasedMatch
	{
		public enum MatchStatus
		{
			Active = 0,
			AutoMatching = 1,
			Cancelled = 2,
			Complete = 3,
			Expired = 4,
			Unknown = 5,
			Deleted = 6
		}

		public enum MatchTurnStatus
		{
			Complete = 0,
			Invited = 1,
			MyTurn = 2,
			TheirTurn = 3,
			Unknown = 4
		}

		private string mMatchId;

		private byte[] mData;

		private bool mCanRematch;

		private uint mAvailableAutomatchSlots;

		private string mSelfParticipantId;

		private global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant> mParticipants;

		private string mPendingParticipantId;

		private global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus mTurnStatus;

		private global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus mMatchStatus;

		private uint mVariant;

		private uint mVersion;

		public string MatchId
		{
			get
			{
				return mMatchId;
			}
		}

		public byte[] Data
		{
			get
			{
				return mData;
			}
		}

		public bool CanRematch
		{
			get
			{
				return mCanRematch;
			}
		}

		public string SelfParticipantId
		{
			get
			{
				return mSelfParticipantId;
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.Participant Self
		{
			get
			{
				return GetParticipant(mSelfParticipantId);
			}
		}

		public global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant> Participants
		{
			get
			{
				return mParticipants;
			}
		}

		public string PendingParticipantId
		{
			get
			{
				return mPendingParticipantId;
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.Participant PendingParticipant
		{
			get
			{
				return (mPendingParticipantId != null) ? GetParticipant(mPendingParticipantId) : null;
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus TurnStatus
		{
			get
			{
				return mTurnStatus;
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus Status
		{
			get
			{
				return mMatchStatus;
			}
		}

		public uint Variant
		{
			get
			{
				return mVariant;
			}
		}

		public uint Version
		{
			get
			{
				return mVersion;
			}
		}

		public uint AvailableAutomatchSlots
		{
			get
			{
				return mAvailableAutomatchSlots;
			}
		}

		internal TurnBasedMatch(string matchId, byte[] data, bool canRematch, string selfParticipantId, global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant> participants, uint availableAutomatchSlots, string pendingParticipantId, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus turnStatus, global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus matchStatus, uint variant, uint version)
		{
			mMatchId = matchId;
			mData = data;
			mCanRematch = canRematch;
			mSelfParticipantId = selfParticipantId;
			mParticipants = participants;
			mParticipants.Sort();
			mAvailableAutomatchSlots = availableAutomatchSlots;
			mPendingParticipantId = pendingParticipantId;
			mTurnStatus = turnStatus;
			mMatchStatus = matchStatus;
			mVariant = variant;
			mVersion = version;
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.Participant GetParticipant(string participantId)
		{
			foreach (global::GooglePlayGames.BasicApi.Multiplayer.Participant mParticipant in mParticipants)
			{
				if (mParticipant.ParticipantId.Equals(participantId))
				{
					return mParticipant;
				}
			}
			global::GooglePlayGames.OurUtils.Logger.w("Participant not found in turn-based match: " + participantId);
			return null;
		}

		public override string ToString()
		{
			return string.Format("[TurnBasedMatch: mMatchId={0}, mData={1}, mCanRematch={2}, mSelfParticipantId={3}, mParticipants={4}, mPendingParticipantId={5}, mTurnStatus={6}, mMatchStatus={7}, mVariant={8}, mVersion={9}]", mMatchId, mData, mCanRematch, mSelfParticipantId, string.Join(",", global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(mParticipants, (global::GooglePlayGames.BasicApi.Multiplayer.Participant p) => p.ToString()))), mPendingParticipantId, mTurnStatus, mMatchStatus, mVariant, mVersion);
		}
	}
}
