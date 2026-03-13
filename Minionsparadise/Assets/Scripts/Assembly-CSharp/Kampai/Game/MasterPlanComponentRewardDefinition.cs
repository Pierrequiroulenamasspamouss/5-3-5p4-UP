namespace Kampai.Game
{
	public class MasterPlanComponentRewardDefinition
	{
		public int rewardItemId { get; set; }

		public uint rewardQuantity { get; set; }

		public uint grindReward { get; set; }

		public uint premiumReward { get; set; }

		public global::Kampai.Game.MasterPlanComponentReward Build()
		{
			global::Kampai.Game.MasterPlanComponentReward masterPlanComponentReward = new global::Kampai.Game.MasterPlanComponentReward();
			masterPlanComponentReward.Definition = this;
			return masterPlanComponentReward;
		}
	}
}
