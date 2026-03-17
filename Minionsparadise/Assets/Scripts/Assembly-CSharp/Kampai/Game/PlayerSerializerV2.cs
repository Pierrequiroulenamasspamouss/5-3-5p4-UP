namespace Kampai.Game
{
	internal class PlayerSerializerV2 : global::Kampai.Game.PlayerSerializerV1
	{
		public override int Version
		{
			get
			{
				return 2;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 2)
			{
				if (player.GetByDefinitionId<global::Kampai.Game.Building>(3502).Count == 0)
				{
					global::Kampai.Game.AspirationalBuildingDefinition aspirationalBuildingDefinition = definitionService.Get<global::Kampai.Game.AspirationalBuildingDefinition>(46002);
					global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get<global::Kampai.Game.BuildingDefinition>(3502);
					global::Kampai.Game.Building building = buildingDefinition.BuildBuilding();
					building.Location = new global::Kampai.Game.Location(aspirationalBuildingDefinition.Location.x, aspirationalBuildingDefinition.Location.y);
					building.SetState(global::Kampai.Game.BuildingState.Idle);
					player.AssignNextInstanceId(building);
					player.Add(building);
				}
				global::Kampai.Game.PurchasedLandExpansion byInstanceId = player.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
				if (!byInstanceId.HasPurchased(197379) && !byInstanceId.IsAdjacentExpansion(197379))
				{
					byInstanceId.AdjacentExpansions.Add(197379);
				}
				if (!byInstanceId.HasPurchased(789516) && !byInstanceId.IsAdjacentExpansion(789516))
				{
					byInstanceId.AdjacentExpansions.Add(789516);
				}
				if (byInstanceId.HasPurchased(592137))
				{
					byInstanceId.PurchasedExpansions.Remove(592137);
				}
				if (byInstanceId.IsAdjacentExpansion(592137))
				{
					byInstanceId.AdjacentExpansions.Remove(592137);
				}
				player.Version = 2;
			}
			return player;
		}
	}
}
