namespace Kampai.Game
{
	public class StageBuilding : global::Kampai.Game.RepairableBuilding<global::Kampai.Game.StageBuildingDefinition>, global::Kampai.Game.Building, global::Kampai.Game.ZoomableBuilding, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
		public global::Kampai.Game.ZoomableBuildingDefinition ZoomableDefinition
		{
			get
			{
				return base.Definition;
			}
		}

		public StageBuilding(global::Kampai.Game.StageBuildingDefinition def)
			: base(def)
		{
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.StageBuildingObject>();
		}
	}
}
