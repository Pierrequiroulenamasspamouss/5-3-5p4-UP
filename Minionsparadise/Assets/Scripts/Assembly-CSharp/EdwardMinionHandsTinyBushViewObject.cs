public class EdwardMinionHandsTinyBushViewObject : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.GameObject ParticleVFXPrefab;

	public global::UnityEngine.Animation ShakeEffectAnimation;

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(ParticleVFXPrefab);
		gameObject.transform.SetParent(base.transform, false);
		global::UnityEngine.Vector3 position = base.transform.position;
		position.y += 1f;
		gameObject.transform.position = position;
		global::UnityEngine.Object.Destroy(gameObject, 5f);
		ShakeEffectAnimation.Play();
	}
}
