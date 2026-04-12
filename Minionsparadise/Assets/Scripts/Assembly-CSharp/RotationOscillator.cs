public class RotationOscillator : global::UnityEngine.MonoBehaviour
{
	public bool XAxis;

	public bool YAxis;

	public bool ZAxis;

	public global::UnityEngine.Vector3 frequency;

	public global::UnityEngine.Vector3 magnitude;

	private global::UnityEngine.Quaternion rotation;

	private global::UnityEngine.Vector3 axis;

	private float elapsedTime;

	private void Start()
	{
		rotation = base.transform.localRotation;
		axis = new global::UnityEngine.Vector3((!XAxis) ? 0f : 1f, (!YAxis) ? 0f : 1f, (!ZAxis) ? 0f : 1f);
	}

	private void FixedUpdate()
	{
		elapsedTime += global::UnityEngine.Time.deltaTime;
		base.transform.localRotation = new global::UnityEngine.Quaternion(rotation.x + axis.x * global::UnityEngine.Mathf.Sin(elapsedTime * frequency.x) * magnitude.x, rotation.y + axis.y * global::UnityEngine.Mathf.Sin(elapsedTime * frequency.y) * magnitude.y, rotation.z + axis.z * global::UnityEngine.Mathf.Sin(elapsedTime * frequency.z) * magnitude.z, 1f);
	}
}
