namespace Kampai.Game.View
{
	public class BuildingObjectCollection
	{
		public global::Kampai.Game.View.BuildingObject BuildingObject { get; set; }

		public global::Kampai.Game.View.ScaffoldingBuildingObject ScaffoldingBuildingObject { get; set; }

		public global::Kampai.Game.View.RibbonBuildingObject RibbonBuildingObject { get; set; }

		public global::Kampai.Game.View.PlatformBuildingObject PlatformBuildingObject { get; set; }

		public bool Rushed { get; set; }

		public BuildingObjectCollection(global::Kampai.Game.View.BuildingObject buildingObject)
		{
			BuildingObject = buildingObject;
		}
	}
}
