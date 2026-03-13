namespace Kampai.Game
{
	public interface IZoomCameraModel
	{
		bool ZoomedIn { get; set; }

		bool ZoomInProgress { get; set; }

		global::Kampai.Game.BuildingZoomType LastZoomBuildingType { get; set; }

		int PreviousCameraBehavior { get; set; }

		global::UnityEngine.Vector3 PreviousCameraPosition { get; set; }

		global::UnityEngine.Vector3 PreviousCameraRotation { get; set; }

		float PreviousCameraFieldOfView { get; set; }

		float LastResourceZoomRotation { get; set; }

		global::UnityEngine.Vector3 GetZoomedCameraPosition(global::Kampai.Game.ZoomableBuilding building);

		global::UnityEngine.Quaternion GetZoomedCameraRotation(global::Kampai.Game.ZoomableBuilding building);

		float GetZoomedFOV(global::Kampai.Game.ZoomableBuilding building);
	}
}
