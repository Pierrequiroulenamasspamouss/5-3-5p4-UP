namespace Kampai.Game
{
	public interface IPlayerSerializer
	{
		int Version { get; }

		global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger);

		byte[] Serialize(global::Kampai.Game.Player player, global::Kampai.Game.IDefinitionService defintitionService, global::Kampai.Util.IKampaiLogger logger);
	}
}
