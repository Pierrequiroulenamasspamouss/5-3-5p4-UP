namespace Kampai.Game
{
	public class DecorationBuilding : global::Kampai.Game.Building<global::Kampai.Game.DecorationBuildingDefinition>
	{
		public DecorationBuilding(global::Kampai.Game.DecorationBuildingDefinition def)
			: base(def)
		{
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.DecorationBuildingObject>();
		}
	}
}
