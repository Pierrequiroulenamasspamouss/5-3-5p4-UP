namespace Kampai.Game
{
	public class SetupAspirationalBuildingsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionService landExpansionService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionConfigService landExpansionConfigService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateInventoryBuildingSignal createInventoryBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Util.ICoroutineProgressMonitor coroutineProgressMonitor { get; set; }

		public override void Execute()
		{
			coroutineProgressMonitor.StartTask(Setup(), "setup aspirational");
		}

		private bool IsBuildingInInventory(global::Kampai.Game.AspirationalBuildingDefinition aspirationalDef)
		{
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.Building>(aspirationalDef.BuildingDefinitionID);
			foreach (global::Kampai.Game.Building item in byDefinitionId)
			{
				if (aspirationalDef.Location.x == item.Location.x && aspirationalDef.Location.y == item.Location.y)
				{
					return true;
				}
			}
			return false;
		}

		private global::System.Collections.IEnumerator Setup()
		{
			global::Kampai.Game.PurchasedLandExpansion purchasedLandExpansion = playerService.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
			foreach (int expansionId in landExpansionConfigService.GetExpansionIds())
			{
				if (purchasedLandExpansion.HasPurchased(expansionId))
				{
					continue;
				}
				global::Kampai.Game.LandExpansionConfig config = landExpansionConfigService.GetExpansionConfig(expansionId);
				if (config.containedAspirationalBuildings == null)
				{
					continue;
				}
				foreach (int aspirationalDefId in landExpansionConfigService.GetExpansionConfig(expansionId).containedAspirationalBuildings)
				{
					global::Kampai.Game.AspirationalBuildingDefinition aspirationalDef = definitionService.Get<global::Kampai.Game.AspirationalBuildingDefinition>(aspirationalDefId);
					if (!IsBuildingInInventory(aspirationalDef))
					{
						global::Kampai.Game.BuildingDefinition buildingDef = definitionService.Get<global::Kampai.Game.BuildingDefinition>(aspirationalDef.BuildingDefinitionID);
						global::Kampai.Game.Building aspirationalBuilding = buildingDef.BuildBuilding();
						aspirationalBuilding.Location = new global::Kampai.Game.Location(aspirationalDef.Location.x, aspirationalDef.Location.y);
						aspirationalBuilding.SetState(global::Kampai.Game.BuildingState.Idle);
						aspirationalBuilding.ID = -aspirationalDefId;
						landExpansionService.TrackAspirationalBuilding(aspirationalDefId, aspirationalBuilding);
						createInventoryBuildingSignal.Dispatch(aspirationalBuilding, aspirationalBuilding.Location);
					}
				}
				yield return null;
			}
		}
	}
}
