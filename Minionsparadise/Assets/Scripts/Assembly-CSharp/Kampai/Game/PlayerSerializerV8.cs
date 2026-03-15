namespace Kampai.Game
{
	internal class PlayerSerializerV8 : global::Kampai.Game.PlayerSerializerV7
	{
		public override int Version
		{
			get
			{
				return 8;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 8)
			{
				global::Kampai.Game.MIBBuilding firstInstanceByDefinitionId = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.MIBBuilding>(3129);
				if (firstInstanceByDefinitionId == null)
				{
					global::Kampai.Game.MIBBuildingDefinition definition = null;
					if (definitionService.TryGet<global::Kampai.Game.MIBBuildingDefinition>(3129, out definition))
					{
						firstInstanceByDefinitionId = definition.Build() as global::Kampai.Game.MIBBuilding;
						firstInstanceByDefinitionId.State = global::Kampai.Game.BuildingState.Idle;
						firstInstanceByDefinitionId.Location = new global::Kampai.Game.Location(123, 176);
						player.AssignNextInstanceId(firstInstanceByDefinitionId);
						player.Add(firstInstanceByDefinitionId);
					}
				}
				player.Version = 8;
			}
			return player;
		}
	}
}
