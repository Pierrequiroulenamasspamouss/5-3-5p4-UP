namespace Kampai.Game
{
	public class FountainBuilding : global::Kampai.Game.RepairableBuilding<global::Kampai.Game.FountainBuildingDefinition>
	{
		public FountainBuilding(global::Kampai.Game.FountainBuildingDefinition def)
			: base(def)
		{
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.FountainBuildingObject>();
		}
	}
}
