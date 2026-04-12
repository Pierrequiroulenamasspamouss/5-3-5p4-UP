namespace Kampai.Game
{
	public class DCNBuilding : global::Kampai.Game.Building<global::Kampai.Game.DCNBuildingDefinition>
	{
		public DCNBuilding(global::Kampai.Game.DCNBuildingDefinition def)
			: base(def)
		{
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.DCNBuildingObjectView>();
		}
	}
}
