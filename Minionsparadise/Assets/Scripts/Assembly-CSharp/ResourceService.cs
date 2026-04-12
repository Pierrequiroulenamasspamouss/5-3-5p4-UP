public class ResourceService : IResourceService
{
	public global::UnityEngine.Object Load(string path)
	{
		return global::UnityEngine.Resources.Load(path);
	}

	public string LoadText(string path)
	{
		global::UnityEngine.Object obj = global::UnityEngine.Resources.Load(path);
		if (obj == null) return null;
		global::UnityEngine.TextAsset textAsset = obj as global::UnityEngine.TextAsset;
		return (textAsset != null) ? textAsset.text : null;
	}
}
