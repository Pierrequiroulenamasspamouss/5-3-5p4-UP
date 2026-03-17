namespace Kampai.Game
{
	public class SpecialBuilding : global::Kampai.Game.Building<global::Kampai.Game.SpecialBuildingDefinition>
	{
		public SpecialBuilding(global::Kampai.Game.SpecialBuildingDefinition def)
			: base(def)
		{
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.SpecialBuildingObject>();
		}
	}
}
