namespace Kampai.Game.View
{
	public class LandExpansionBuildingObject : global::Kampai.Game.View.BuildingObject
	{
		private global::Kampai.Game.BurnLandExpansionSignal burnSignal;

		private string vfxGrassClearing;

		private string burntPrefab;

		private global::UnityEngine.ParticleSystem grassClearingParticles;

		private float seconds = 3f;

		public float BurnTimer { get; set; }

		internal void Burn(global::Kampai.Game.BurnLandExpansionSignal burnSignal, int ID, string vfxGrassClearing)
		{
			this.burnSignal = burnSignal;
			this.ID = ID;
			this.vfxGrassClearing = vfxGrassClearing;
			StartCoroutine(BurnSequence());
		}

		private global::System.Collections.IEnumerator BurnSequence()
		{
			IncrementMaterialRenderQueue(1);
			Go.to(this, seconds, new GoTweenConfig().floatProp("BurnTimer", 0.5f).setEaseType(GoEaseType.Linear).onUpdate(delegate
			{
				for (int i = 0; i < base.objectRenderers.Length; i++)
				{
					global::UnityEngine.Material[] materials = base.objectRenderers[i].materials;
					for (int j = 0; j < materials.Length; j++)
					{
						bool flag = false;
						string[] shaderKeywords = materials[j].shaderKeywords;
						for (int k = 0; k < shaderKeywords.Length; k++)
						{
							if ("ALPHA_CLIP".Equals(shaderKeywords[k]))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							materials[j].SetFloat("_AlphaClip", BurnTimer);
						}
						else
						{
							materials[j].color = new global::UnityEngine.Color(1f, 1f, 1f, 1f - 4f * BurnTimer);
						}
					}
				}
			}));
			global::UnityEngine.GameObject clearingGO = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(vfxGrassClearing));
			clearingGO.transform.parent = base.transform;
			clearingGO.transform.localPosition = new global::UnityEngine.Vector3(1f, 1f, -1f);
			grassClearingParticles = clearingGO.GetComponent<global::UnityEngine.ParticleSystem>();
			yield return new global::UnityEngine.WaitForSeconds(1f);
			grassClearingParticles.Stop();
			yield return new global::UnityEngine.WaitForSeconds(seconds);
			burnSignal.Dispatch(ID);
		}

		public override void SetMaterialColor(global::UnityEngine.Color color)
		{
			for (int i = 0; i < base.objectRenderers.Length; i++)
			{
				global::UnityEngine.Renderer renderer = base.objectRenderers[i];
				global::UnityEngine.Material[] materials = renderer.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					materials[j].color = color;
				}
			}
		}

		public override void SetMaterialShaderFloat(string name, float value)
		{
			for (int i = 0; i < base.objectRenderers.Length; i++)
			{
				global::UnityEngine.Material[] materials = base.objectRenderers[i].materials;
				for (int j = 0; j < materials.Length; j++)
				{
					materials[j].SetFloat(name, value);
				}
			}
		}

		public override void IncrementMaterialRenderQueue(int delta)
		{
			for (int i = 0; i < base.objectRenderers.Length; i++)
			{
				global::UnityEngine.Material[] materials = base.objectRenderers[i].materials;
				for (int j = 0; j < materials.Length; j++)
				{
					materials[j].renderQueue += delta;
				}
			}
		}
	}
}
