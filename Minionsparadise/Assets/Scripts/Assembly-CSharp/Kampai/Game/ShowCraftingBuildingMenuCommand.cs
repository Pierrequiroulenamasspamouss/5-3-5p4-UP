namespace Kampai.Game
{
	public class ShowCraftingBuildingMenuCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Building building { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject BuildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.OpenBuildingMenuSignal openBuildingMenuSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.BuildingManagerView component = BuildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(building.ID);
			if (buildingObject != null)
			{
				openBuildingMenuSignal.Dispatch(buildingObject, building);
			}
		}
	}
}
