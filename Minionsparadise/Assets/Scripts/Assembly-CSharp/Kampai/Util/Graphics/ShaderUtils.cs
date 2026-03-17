namespace Kampai.Util.Graphics
{
	public static class ShaderUtils
	{
		[global::System.Flags]
		public enum ShaderProperties
		{
			Mode = 1,
			LayerIndex = 2,
			MainTexture = 4,
			AlphaTexture = 8,
			WaterTexture = 0x10,
			UVScroll = 0x20,
			MainColor = 0x40,
			AlphaChannel = 0x80,
			ColorMask = 0x100,
			Boost = 0x200,
			VertexColor = 0x400,
			BlendedColor = 0x800,
			FadeAlpha = 0x1000,
			AlphaTex = 0x2000,
			Overlay = 0x4000,
			Desaturation = 0x8000,
			StencilRef = 0x10000,
			StencilComp = 0x20000,
			StencilReadMask = 0x40000,
			StencilWriteMask = 0x80000,
			StencilPassOp = 0x100000,
			StencilFailOp = 0x200000,
			StencilZFailOp = 0x400000,
			ZWrite = 0x800000,
			ZTest = 0x1000000,
			Cull = 0x2000000,
			AlphaClip = 0x4000000,
			Saturation = 0x8000000,
			Alpha = 0x10000000,
			DstBlend = 0x20000000,
			SrcBlend = 0x40000000,
			TextureValues = 0x201C,
			FloatValues = 0x1F8D9603,
			VectorValues = 0x20,
			ColorValues = 0x48C0,
			StencilOperation = 0x700000,
			BlendOperation = 0x60000000,
			CompareFunc = 0x20000,
			CullMode = 0x2000000
		}

		public static int ConvertShaderBlendToRenderQueueValue(global::Kampai.Util.Graphics.BlendMode mode)
		{
			return (int)ConvertShaderBlendToRenderQueue(mode);
		}

		public static global::Kampai.Util.Graphics.RenderQueue ConvertShaderBlendToRenderQueue(global::Kampai.Util.Graphics.BlendMode mode)
		{
			global::Kampai.Util.Graphics.RenderQueue result = global::Kampai.Util.Graphics.RenderQueue.Geometry;
			switch (mode)
			{
			case global::Kampai.Util.Graphics.BlendMode.Background:
				result = global::Kampai.Util.Graphics.RenderQueue.Background;
				break;
			case global::Kampai.Util.Graphics.BlendMode.Geometry:
				result = global::Kampai.Util.Graphics.RenderQueue.Geometry;
				break;
			case global::Kampai.Util.Graphics.BlendMode.AlphaTest:
				result = global::Kampai.Util.Graphics.RenderQueue.AlphaTest;
				break;
			case global::Kampai.Util.Graphics.BlendMode.Transparent:
				result = global::Kampai.Util.Graphics.RenderQueue.Transparent;
				break;
			case global::Kampai.Util.Graphics.BlendMode.Overlay:
				result = global::Kampai.Util.Graphics.RenderQueue.Overlay;
				break;
			}
			return result;
		}

		public static int GetMaterialRenderQueue(global::UnityEngine.Material material)
		{
			if (material == null)
			{
				return 0;
			}
			int num = -1;
			if (material.HasProperty("_Mode"))
			{
				num = (int)ConvertShaderBlendToRenderQueue((global::Kampai.Util.Graphics.BlendMode)material.GetFloat("_Mode"));
				if (material.HasProperty("_LayerIndex"))
				{
					num += (int)material.GetFloat("_LayerIndex");
				}
			}
			return num;
		}

		public static void EnableStencilShader(global::UnityEngine.Material material, int stencilRef, int count, global::Kampai.Util.Graphics.StencilOperation passOp = global::Kampai.Util.Graphics.StencilOperation.Replace, global::Kampai.Util.Graphics.StencilOperation failOp = global::Kampai.Util.Graphics.StencilOperation.Keep, global::Kampai.Util.Graphics.StencilOperation zFailOp = global::Kampai.Util.Graphics.StencilOperation.Keep)
		{
			material.SetProperty(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilRef, stencilRef);
			material.SetProperty(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilComp, global::UnityEngine.Rendering.CompareFunction.Equal);
			material.SetProperty(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilReadMask, stencilRef);
			material.SetProperty(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilWriteMask, stencilRef);
			material.SetProperty(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilPassOp, passOp);
			material.SetProperty(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilFailOp, failOp);
			material.SetProperty(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilZFailOp, zFailOp);
			material.SetRenderQueueProperty(global::Kampai.Util.Graphics.BlendMode.Transparent, count);
			material.SetProperty(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.Alpha, 0f);
		}

		public static int GetPropertyHash(global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties propertyName)
		{
			int result = 0;
			switch (propertyName)
			{
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.Mode:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Queue.Mode;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.LayerIndex:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Queue.LayerIndex;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.MainTexture:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Texture.MainTexture;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.AlphaTexture:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Texture.AlphaTexture;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.WaterTexture:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Texture.WaterTexture;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.UVScroll:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Texture.UVScroll;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.AlphaTex:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Texture.AlphaTexture;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.MainColor:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Color.MainColor;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.AlphaChannel:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Color.AlphaChannel;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.ColorMask:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Color.ColorMask;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.Boost:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Color.Boost;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.VertexColor:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Color.VertexColor;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.BlendedColor:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Procedural.BlendedColor;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.FadeAlpha:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Procedural.FadeAlpha;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.Overlay:
				result = global::Kampai.Util.GameConstants.ShaderProperties.UI.Overlay;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.Desaturation:
				result = global::Kampai.Util.GameConstants.ShaderProperties.UI.Desaturation;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilRef:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Stencil.Ref;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilComp:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Stencil.Comp;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilReadMask:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Stencil.ReadMask;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilWriteMask:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Stencil.WriteMask;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilPassOp:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Stencil.PassOp;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilFailOp:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Stencil.FailOp;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.StencilZFailOp:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Stencil.ZFailOp;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.ZWrite:
				result = global::Kampai.Util.GameConstants.ShaderProperties.ZWrite;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.ZTest:
				result = global::Kampai.Util.GameConstants.ShaderProperties.ZTest;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.Cull:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Cull;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.AlphaClip:
				result = global::Kampai.Util.GameConstants.ShaderProperties.AlphaClip;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.Saturation:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Saturation;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.Alpha:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Alpha;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.DstBlend:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Blend.DstBlend;
				break;
			case global::Kampai.Util.Graphics.ShaderUtils.ShaderProperties.SrcBlend:
				result = global::Kampai.Util.GameConstants.ShaderProperties.Blend.SrcBlend;
				break;
			}
			return result;
		}
	}
}
