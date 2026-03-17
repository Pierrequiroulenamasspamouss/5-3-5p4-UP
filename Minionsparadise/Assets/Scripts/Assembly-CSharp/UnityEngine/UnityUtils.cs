namespace UnityEngine
{
	public static class UnityUtils
	{
		public static void SetEmissionEnabled(this global::UnityEngine.ParticleSystem ps, bool enabled)
		{
			global::UnityEngine.ParticleSystem.EmissionModule emission = ps.emission;
			emission.enabled = enabled;
		}
	}
}
