namespace Kampai.Game.View
{
	public class KeyboardPanView : global::Kampai.Game.View.PanView
	{
		public override void CalculateBehaviour(global::UnityEngine.Vector3 position)
		{
			mouseRay = mainCamera.ScreenPointToRay(position);
			groundPlane.Raycast(mouseRay, out hitDistance);
			if (!initialized)
			{
				initialized = true;
				hitPosition = mouseRay.GetPoint(hitDistance);
			}
			currentPosition = mouseRay.GetPoint(hitDistance);
			velocity = hitPosition - currentPosition;
		}
	}
}
