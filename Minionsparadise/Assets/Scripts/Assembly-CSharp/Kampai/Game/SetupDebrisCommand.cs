namespace Kampai.Game
{
	public class SetupDebrisCommand : global::strange.extensions.command.impl.Command
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

		public override void Execute()
		{
			global::Kampai.Game.PurchasedLandExpansion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
			foreach (int expansionId in landExpansionConfigService.GetExpansionIds())
			{
				if (byInstanceId.HasPurchased(expansionId))
				{
					continue;
				}
				global::Kampai.Game.LandExpansionConfig expansionConfig = landExpansionConfigService.GetExpansionConfig(expansionId);
				if (expansionConfig.containedDebris == null)
				{
					continue;
				}
				foreach (int containedDebri in expansionConfig.containedDebris)
				{
					global::Kampai.Game.DebrisDefinition debrisDefinition = definitionService.Get<global::Kampai.Game.DebrisDefinition>(containedDebri);
					global::Kampai.Game.DebrisBuildingDefinition debrisBuildingDefinition = definitionService.Get<global::Kampai.Game.DebrisBuildingDefinition>(debrisDefinition.BuildingDefinitionID);
					global::Kampai.Game.DebrisBuilding debrisBuilding = debrisBuildingDefinition.Build() as global::Kampai.Game.DebrisBuilding;
					debrisBuilding.Location = new global::Kampai.Game.Location(debrisDefinition.Location.x, debrisDefinition.Location.y);
					debrisBuilding.SetState(global::Kampai.Game.BuildingState.Disabled);
					debrisBuilding.ID = -containedDebri;
					landExpansionService.TrackDebris(containedDebri, debrisBuilding);
					createInventoryBuildingSignal.Dispatch(debrisBuilding, debrisBuilding.Location);
				}
			}
		}
	}
}
