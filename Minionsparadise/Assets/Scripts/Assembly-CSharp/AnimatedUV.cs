public class AnimatedUV : global::UnityEngine.MonoBehaviour
{
	public int materialIndex;

	public global::UnityEngine.Vector2 uvAnimationRate = new global::UnityEngine.Vector2(1f, 0f);

	public string textureName = "_MainTex";

	private global::UnityEngine.Vector2 uvOffset = global::UnityEngine.Vector2.zero;

	private global::UnityEngine.Renderer rend;

	private void Start()
	{
		rend = GetComponent<global::UnityEngine.Renderer>();
	}

	private void LateUpdate()
	{
		uvOffset += uvAnimationRate * global::UnityEngine.Time.deltaTime;
		if (rend.enabled)
		{
			rend.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
		}
	}
}
