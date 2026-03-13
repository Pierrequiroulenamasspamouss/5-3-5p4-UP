namespace Kampai.Game
{
	public struct BuildingZoomSettings
	{
		public global::Kampai.Game.ZoomType ZoomType { get; private set; }

		public global::Kampai.Game.BuildingZoomType ZoomBuildingType { get; private set; }

		public global::System.Action OnComplete { get; private set; }

		public bool EnableCamera { get; private set; }

		public BuildingZoomSettings(global::Kampai.Game.ZoomType zoomType, global::Kampai.Game.BuildingZoomType zoomBuildingType, global::System.Action onComplete = null, bool enableCamera = true)
		{
			ZoomType = zoomType;
			ZoomBuildingType = zoomBuildingType;
			OnComplete = onComplete;
			EnableCamera = enableCamera;
		}
	}
}
