namespace Kampai.Common
{
	public interface IManifestService
	{
		void GenerateMasterManifest();

		string GetAssetLocation(string asset);

		string GetBundleLocation(string bundle);

		string GetBundleOriginalName(string bundle);

		string GetBundleByOriginalName(string name);

		int GetBundleTier(string bundle);

		global::System.Collections.Generic.List<global::Kampai.Util.BundleInfo> GetBundles();

		string GetDLCURL();

		global::System.Collections.Generic.IList<string> GetSharedBundles();

		global::System.Collections.Generic.IList<string> GetShaderBundles();

		global::System.Collections.Generic.IList<string> GetAudioBundles();

		global::System.Collections.Generic.IList<string> GetAssetsInBundle(string bundle);

		bool ContainsBundle(string name);

		global::System.Collections.Generic.IList<global::Kampai.Util.BundleInfo> GetUnstreamablePackagedBundlesList();

		bool IsStreamingBundle(string bundle);

		bool IsBundleTierTooHigh(string bundle);

		bool IsAssetTierTooHigh(string asset);
	}
}
