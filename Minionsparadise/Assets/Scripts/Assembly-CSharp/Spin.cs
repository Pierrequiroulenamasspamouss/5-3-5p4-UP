public class Spin : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Vector3 amount;

	private void Update()
	{
		base.transform.Rotate(amount);
	}
}
