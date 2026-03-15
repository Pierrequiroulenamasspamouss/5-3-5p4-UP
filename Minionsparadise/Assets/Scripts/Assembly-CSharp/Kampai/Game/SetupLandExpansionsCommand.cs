namespace Kampai.Game
{
	public class SetupLandExpansionsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateInventoryBuildingSignal createInventoryBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionService landExpansionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.SetupForSaleSignsSignal setupForSaleSignsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetupFlowersSignal setupFlowersSignal { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.LandExpansionDefinition> all = definitionService.GetAll<global::Kampai.Game.LandExpansionDefinition>();
			global::Kampai.Game.PurchasedLandExpansion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
			foreach (global::Kampai.Game.LandExpansionDefinition item in all)
			{
				if (!byInstanceId.PurchasedExpansions.Contains(item.ExpansionID))
				{
					global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get(item.BuildingDefinitionID) as global::Kampai.Game.BuildingDefinition;
					global::Kampai.Game.LandExpansionBuilding landExpansionBuilding = buildingDefinition.Build() as global::Kampai.Game.LandExpansionBuilding;
					landExpansionBuilding.State = global::Kampai.Game.BuildingState.Idle;
					global::Kampai.Game.Location location = null;
					location = new global::Kampai.Game.Location(item.Location.x, item.Location.y);
					landExpansionBuilding.ID = item.ID;
					landExpansionBuilding.ExpansionID = item.ExpansionID;
					landExpansionBuilding.Location = location;
					landExpansionBuilding.MinimumLevel = item.MinimumLevel;
					if (item.Grass)
					{
						landExpansionService.AddBuilding(landExpansionBuilding);
						createInventoryBuildingSignal.Dispatch(landExpansionBuilding, location);
					}
					else
					{
						landExpansionService.TrackFlower(landExpansionBuilding);
					}
				}
			}
			setupFlowersSignal.Dispatch();
			setupForSaleSignsSignal.Dispatch();
		}
	}
}
