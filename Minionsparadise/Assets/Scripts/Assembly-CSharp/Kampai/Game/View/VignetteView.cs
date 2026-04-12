namespace Kampai.Game.View
{
	public class VignetteView : global::strange.extensions.mediation.impl.View
	{
		private const string key_VignetteSize = "_Size";

		private float? initialVignetteSize;

		internal void SetVignetteSize(float? size)
		{
			global::UnityEngine.Material material = GetComponent<global::UnityEngine.Renderer>().material;
			if (!material.HasProperty("_Size"))
			{
				return;
			}
			if (!initialVignetteSize.HasValue)
			{
				initialVignetteSize = material.GetFloat("_Size");
			}
			float value = initialVignetteSize.Value;
			if (size.HasValue)
			{
				value = size.Value;
			}
			material.SetFloat("_Size", value);
		}
	}
}
