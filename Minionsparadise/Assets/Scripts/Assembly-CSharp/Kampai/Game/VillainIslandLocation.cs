namespace Kampai.Game
{
	public class VillainIslandLocation : global::strange.extensions.mediation.impl.View
	{
		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> colliders;

		public void EnableColliders(bool enable)
		{
			foreach (global::UnityEngine.GameObject collider in colliders)
			{
				collider.GetComponent<global::UnityEngine.Collider>().enabled = enable;
			}
		}
	}
}
