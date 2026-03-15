namespace Kampai.Game
{
	public class MasterPlanLeftOverBuilding : global::Kampai.Game.Building<global::Kampai.Game.MasterPlanLeftOverBuildingDefinition>
	{
		public MasterPlanLeftOverBuilding(global::Kampai.Game.MasterPlanLeftOverBuildingDefinition def)
			: base(def)
		{
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.MasterPlanLeftOverBuildingObject>();
		}
	}
}
