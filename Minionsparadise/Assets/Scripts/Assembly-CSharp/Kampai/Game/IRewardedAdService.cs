namespace Kampai.Game
{
	public interface IRewardedAdService
	{
		void Initialize();

		bool IsRewardedVideoAvailable();

		bool IsPlacementAvailable(global::Kampai.Game.AdPlacementName placementName);

		bool IsPlacementActive(global::Kampai.Game.AdPlacementName placementName, int instanceId = 0);

		global::Kampai.Game.AdPlacementInstance GetPlacementInstance(global::Kampai.Game.AdPlacementName placementName, int instanceId = 0);

		void ShowRewardedVideo(global::Kampai.Game.AdPlacementInstance instance, global::Kampai.Game.Transaction.TransactionDefinition reward = null);

		void RewardPlayer(global::Kampai.Game.Transaction.TransactionDefinition reward, global::Kampai.Game.AdPlacementInstance placementInstance);
	}
}
