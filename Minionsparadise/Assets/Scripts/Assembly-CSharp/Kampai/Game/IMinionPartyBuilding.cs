namespace Kampai.Game
{
	public interface IMinionPartyBuilding : global::Kampai.Game.Instance, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
		string GetPartyPrefab(global::Kampai.Game.MinionPartyType partyType);
	}
}
