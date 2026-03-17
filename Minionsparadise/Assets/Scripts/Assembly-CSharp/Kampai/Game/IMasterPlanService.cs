namespace Kampai.Game
{
	public interface IMasterPlanService
	{
		global::Kampai.Game.MasterPlan CurrentMasterPlan { get; }

		void Initialize();

		void CreateMasterPlanComponents(global::Kampai.Game.MasterPlan masterPlan);

		global::Kampai.Game.MasterPlan CreateNewMasterPlan();

		bool HasReceivedInitialRewardFromCurrentPlan();

		bool HasReceivedInitialRewardFromPlanDefinition(global::Kampai.Game.MasterPlanDefinition planDefinition);

		global::UnityEngine.Vector3 GetComponentBuildingOffset(int buildingID);

		global::UnityEngine.Vector3 GetComponentBuildingPosition(int buildingID);

		bool ForceNextMPDefinition(int defID);

		void SelectMasterPlanComponent(global::Kampai.Game.MasterPlanComponent component);

		void ProcessTransactionData(global::Kampai.Game.Transaction.TransactionUpdateData data);

		void ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType type, uint progress, int source = 0);

		void AddMasterPlanObject(global::Kampai.Game.View.MasterPlanObject obj);

		bool AllComponentsAreComplete(int masterPlanDefinitionID);

		int GetComponentCompleteCount(global::Kampai.Game.MasterPlanDefinition definition);

		void SetWayfinderState();

		global::Kampai.Game.MasterPlanComponent GetActiveComponentFromPlanDefinition(int masterPlanDefinitionID);
	}
}
