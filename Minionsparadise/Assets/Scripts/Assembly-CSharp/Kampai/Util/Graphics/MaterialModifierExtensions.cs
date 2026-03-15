namespace Kampai.Util.Graphics
{
	public static class MaterialModifierExtensions
	{
		public static global::Kampai.Util.Graphics.MaterialModifier SetAlpha(this global::Kampai.Util.Graphics.MaterialModifier modifier, float alpha, bool update = true)
		{
			modifier.SetFloat(global::Kampai.Util.GameConstants.ShaderProperties.Alpha, alpha);
			if (update)
			{
				modifier.Update();
			}
			return modifier;
		}

		public static global::Kampai.Util.Graphics.MaterialModifier SetFadeAlpha(this global::Kampai.Util.Graphics.MaterialModifier modifier, float fadeAlpha, bool update = true)
		{
			modifier.SetFloat(global::Kampai.Util.GameConstants.ShaderProperties.Procedural.FadeAlpha, fadeAlpha);
			if (update)
			{
				modifier.Update();
			}
			return modifier;
		}

		public static global::Kampai.Util.Graphics.MaterialModifier SetMaterialColor(this global::Kampai.Util.Graphics.MaterialModifier modifier, global::UnityEngine.Color color, bool update = true)
		{
			modifier.SetColor(global::Kampai.Util.GameConstants.ShaderProperties.Color.MainColor, color);
			if (update)
			{
				modifier.Update();
			}
			return modifier;
		}

		public static global::Kampai.Util.Graphics.MaterialModifier SetBlendedColor(this global::Kampai.Util.Graphics.MaterialModifier modifier, global::UnityEngine.Color color, bool update = true)
		{
			modifier.SetColor(global::Kampai.Util.GameConstants.ShaderProperties.Procedural.BlendedColor, color);
			if (update)
			{
				modifier.Update();
			}
			return modifier;
		}

		public static float GetFadeAlpha(this global::Kampai.Util.Graphics.MaterialModifier modifier)
		{
			return modifier.GetFloat(global::Kampai.Util.GameConstants.ShaderProperties.Procedural.FadeAlpha);
		}
	}
}
