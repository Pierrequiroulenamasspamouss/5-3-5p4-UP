namespace Kampai.Game
{
	public class ZoomCameraModel : global::Kampai.Game.IZoomCameraModel
	{
		public bool ZoomedIn { get; set; }

		public bool ZoomInProgress { get; set; }

		public global::Kampai.Game.BuildingZoomType LastZoomBuildingType { get; set; }

		public int PreviousCameraBehavior { get; set; }

		public global::UnityEngine.Vector3 PreviousCameraPosition { get; set; }

		public global::UnityEngine.Vector3 PreviousCameraRotation { get; set; }

		public float PreviousCameraFieldOfView { get; set; }

		public float LastResourceZoomRotation { get; set; }

		public global::UnityEngine.Vector3 GetZoomedCameraPosition(global::Kampai.Game.ZoomableBuilding building)
		{
			global::Kampai.Game.ZoomableBuildingDefinition zoomableDefinition = building.ZoomableDefinition;
			global::UnityEngine.Vector3 zoomOffset = zoomableDefinition.zoomOffset;
			global::Kampai.Game.Location location = building.Location;
			return new global::UnityEngine.Vector3(location.x, 0f, location.y) + zoomOffset;
		}

		public global::UnityEngine.Quaternion GetZoomedCameraRotation(global::Kampai.Game.ZoomableBuilding building)
		{
			return global::UnityEngine.Quaternion.Euler(building.ZoomableDefinition.zoomEulers);
		}

		public float GetZoomedFOV(global::Kampai.Game.ZoomableBuilding building)
		{
			global::Kampai.Game.ZoomableBuildingDefinition zoomableDefinition = building.ZoomableDefinition;
			return zoomableDefinition.zoomFOV;
		}
	}
}
