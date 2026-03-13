public class WaterSlideTargetZoneAnimTrigger : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Animator landingZoneAnim;

	public global::UnityEngine.Animator unicornAnim;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (other != null)
		{
			landingZoneAnim.Play("splash");
			unicornAnim.Play("splash");
		}
	}
}
