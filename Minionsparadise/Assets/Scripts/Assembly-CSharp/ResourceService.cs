public class ResourceService : IResourceService
{
	public global::UnityEngine.Object Load(string path)
	{
		return global::UnityEngine.Resources.Load(path);
	}

	public string LoadText(string path)
	{
		global::UnityEngine.TextAsset textAsset = global::UnityEngine.Resources.Load(path) as global::UnityEngine.TextAsset;
		if (textAsset == null)
		{
			global::UnityEngine.Debug.LogError("ANTIGRAVITY: ResourceService failed to load TextAsset at path: " + path);
			return null;
		}
		global::UnityEngine.Debug.Log("ANTIGRAVITY: ResourceService loaded TextAsset at path: " + path + " (Size: " + textAsset.text.Length + ")");
		return textAsset.text;
	}
}
