namespace GooglePlayGames.BasicApi.Multiplayer
{
	public class Invitation
	{
		public enum InvType
		{
			RealTime = 0,
			TurnBased = 1,
			Unknown = 2
		}

		private global::GooglePlayGames.BasicApi.Multiplayer.Invitation.InvType mInvitationType;

		private string mInvitationId;

		private global::GooglePlayGames.BasicApi.Multiplayer.Participant mInviter;

		private int mVariant;

		public global::GooglePlayGames.BasicApi.Multiplayer.Invitation.InvType InvitationType
		{
			get
			{
				return mInvitationType;
			}
		}

		public string InvitationId
		{
			get
			{
				return mInvitationId;
			}
		}

		public global::GooglePlayGames.BasicApi.Multiplayer.Participant Inviter
		{
			get
			{
				return mInviter;
			}
		}

		public int Variant
		{
			get
			{
				return mVariant;
			}
		}

		internal Invitation(global::GooglePlayGames.BasicApi.Multiplayer.Invitation.InvType invType, string invId, global::GooglePlayGames.BasicApi.Multiplayer.Participant inviter, int variant)
		{
			mInvitationType = invType;
			mInvitationId = invId;
			mInviter = inviter;
			mVariant = variant;
		}

		public override string ToString()
		{
			return string.Format("[Invitation: InvitationType={0}, InvitationId={1}, Inviter={2}, Variant={3}]", InvitationType, InvitationId, Inviter, Variant);
		}
	}
}
