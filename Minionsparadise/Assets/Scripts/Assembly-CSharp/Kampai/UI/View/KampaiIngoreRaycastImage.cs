namespace Kampai.UI.View
{
	public class KampaiIngoreRaycastImage : global::Kampai.UI.View.KampaiImage
	{
		public override bool IsRaycastLocationValid(global::UnityEngine.Vector2 sp, global::UnityEngine.Camera eventCamera)
		{
			return false;
		}
	}
}
