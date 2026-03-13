namespace Kampai.Game
{
	internal class PlayerSerializerV7 : global::Kampai.Game.PlayerSerializerV6
	{
		public override int Version
		{
			get
			{
				return 7;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 7)
			{
				if (player.HighestFtueLevel >= 7)
				{
					player.GetByInstanceId<global::Kampai.Game.StageBuilding>(370).State = global::Kampai.Game.BuildingState.Idle;
				}
				global::Kampai.Game.DCNBuilding firstInstanceByDefinitionId = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.DCNBuilding>(3128);
				if (firstInstanceByDefinitionId == null)
				{
					global::Kampai.Game.DCNBuildingDefinition definition = null;
					if (definitionService.TryGet<global::Kampai.Game.DCNBuildingDefinition>(3128, out definition))
					{
						firstInstanceByDefinitionId = definition.Build() as global::Kampai.Game.DCNBuilding;
						firstInstanceByDefinitionId.State = global::Kampai.Game.BuildingState.Idle;
						firstInstanceByDefinitionId.Location = new global::Kampai.Game.Location(107, 172);
						player.AssignNextInstanceId(firstInstanceByDefinitionId);
						player.Add(firstInstanceByDefinitionId);
					}
				}
				player.Version = 7;
			}
			return player;
		}
	}
}
