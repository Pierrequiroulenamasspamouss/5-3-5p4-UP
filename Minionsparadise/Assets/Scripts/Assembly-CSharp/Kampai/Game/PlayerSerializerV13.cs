namespace Kampai.Game
{
	internal class PlayerSerializerV13 : global::Kampai.Game.PlayerSerializerV12
	{
		public override int Version
		{
			get
			{
				return 13;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 13)
			{
				player.Version = 13;
			}
			return player;
		}
	}
}
