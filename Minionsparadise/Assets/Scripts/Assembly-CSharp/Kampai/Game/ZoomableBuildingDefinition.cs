namespace Kampai.Game
{
	public interface ZoomableBuildingDefinition
	{
		global::UnityEngine.Vector3 zoomOffset { get; set; }

		global::UnityEngine.Vector3 zoomEulers { get; set; }

		float zoomFOV { get; set; }
	}
}
