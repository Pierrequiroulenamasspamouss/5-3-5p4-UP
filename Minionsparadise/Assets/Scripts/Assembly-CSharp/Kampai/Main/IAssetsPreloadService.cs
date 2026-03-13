namespace Kampai.Main
{
	public interface IAssetsPreloadService
	{
		void AddAssetToPreloadQueue(global::Kampai.Main.PreloadableAsset asset);

		void PreloadAllAssets();

		void StopAssetsPreload();

		void SetIntegrationStepLength(int msec);
	}
}
