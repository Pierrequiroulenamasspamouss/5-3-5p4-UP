public class MignetteShadowViewObject : global::UnityEngine.MonoBehaviour
{
	private global::UnityEngine.Quaternion initialQuat = global::UnityEngine.Quaternion.identity;

	private void Start()
	{
		initialQuat = base.transform.rotation;
	}

	private void LateUpdate()
	{
		global::UnityEngine.Vector3 position = base.transform.position;
		position.y = 0.01f;
		base.transform.position = position;
		base.transform.rotation = initialQuat;
	}
}
