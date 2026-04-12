namespace Kampai.Game
{
	public class WelcomeHutBuilding : global::Kampai.Game.RepairableBuilding<global::Kampai.Game.WelcomeHutBuildingDefinition>
	{
		public WelcomeHutBuilding(global::Kampai.Game.WelcomeHutBuildingDefinition def)
			: base(def)
		{
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.WelcomeHutBuildingObject>();
		}
	}
}
