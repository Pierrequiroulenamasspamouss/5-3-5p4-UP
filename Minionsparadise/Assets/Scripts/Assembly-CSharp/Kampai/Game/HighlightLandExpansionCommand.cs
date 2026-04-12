namespace Kampai.Game
{
	public class HighlightLandExpansionCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int expansionID { get; set; }

		[Inject]
		public bool enabled { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionService landExpansionService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding> list = landExpansionService.GetBuildingsByExpansionID(expansionID) as global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionBuilding>;
			foreach (global::Kampai.Game.LandExpansionBuilding item in list)
			{
				global::Kampai.Game.View.LandExpansionBuildingObject landExpansionBuildingObject = component.GetBuildingObject(item.ID) as global::Kampai.Game.View.LandExpansionBuildingObject;
				if (landExpansionBuildingObject != null)
				{
					landExpansionBuildingObject.Highlight(enabled);
				}
			}
		}
	}
}
