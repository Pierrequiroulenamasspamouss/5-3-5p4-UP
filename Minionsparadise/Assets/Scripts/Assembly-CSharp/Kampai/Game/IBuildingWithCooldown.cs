namespace Kampai.Game
{
	public interface IBuildingWithCooldown : global::Kampai.Game.Building, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
		int GetCooldown();
	}
}
