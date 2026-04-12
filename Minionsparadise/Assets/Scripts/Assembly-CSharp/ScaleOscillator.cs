public class ScaleOscillator : global::UnityEngine.MonoBehaviour
{
	public bool XAxis;

	public bool YAxis;

	public bool ZAxis;

	public global::UnityEngine.Vector3 frequency;

	public global::UnityEngine.Vector3 magnitude;

	private global::UnityEngine.Vector3 scale;

	private global::UnityEngine.Vector3 axis;

	private float elapsedTime;

	private void Start()
	{
		scale = base.transform.localScale;
		axis = new global::UnityEngine.Vector3((!XAxis) ? 0f : 1f, (!YAxis) ? 0f : 1f, (!ZAxis) ? 0f : 1f);
	}

	private void FixedUpdate()
	{
		elapsedTime += global::UnityEngine.Time.deltaTime;
		base.transform.localScale = new global::UnityEngine.Vector3(scale.x + axis.x * global::UnityEngine.Mathf.Sin(elapsedTime * frequency.x) * magnitude.x, scale.y + axis.y * global::UnityEngine.Mathf.Sin(elapsedTime * frequency.y) * magnitude.y, scale.z + axis.z * global::UnityEngine.Mathf.Sin(elapsedTime * frequency.z) * magnitude.z);
	}
}
