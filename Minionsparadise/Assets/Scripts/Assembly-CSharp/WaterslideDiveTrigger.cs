public class WaterslideDiveTrigger : global::UnityEngine.MonoBehaviour
{
	public bool StartRoll = true;

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		PathAgent componentInParent = other.transform.GetComponentInParent<PathAgent>();
		if (componentInParent != null)
		{
			componentInParent.OnDiveTrigger(StartRoll);
		}
	}
}
