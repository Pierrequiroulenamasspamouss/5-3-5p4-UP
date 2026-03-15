public class MinionMoveIndicator : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Animation indicatorAnimation;

	public global::UnityEngine.MeshRenderer meshRenderer;

	private void Start()
	{
		indicatorAnimation.Play();
		meshRenderer.enabled = true;
	}
}
