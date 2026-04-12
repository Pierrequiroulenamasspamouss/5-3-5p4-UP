namespace Kampai.UI.View
{
	public class ModButtonScaleFixer : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.Vector3 TargetScale = global::UnityEngine.Vector3.one;

		private void LateUpdate()
		{
			// LateUpdate ensures we override the Animator's scale reset each frame
			if (base.transform.localScale != TargetScale)
			{
				base.transform.localScale = TargetScale;
			}
		}
	}
}
