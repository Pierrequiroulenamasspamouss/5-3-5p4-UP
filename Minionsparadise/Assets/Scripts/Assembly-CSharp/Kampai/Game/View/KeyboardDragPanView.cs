namespace Kampai.Game.View
{
	public class KeyboardDragPanView : global::Kampai.Game.View.DragPanView
	{
		public override void CalculateBehaviour(global::UnityEngine.Vector3 position)
		{
			if (position.x < xThreshold || position.x > screenWidth - xThreshold || position.y < yThreshold || position.y > screenHeight - yThreshold)
			{
				pan = true;
			}
			else
			{
				pan = false;
			}
		}
	}
}
