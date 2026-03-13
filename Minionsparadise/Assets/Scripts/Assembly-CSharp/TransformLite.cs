public class TransformLite
{
	public global::UnityEngine.Vector3 position;

	public global::UnityEngine.Vector3 scale;

	public global::UnityEngine.Quaternion rotation;

	public TransformLite(global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation, global::UnityEngine.Vector3 scale)
	{
		this.position = position;
		this.rotation = rotation;
		this.scale = scale;
	}
}
