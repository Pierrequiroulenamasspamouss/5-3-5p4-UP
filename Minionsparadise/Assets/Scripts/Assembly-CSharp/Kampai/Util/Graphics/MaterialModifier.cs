namespace Kampai.Util.Graphics
{
	public class MaterialModifier
	{
		public static readonly global::System.Collections.Generic.List<int> FixedFuncProperties = new global::System.Collections.Generic.List<int>
		{
			global::Kampai.Util.GameConstants.ShaderProperties.ZWrite,
			global::Kampai.Util.GameConstants.ShaderProperties.Cull,
			global::Kampai.Util.GameConstants.ShaderProperties.ZTest,
			global::Kampai.Util.GameConstants.ShaderProperties.Blend.DstBlend,
			global::Kampai.Util.GameConstants.ShaderProperties.Blend.SrcBlend,
			global::Kampai.Util.GameConstants.ShaderProperties.Color.ColorMask,
			global::Kampai.Util.GameConstants.ShaderProperties.Stencil.Ref
		};

		private global::System.Collections.Generic.Dictionary<int, float> m_floatParams;

		private global::System.Collections.Generic.Dictionary<int, global::UnityEngine.Color> m_colorParams;

		private global::System.Collections.Generic.Dictionary<int, global::UnityEngine.Texture> m_textureParams;

		private global::System.Collections.Generic.Dictionary<int, global::UnityEngine.Vector4> m_vectorParams;

		protected global::System.Collections.Generic.Dictionary<int, float> m_fixedFunctionParams;

		private global::UnityEngine.MaterialPropertyBlock m_propertyBlock;

		private bool m_isDirty;

		private readonly global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::UnityEngine.Material[]> m_materialCache = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::UnityEngine.Material[]>();

		private global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::UnityEngine.Material[]> m_instancedMaterialCache;

		private global::System.Collections.Generic.List<global::UnityEngine.Renderer> m_targetRenderers;

		public int PropertyCount { get; private set; }

		public MaterialModifier(global::System.Collections.Generic.IEnumerable<global::UnityEngine.Renderer> renderers)
		{
			m_targetRenderers = new global::System.Collections.Generic.List<global::UnityEngine.Renderer>();
			foreach (global::UnityEngine.Renderer renderer in renderers)
			{
				m_targetRenderers.Add(renderer);
				m_materialCache.Add(renderer, renderer.sharedMaterials);
			}
			m_propertyBlock = new global::UnityEngine.MaterialPropertyBlock();
		}

		public bool IsEmpty()
		{
			return PropertyCount == 0;
		}

		public global::Kampai.Util.Graphics.MaterialModifier SetFloat(int parameterHash, float value)
		{
			if (FixedFuncProperties.Contains(parameterHash))
			{
				m_fixedFunctionParams = m_fixedFunctionParams ?? new global::System.Collections.Generic.Dictionary<int, float>();
				SetValue(m_fixedFunctionParams, value, parameterHash);
				return this;
			}
			m_floatParams = m_floatParams ?? new global::System.Collections.Generic.Dictionary<int, float>();
			SetValue(m_floatParams, value, parameterHash);
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier SetColor(int parameterHash, global::UnityEngine.Color value)
		{
			m_colorParams = m_colorParams ?? new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.Color>();
			SetValue(m_colorParams, value, parameterHash);
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier SetTexture(int parameterHash, global::UnityEngine.Texture value)
		{
			m_textureParams = m_textureParams ?? new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.Texture>();
			SetValue(m_textureParams, value, parameterHash);
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier SetVector(int parameterHash, global::UnityEngine.Vector4 value)
		{
			m_vectorParams = m_vectorParams ?? new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.Vector4>();
			SetValue(m_vectorParams, value, parameterHash);
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier SetFloat(string paramName, float value)
		{
			return SetFloat(global::UnityEngine.Shader.PropertyToID(paramName), value);
		}

		public global::Kampai.Util.Graphics.MaterialModifier SetColor(string paramName, global::UnityEngine.Color value)
		{
			return SetColor(global::UnityEngine.Shader.PropertyToID(paramName), value);
		}

		public global::Kampai.Util.Graphics.MaterialModifier SetTexture(string paramName, global::UnityEngine.Texture value)
		{
			return SetTexture(global::UnityEngine.Shader.PropertyToID(paramName), value);
		}

		public global::Kampai.Util.Graphics.MaterialModifier SetVector(string paramName, global::UnityEngine.Vector4 value)
		{
			return SetVector(global::UnityEngine.Shader.PropertyToID(paramName), value);
		}

		private void SetValue<T>(global::System.Collections.Generic.Dictionary<int, T> dictionary, T value, int paramHash)
		{
			if (dictionary.ContainsKey(paramHash))
			{
				m_isDirty = true;
				dictionary[paramHash] = value;
			}
			else
			{
				m_isDirty = true;
				dictionary.Add(paramHash, value);
				PropertyCount++;
			}
		}

		public bool HasFloat(int parameterHash)
		{
			return HasFixedFunction(parameterHash) || (m_floatParams != null && m_floatParams.ContainsKey(parameterHash));
		}

		private bool HasFixedFunction(int parameterHash)
		{
			return m_fixedFunctionParams != null && m_fixedFunctionParams.ContainsKey(parameterHash);
		}

		public bool HasColor(int parameterHash)
		{
			return m_colorParams != null && m_colorParams.ContainsKey(parameterHash);
		}

		public bool HasTexture(int parameterHash)
		{
			return m_textureParams != null && m_textureParams.ContainsKey(parameterHash);
		}

		public bool HasVector(int parameterHash)
		{
			return m_vectorParams != null && m_vectorParams.ContainsKey(parameterHash);
		}

		public bool HasFloat(string paramName)
		{
			return m_floatParams != null && m_floatParams.ContainsKey(global::UnityEngine.Shader.PropertyToID(paramName));
		}

		public bool HasColor(string paramName)
		{
			return m_colorParams != null && m_colorParams.ContainsKey(global::UnityEngine.Shader.PropertyToID(paramName));
		}

		public bool HasTexture(string paramName)
		{
			return m_textureParams != null && m_textureParams.ContainsKey(global::UnityEngine.Shader.PropertyToID(paramName));
		}

		public bool HasVector(string paramName)
		{
			return m_vectorParams != null && m_vectorParams.ContainsKey(global::UnityEngine.Shader.PropertyToID(paramName));
		}

		public float GetFloat(int parameterHash)
		{
			return GetValue((!HasFixedFunction(parameterHash)) ? m_floatParams : m_fixedFunctionParams, parameterHash);
		}

		public global::UnityEngine.Color GetColor(int parameterHash)
		{
			return GetValue(m_colorParams, parameterHash);
		}

		public global::UnityEngine.Texture GetTexture(int parameterHash)
		{
			return GetValue(m_textureParams, parameterHash);
		}

		public global::UnityEngine.Vector4 GetVector(int parameterHash)
		{
			return GetValue(m_vectorParams, parameterHash);
		}

		public float GetFloat(string paramName)
		{
			return GetFloat(global::UnityEngine.Shader.PropertyToID(paramName));
		}

		public global::UnityEngine.Color GetColor(string paramName)
		{
			return GetColor(global::UnityEngine.Shader.PropertyToID(paramName));
		}

		public global::UnityEngine.Texture GetTexture(string paramName)
		{
			return GetTexture(global::UnityEngine.Shader.PropertyToID(paramName));
		}

		public global::UnityEngine.Vector4 GetVector(string paramName)
		{
			return GetVector(global::UnityEngine.Shader.PropertyToID(paramName));
		}

		private static T GetValue<T>(global::System.Collections.Generic.Dictionary<int, T> dictionary, int parameterHash)
		{
			if (dictionary == null || !dictionary.ContainsKey(parameterHash))
			{
				return default(T);
			}
			return dictionary[parameterHash];
		}

		public global::Kampai.Util.Graphics.MaterialModifier RemoveFloat(int parameterHash)
		{
			RemoveValue((!HasFixedFunction(parameterHash)) ? m_floatParams : m_fixedFunctionParams, parameterHash);
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier RemoveColor(int parameterHash)
		{
			RemoveValue(m_colorParams, parameterHash);
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier RemoveTexture(int parameterHash)
		{
			RemoveValue(m_textureParams, parameterHash);
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier RemoveVector(int parameterHash)
		{
			RemoveValue(m_vectorParams, parameterHash);
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier RemoveFloat(string paramName)
		{
			RemoveValue(m_floatParams, global::UnityEngine.Shader.PropertyToID(paramName));
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier RemoveColor(string paramName)
		{
			RemoveValue(m_colorParams, global::UnityEngine.Shader.PropertyToID(paramName));
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier RemoveTexture(string paramName)
		{
			RemoveValue(m_textureParams, global::UnityEngine.Shader.PropertyToID(paramName));
			return this;
		}

		public global::Kampai.Util.Graphics.MaterialModifier RemoveVector(string paramName)
		{
			RemoveValue(m_vectorParams, global::UnityEngine.Shader.PropertyToID(paramName));
			return this;
		}

		private void RemoveValue<T>(global::System.Collections.Generic.Dictionary<int, T> dictionary, int parameterHash)
		{
			if (dictionary.ContainsKey(parameterHash))
			{
				m_isDirty = true;
				PropertyCount--;
				dictionary.Remove(parameterHash);
				if (dictionary.Count == 0)
				{
					dictionary.Clear();
					dictionary = null;
				}
			}
		}

		public void Update()
		{
			if (m_targetRenderers != null && m_isDirty)
			{
				m_propertyBlock.Clear();
				UpdateFixedFunctions();
				UpdateParams(m_floatParams, m_propertyBlock.SetFloat);
				UpdateParams(m_colorParams, m_propertyBlock.SetColor);
				UpdateParams(m_textureParams, m_propertyBlock.SetTexture);
				UpdateParams(m_vectorParams, m_propertyBlock.SetVector);
				for (int i = 0; i < m_targetRenderers.Count; i++)
				{
					m_targetRenderers[i].SetPropertyBlock(m_propertyBlock);
				}
				m_isDirty = false;
			}
		}

		public void Destroy()
		{
			Reset();
			m_propertyBlock.Clear();
			m_targetRenderers = null;
		}

		public global::Kampai.Util.Graphics.MaterialModifier Reset()
		{
			ResetRenderers();
			CleanInstancedMaterials();
			ClearParams(m_floatParams);
			ClearParams(m_colorParams);
			ClearParams(m_textureParams);
			ClearParams(m_vectorParams);
			return this;
		}

		private void ClearParams<T>(global::System.Collections.Generic.Dictionary<int, T> dict)
		{
			if (dict != null)
			{
				PropertyCount -= dict.Count;
				dict.Clear();
			}
		}

		private void ResetRenderers()
		{
			if (m_targetRenderers != null)
			{
				for (int i = 0; i < m_targetRenderers.Count; i++)
				{
					global::UnityEngine.Renderer renderer = m_targetRenderers[i];
					renderer.materials = m_materialCache[renderer];
					renderer.SetPropertyBlock(null);
				}
			}
		}

		private void CleanInstancedMaterials()
		{
			if (m_instancedMaterialCache == null || m_instancedMaterialCache.Count <= 0 || m_materialCache.Count <= 0)
			{
				return;
			}
			foreach (global::System.Collections.Generic.KeyValuePair<global::UnityEngine.Renderer, global::UnityEngine.Material[]> item in m_instancedMaterialCache)
			{
				if (item.Value != null)
				{
					for (int i = 0; i < item.Value.Length; i++)
					{
						global::UnityEngine.Object.DestroyImmediate(item.Value[i], true);
					}
				}
			}
			m_instancedMaterialCache.Clear();
			m_instancedMaterialCache = null;
		}

		private void UpdateFixedFunctions()
		{
			if (m_fixedFunctionParams == null || m_fixedFunctionParams.Count == 0)
			{
				return;
			}
			if (m_instancedMaterialCache == null)
			{
				m_instancedMaterialCache = new global::System.Collections.Generic.Dictionary<global::UnityEngine.Renderer, global::UnityEngine.Material[]>();
				foreach (global::System.Collections.Generic.KeyValuePair<global::UnityEngine.Renderer, global::UnityEngine.Material[]> item in m_materialCache)
				{
					global::UnityEngine.Renderer key = item.Key;
					global::UnityEngine.Material[] value = item.Value;
					global::UnityEngine.Material[] array = new global::UnityEngine.Material[value.Length];
					for (int i = 0; i < value.Length; i++)
					{
						array[i] = new global::UnityEngine.Material(value[i]);
					}
					key.materials = array;
					m_instancedMaterialCache.Add(item.Key, array);
				}
			}
			using (global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<int, float>> enumerator2 = (global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<int, float>>)m_fixedFunctionParams.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					global::System.Collections.Generic.KeyValuePair<int, float> current2 = enumerator2.Current;
					foreach (global::UnityEngine.Material[] value2 in m_instancedMaterialCache.Values)
					{
						for (int j = 0; j < value2.Length; j++)
						{
							value2[j].SetFloat(current2.Key, current2.Value);
						}
					}
				}
			}
		}

		private void UpdateParams<T>(global::System.Collections.Generic.Dictionary<int, T> dictionary, global::System.Action<int, T> updateValueAction)
		{
			if (dictionary == null || dictionary.Count == 0)
			{
				return;
			}
			using (global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<int, T>> enumerator = (global::System.Collections.Generic.IEnumerator<global::System.Collections.Generic.KeyValuePair<int, T>>)dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					global::System.Collections.Generic.KeyValuePair<int, T> current = enumerator.Current;
					updateValueAction(current.Key, current.Value);
				}
			}
		}
	}
}
