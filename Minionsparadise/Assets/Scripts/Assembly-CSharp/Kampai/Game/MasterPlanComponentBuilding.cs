namespace Kampai.Game
{
	public class MasterPlanComponentBuilding : global::Kampai.Game.Building<global::Kampai.Game.MasterPlanComponentBuildingDefinition>
	{
		public MasterPlanComponentBuilding(global::Kampai.Game.MasterPlanComponentBuildingDefinition def)
			: base(def)
		{
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.MasterPlanComponentBuildingObject>();
		}

		public override string GetPrefab(int index = 0)
		{
			return base.Definition.Prefab;
		}
	}
}
