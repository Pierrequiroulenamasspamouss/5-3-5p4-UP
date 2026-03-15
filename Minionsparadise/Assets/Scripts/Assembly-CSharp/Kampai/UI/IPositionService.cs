namespace Kampai.UI
{
	public interface IPositionService
	{
		global::UnityEngine.Vector2 GetUIAnchorRatioPosition(global::UnityEngine.Vector3 worldPosition);

		global::UnityEngine.Vector2 GetUIAnchorRatioPosition(int buildingInstanceID);

		global::Kampai.UI.PositionData GetPositionData(global::UnityEngine.Vector3 worldPosition);

		global::Kampai.UI.SnappablePositionData GetSnappablePositionData(global::Kampai.UI.PositionData normalPositionData, global::Kampai.UI.ViewportBoundary boundary, bool avoidHudElements = false);

		void AddHUDElementToAvoid(global::UnityEngine.GameObject toAppend, bool isCircleShape = false);

		void RemoveHUDElementToAvoid(global::UnityEngine.GameObject toRemove);
	}
}
