namespace Kampai.Game
{
	public class SocialClaimRewardItem
	{
		public enum ClaimState
		{
			REWARD_CLAIMED = 1,
			EVENT_NOT_COMPLETED = 2,
			EVENT_COMPLETED_REWARD_NOT_CLAIMED = 3,
			UNKNOWN = 4
		}

		public int eventID;

		public global::Kampai.Game.SocialClaimRewardItem.ClaimState claimState;

		public SocialClaimRewardItem(int eventID = 0, global::Kampai.Game.SocialClaimRewardItem.ClaimState claimState = global::Kampai.Game.SocialClaimRewardItem.ClaimState.UNKNOWN)
		{
			this.eventID = eventID;
			this.claimState = claimState;
		}
	}
}
