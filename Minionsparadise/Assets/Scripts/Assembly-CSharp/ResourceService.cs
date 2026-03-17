public class ResourceService : IResourceService
{
	public global::UnityEngine.Object Load(string path)
	{
		return global::UnityEngine.Resources.Load(path);
	}

	public string LoadText(string path)
	{
		return (global::UnityEngine.Resources.Load(path) as global::UnityEngine.TextAsset).text;
	}
}
