namespace Kampai.Main
{
	public interface IAssetBundlesService
	{
		bool IsSharedBundle(string bundleName);

		void LoadSharedBundle(string bundleName);

		global::UnityEngine.AssetBundle GetSharedBundle(string bundleName);

		global::UnityEngine.AssetBundle GetDLCBundle(string bundleName);

		void UnloadSharedBundles();

		void UnloadDLCBundles();
	}
}
