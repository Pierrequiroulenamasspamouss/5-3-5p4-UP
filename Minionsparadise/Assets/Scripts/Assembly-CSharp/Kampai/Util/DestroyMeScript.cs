namespace Kampai.Util
{
	public class DestroyMeScript : global::UnityEngine.MonoBehaviour
	{
		public float DestroyTime;

		private void Start()
		{
			StartCoroutine("DestroyMe");
		}

		private global::System.Collections.IEnumerator DestroyMe()
		{
			yield return new global::UnityEngine.WaitForSeconds(DestroyTime);
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
