namespace Kampai.Common
{
	public interface IManifestService
	{
		void GenerateMasterManifest();

		string GetAssetLocation(string asset);

		string GetAssetFileLocation(string assetFile);

		string GetAssetFileOriginalName(string assetFile);

		string GetAssetFileByOriginalName(string name);

		bool ContainsAssetFile(string name);
	}
}
