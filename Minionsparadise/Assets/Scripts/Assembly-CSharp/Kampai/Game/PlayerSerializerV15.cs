namespace Kampai.Game
{
	internal sealed class PlayerSerializerV15 : global::Kampai.Game.PlayerSerializerV14
	{
		public override int Version
		{
			get
			{
				return 15;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 15)
			{
				UpdateLairPosition(player);
				UpdateBridgePosition(player);
				player.Version = 15;
			}
			return player;
		}

		private void UpdateLairPosition(global::Kampai.Game.Player player)
		{
			global::Kampai.Game.VillainLairEntranceBuilding byInstanceId = player.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(374);
			if (byInstanceId != null)
			{
				byInstanceId.Location.x = 164;
				byInstanceId.Location.y = 209;
			}
		}

		private void UpdateBridgePosition(global::Kampai.Game.Player player)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.BridgeBuilding> instancesByType = player.GetInstancesByType<global::Kampai.Game.BridgeBuilding>();
			foreach (global::Kampai.Game.BridgeBuilding item in instancesByType)
			{
				switch (item.Definition.ID)
				{
				case 3102:
				case 3105:
					item.Location.x = 159;
					break;
				case 3103:
				case 3104:
					item.Location.x = 98;
					break;
				}
			}
		}
	}
}
