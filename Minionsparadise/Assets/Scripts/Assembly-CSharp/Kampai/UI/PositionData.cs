namespace Kampai.UI
{
	public class PositionData
	{
		public global::UnityEngine.Vector3 WorldPositionInUI;

		public global::UnityEngine.Vector3 ViewportPosition;

		public global::UnityEngine.Vector3 ViewportDirectionFromCenter;

		public bool IsVisible;

		public PositionData(global::UnityEngine.Vector3 worldPositionInUI, global::UnityEngine.Vector3 viewportPosition)
		{
			WorldPositionInUI = worldPositionInUI;
			ViewportDirectionFromCenter = viewportPosition - new global::UnityEngine.Vector3(0.5f, 0.5f, viewportPosition.z);
			if (viewportPosition.z < 0f)
			{
				viewportPosition.x *= -1f;
				viewportPosition.y *= -1f;
			}
			ViewportPosition = viewportPosition;
			IsVisible = viewportPosition.x >= 0f && viewportPosition.x <= 1f && viewportPosition.y >= 0f && viewportPosition.y <= 1f;
		}

		public PositionData(global::Kampai.UI.SnappablePositionData data)
		{
			WorldPositionInUI = data.WorldPositionInUI;
			ViewportPosition = data.ViewportPosition;
			ViewportDirectionFromCenter = data.ViewportDirectionFromCenter;
			IsVisible = data.IsVisible;
		}
	}
}
