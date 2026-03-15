namespace Kampai.Game
{
	public interface ZoomableBuilding : global::Kampai.Game.Building, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
		global::Kampai.Game.ZoomableBuildingDefinition ZoomableDefinition { get; }
	}
}
