namespace Kampai.Util
{
	internal sealed class FadeMaterialBlock : global::UnityEngine.MonoBehaviour
	{
		private const int ClearCacheTimeSeconds = 10;

		private global::UnityEngine.MaterialPropertyBlock propertyBlock;

		private float m_fadeAlpha = 1f;

		private readonly global::System.Collections.Generic.IList<global::UnityEngine.Renderer> m_renderers = new global::System.Collections.Generic.List<global::UnityEngine.Renderer>();

		private readonly global::System.Collections.Generic.Dictionary<global::UnityEngine.ParticleSystem, bool> m_particleSystems = new global::System.Collections.Generic.Dictionary<global::UnityEngine.ParticleSystem, bool>();

		private readonly global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::UnityEngine.Material[]> m_materialCache = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::UnityEngine.Material[]>();

		private readonly global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::UnityEngine.Material[]> m_fadeMaterialCache = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::UnityEngine.Material[]>();

		private readonly global::System.Collections.Generic.List<global::UnityEngine.Material> m_fadeMaterials = new global::System.Collections.Generic.List<global::UnityEngine.Material>();

		private bool fadeIn;

		private GoTween m_fadeTween;

		public float zWriteOffValue = 0.15f;

		private global::UnityEngine.Coroutine m_clearCacheCoroutine;

		public float FadeAlpha
		{
			get
			{
				return m_fadeAlpha;
			}
			set
			{
				m_fadeAlpha = value;
				propertyBlock.Clear();
				propertyBlock.SetFloat(global::Kampai.Util.GameConstants.ShaderProperties.Alpha, 0f);
				propertyBlock.SetFloat(global::Kampai.Util.GameConstants.ShaderProperties.Procedural.FadeAlpha, value);
				global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::UnityEngine.Material[]> dictionary = ((!(m_fadeAlpha <= zWriteOffValue)) ? m_materialCache : m_fadeMaterialCache);
				int num = 0;
				while (num < m_renderers.Count)
				{
					global::UnityEngine.Renderer renderer = m_renderers[num];
					if (renderer != null)
					{
						renderer.materials = dictionary[renderer];
						renderer.SetPropertyBlock(propertyBlock);
						num++;
					}
					else
					{
						m_renderers.RemoveAt(num);
					}
				}
			}
		}

		private void Awake()
		{
			propertyBlock = new global::UnityEngine.MaterialPropertyBlock();
		}

		private void OnDestroy()
		{
			Clear();
		}

		public void StartFade(bool fadeIn, float duration, global::System.Collections.Generic.List<global::UnityEngine.Renderer> renderers)
		{
			this.fadeIn = fadeIn;
			if (!fadeIn)
			{
				m_fadeAlpha = 1f;
				SetRenderers(renderers);
			}
			if (m_fadeTween != null)
			{
				m_fadeTween.playBackwards();
				return;
			}
			if (m_clearCacheCoroutine != null)
			{
				StopCoroutine(m_clearCacheCoroutine);
				m_clearCacheCoroutine = null;
			}
			m_fadeTween = Go.to(this, duration, new GoTweenConfig().floatProp("FadeAlpha", (!fadeIn) ? 0f : 1f).onComplete(OnCompleteTween));
		}

		private bool SetRenderers(global::System.Collections.Generic.List<global::UnityEngine.Renderer> renderers)
		{
			if (renderers == null || renderers.Count == 0)
			{
				return false;
			}
			Clear();
			for (int i = 0; i < renderers.Count; i++)
			{
				global::UnityEngine.Renderer renderer = renderers[i];
				global::UnityEngine.ParticleSystemRenderer particleSystemRenderer = renderer as global::UnityEngine.ParticleSystemRenderer;
				if (particleSystemRenderer != null)
				{
					global::UnityEngine.ParticleSystem component = particleSystemRenderer.gameObject.GetComponent<global::UnityEngine.ParticleSystem>();
					if (component != null && !m_particleSystems.ContainsKey(component))
					{
						m_particleSystems.Add(component, component.emission.enabled);
						global::UnityEngine.UnityUtils.SetEmissionEnabled(component, false);
					}
				}
				else
				{
					m_renderers.Add(renderer);
					if (!m_materialCache.ContainsKey(renderer))
					{
						CheckIfRendererFades(renderer);
					}
				}
			}
			return true;
		}

		private void CheckIfRendererFades(global::UnityEngine.Renderer renderer)
		{
			m_materialCache.Add(renderer, renderer.sharedMaterials);
			global::UnityEngine.Material[] array = new global::UnityEngine.Material[renderer.sharedMaterials.Length];
			for (int i = 0; i < renderer.sharedMaterials.Length; i++)
			{
				global::UnityEngine.Material material = renderer.sharedMaterials[i];
				if (material != null && material.HasProperty(global::Kampai.Util.GameConstants.ShaderProperties.ZWrite) && material.HasProperty(global::Kampai.Util.GameConstants.ShaderProperties.Procedural.FadeAlpha))
				{
					global::UnityEngine.Material material2 = new global::UnityEngine.Material(renderer.sharedMaterials[i]);
					string[] shaderKeywords = material.shaderKeywords;
					foreach (string keyword in shaderKeywords)
					{
						material2.EnableKeyword(keyword);
					}
					material2.SetFloat(global::Kampai.Util.GameConstants.ShaderProperties.ZWrite, 0f);
					material = material2;
					m_fadeMaterials.Add(material2);
				}
				if (material != null)
				{
					array[i] = material;
				}
			}
			m_fadeMaterialCache.Add(renderer, array);
		}

		public void Clear()
		{
			if (m_renderers.Count == 0 && m_particleSystems.Count == 0)
			{
				return;
			}
			if (m_fadeTween != null)
			{
				m_fadeTween.complete();
			}
			for (int i = 0; i < m_renderers.Count; i++)
			{
				global::UnityEngine.Renderer renderer = m_renderers[i];
				if (renderer != null)
				{
					renderer.materials = m_materialCache[renderer];
					renderer.SetPropertyBlock(null);
				}
			}
			foreach (global::System.Collections.Generic.KeyValuePair<global::UnityEngine.ParticleSystem, bool> particleSystem in m_particleSystems)
			{
				if (particleSystem.Key != null)
				{
					global::UnityEngine.UnityUtils.SetEmissionEnabled(particleSystem.Key, particleSystem.Value);
				}
			}
			m_particleSystems.Clear();
			m_renderers.Clear();
		}

		private void OnCompleteTween(AbstractGoTween thisTween)
		{
			m_fadeTween.destroy();
			m_fadeTween = null;
			if (fadeIn)
			{
				Clear();
				if (m_clearCacheCoroutine != null)
				{
					StopCoroutine(m_clearCacheCoroutine);
					m_clearCacheCoroutine = null;
				}
				m_clearCacheCoroutine = StartCoroutine(ClearCache());
			}
		}

		private global::System.Collections.IEnumerator ClearCache()
		{
			yield return new global::UnityEngine.WaitForSeconds(10f);
			if (m_fadeMaterialCache.Count > 0 && m_materialCache.Count > 0 && fadeIn && m_fadeTween == null)
			{
				for (int i = 0; i < m_fadeMaterials.Count; i++)
				{
					global::UnityEngine.Object.DestroyImmediate(m_fadeMaterials[i], true);
				}
				m_fadeMaterials.Clear();
				m_fadeMaterialCache.Clear();
				m_materialCache.Clear();
			}
		}
	}
}
