namespace Kampai.Game.View
{
	public class BridgeBuildingObject : global::Kampai.Game.View.BuildingObject
	{
		public override bool CanFadeGFX()
		{
			return false;
		}

		public override bool CanFadeSFX()
		{
			return false;
		}

		protected override global::UnityEngine.Vector3 GetZoomCenterPosition()
		{
			global::UnityEngine.Vector3 zoomCenterPosition = base.GetZoomCenterPosition();
			zoomCenterPosition.y = 0f;
			return zoomCenterPosition;
		}
	}
}
