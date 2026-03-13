public class AlligatorVFXTimedDestroy : global::UnityEngine.MonoBehaviour
{
	public float Duration;

	private void Start()
	{
		StartCoroutine(TimedDestroy());
	}

	private global::System.Collections.IEnumerator TimedDestroy()
	{
		yield return new global::UnityEngine.WaitForSeconds(Duration);
		global::UnityEngine.Object.Destroy(base.gameObject);
	}
}
