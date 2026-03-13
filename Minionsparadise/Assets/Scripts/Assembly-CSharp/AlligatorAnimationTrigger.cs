public class AlligatorAnimationTrigger : global::UnityEngine.MonoBehaviour
{
	public string AnimationStateName = string.Empty;

	public void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		AlligatorAgent component = other.GetComponent<AlligatorAgent>();
		if (component != null)
		{
			component.TrySetAnimationState(AnimationStateName);
		}
	}
}
