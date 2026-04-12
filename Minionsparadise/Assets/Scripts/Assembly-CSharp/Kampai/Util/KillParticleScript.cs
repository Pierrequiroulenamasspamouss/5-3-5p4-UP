namespace Kampai.Util
{
	public class KillParticleScript : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.ParticleSystem ps;

		public void Update()
		{
			if ((bool)ps && !ps.isPlaying)
			{
				global::UnityEngine.Object.Destroy(base.transform.parent.gameObject);
			}
		}
	}
}
