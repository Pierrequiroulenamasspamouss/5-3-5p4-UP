namespace Kampai.Util
{
	public class ParticleInstantiationScript : global::UnityEngine.MonoBehaviour
	{
		private void Start()
		{
			PlayerParticle("fx_smokePoofTest");
		}

		public void PlayerParticle(string path)
		{
			global::UnityEngine.Object.Instantiate(global::UnityEngine.Resources.Load("VFX/FTEE/" + path));
		}

		private void Update()
		{
		}
	}
}
