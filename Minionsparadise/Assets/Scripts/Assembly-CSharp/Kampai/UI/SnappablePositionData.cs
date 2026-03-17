namespace Kampai.UI
{
	public class SnappablePositionData
	{
		public global::UnityEngine.Vector3 ClampedWorldPositionInUI;

		public global::UnityEngine.Vector3 ClampedViewportPosition;

		public global::UnityEngine.Vector3 WorldPositionInUI;

		public global::UnityEngine.Vector3 ViewportPosition;

		public global::UnityEngine.Vector3 ViewportDirectionFromCenter;

		public bool IsVisible;

		public SnappablePositionData(global::UnityEngine.Vector3 worldPositionInUI, global::UnityEngine.Vector3 clampedWorldPositionInUI, global::UnityEngine.Vector3 viewportPosition, global::UnityEngine.Vector3 clampedViewportPosition)
		{
			WorldPositionInUI = worldPositionInUI;
			ViewportDirectionFromCenter = viewportPosition - new global::UnityEngine.Vector3(0.5f, 0.5f, viewportPosition.z);
			if (viewportPosition.z < 0f)
			{
				viewportPosition.x *= -1f;
				viewportPosition.y *= -1f;
			}
			ViewportPosition = viewportPosition;
			ClampedWorldPositionInUI = clampedWorldPositionInUI;
			ClampedViewportPosition = clampedViewportPosition;
			IsVisible = ViewportPosition == clampedViewportPosition;
		}
	}
}
