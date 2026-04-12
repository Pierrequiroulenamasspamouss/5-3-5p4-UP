namespace Kampai.Game
{
	internal class PlayerSerializerV3 : global::Kampai.Game.PlayerSerializerV2
	{
		public override int Version
		{
			get
			{
				return 3;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 3)
			{
				foreach (global::Kampai.Game.Villain item in player.GetInstancesByType<global::Kampai.Game.Villain>())
				{
					global::Kampai.Game.CabanaBuilding cabana = item.Cabana;
					if (cabana != null)
					{
						item.CabanaBuildingId = cabana.ID;
						item.Cabana = null;
					}
				}
				player.Version = 3;
			}
			return player;
		}
	}
}
