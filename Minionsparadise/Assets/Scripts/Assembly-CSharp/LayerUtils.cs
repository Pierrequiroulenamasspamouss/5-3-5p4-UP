public static class LayerUtils
{
	public static void SetLayerRecursively(this global::UnityEngine.GameObject obj, int layer)
	{
		obj.layer = layer;
		foreach (global::UnityEngine.Transform item in obj.transform)
		{
			item.gameObject.SetLayerRecursively(layer);
		}
	}
}
