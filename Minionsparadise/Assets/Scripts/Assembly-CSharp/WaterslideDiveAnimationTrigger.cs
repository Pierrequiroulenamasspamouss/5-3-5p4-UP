public class WaterslideDiveAnimationTrigger : global::UnityEngine.MonoBehaviour
{
	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		PathAgent componentInParent = other.transform.GetComponentInParent<PathAgent>();
		if (componentInParent != null)
		{
			componentInParent.OnPlayDiveAnimation();
		}
	}
}
