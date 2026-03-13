namespace Kampai.Util.Graphics
{
	public static class MaterialPropertyExtensions
	{
		private static bool IsValidProperty(global::UnityEngine.Material material, int propertyHash, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties property, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties propertyGroup)
		{
			return material != null && (property & propertyGroup) != 0 && material.HasProperty(propertyHash);
		}

		public static void SetRenderQueueProperty(this global::UnityEngine.Material material, global::Kampai.Util.Graphics.BlendMode blendMode, int layerIndex = 0)
		{
			if (!(material == null))
			{
				int propertyHash = global::Kampai.Util.Graphics.ShaderUtils.GetPropertyHash(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.Mode);
				material.SetFloat(propertyHash, (float)blendMode);
				propertyHash = global::Kampai.Util.Graphics.ShaderUtils.GetPropertyHash(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.LayerIndex);
				material.SetFloat(propertyHash, layerIndex);
				material.renderQueue = global::Kampai.Util.Graphics.ShaderUtils.ConvertShaderBlendToRenderQueueValue(blendMode) + layerIndex;
			}
		}

		public static void SetProperty(this global::UnityEngine.Material material, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties property, float value)
		{
			int propertyHash = global::Kampai.Util.Graphics.ShaderUtils.GetPropertyHash(property);
			if (IsValidProperty(material, propertyHash, property, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.FloatValues))
			{
				material.SetFloat(propertyHash, value);
			}
		}

		public static void SetProperty(this global::UnityEngine.Material material, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties property, global::Kampai.Util.Graphics.ColorMask value)
		{
			int propertyHash = global::Kampai.Util.Graphics.ShaderUtils.GetPropertyHash(property);
			if (IsValidProperty(material, propertyHash, property, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.ColorMask))
			{
				material.SetFloat(propertyHash, (float)value);
			}
		}

		public static void SetProperty(this global::UnityEngine.Material material, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties property, global::UnityEngine.Rendering.BlendOp value)
		{
			int propertyHash = global::Kampai.Util.Graphics.ShaderUtils.GetPropertyHash(property);
			if (IsValidProperty(material, propertyHash, property, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.BlendOperation))
			{
				material.SetFloat(propertyHash, (float)value);
			}
		}

		public static void SetProperty(this global::UnityEngine.Material material, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties property, global::UnityEngine.Rendering.CullMode value)
		{
			int propertyHash = global::Kampai.Util.Graphics.ShaderUtils.GetPropertyHash(property);
			if (IsValidProperty(material, propertyHash, property, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.Cull))
			{
				material.SetFloat(propertyHash, (float)value);
			}
		}

		public static void SetProperty(this global::UnityEngine.Material material, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties property, global::UnityEngine.Rendering.CompareFunction value)
		{
			int propertyHash = global::Kampai.Util.Graphics.ShaderUtils.GetPropertyHash(property);
			if (IsValidProperty(material, propertyHash, property, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilComp))
			{
				material.SetFloat(propertyHash, (float)value);
			}
		}

		public static void SetProperty(this global::UnityEngine.Material material, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties property, global::Kampai.Util.Graphics.StencilOperation value)
		{
			int propertyHash = global::Kampai.Util.Graphics.ShaderUtils.GetPropertyHash(property);
			if (IsValidProperty(material, propertyHash, property, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilOperation))
			{
				material.SetFloat(propertyHash, (float)value);
			}
		}

		public static void SetProperty(this global::UnityEngine.Material material, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties property, global::UnityEngine.Color value)
		{
			int propertyHash = global::Kampai.Util.Graphics.ShaderUtils.GetPropertyHash(property);
			if (IsValidProperty(material, propertyHash, property, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.ColorValues))
			{
				material.SetColor(propertyHash, value);
			}
		}

		public static void SetProperty(this global::UnityEngine.Material material, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties property, global::UnityEngine.Texture value)
		{
			int propertyHash = global::Kampai.Util.Graphics.ShaderUtils.GetPropertyHash(property);
			if (IsValidProperty(material, propertyHash, property, global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.TextureValues))
			{
				material.SetTexture(propertyHash, value);
			}
		}
	}
}
