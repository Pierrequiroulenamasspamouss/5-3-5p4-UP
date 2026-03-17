namespace Kampai.Game.Mignette.ButterflyCatch.View
{
	public class ButterflyCatchBuildingViewObject : global::Kampai.Game.Mignette.View.MignetteBuildingViewObject
	{
		public global::UnityEngine.GameObject ButterflyParticleParent;

		public float YOffset = 1f;

		private global::UnityEngine.ParticleSystem[] AmbientButterflies;

		public void Start()
		{
			AmbientButterflies = ButterflyParticleParent.GetComponentsInChildren<global::UnityEngine.ParticleSystem>();
			ToggleAmbientButterflies(true);
			base.gameObject.AddComponent<global::Kampai.Game.Mignette.View.MignetteBuildingCooldownView>();
			global::UnityEngine.Vector3 position = base.gameObject.transform.position;
			base.gameObject.transform.position = new global::UnityEngine.Vector3(position.x, position.y + YOffset, position.z);
		}

		public override void ResetCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal)
		{
			ToggleAmbientButterflies(true);
		}

		public override void UpdateCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal, int buildingData, float pctDone)
		{
			if (pctDone < 1f)
			{
				ToggleAmbientButterflies(false);
			}
		}

		public void ToggleAmbientButterflies(bool enable)
		{
			if (AmbientButterflies != null)
			{
				global::UnityEngine.ParticleSystem[] ambientButterflies = AmbientButterflies;
				foreach (global::UnityEngine.ParticleSystem ps in ambientButterflies)
				{
					global::UnityEngine.UnityUtils.SetEmissionEnabled(ps, enable);
				}
			}
		}
	}
}
