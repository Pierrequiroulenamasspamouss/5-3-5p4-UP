namespace Kampai.Main
{
	public class LocalContentService : global::Kampai.Main.ILocalContentService
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LocalContentService") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.Dictionary<string, string> resourceNamesMap;

		private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.Dictionary<string, string>> specificNamesMap;

		private global::System.Collections.Generic.HashSet<string> streamingAssets;

		private global::System.Collections.Generic.List<string> audioBanks;

		private int qualityLevel;

		[PostConstruct]
		public void PostConstruct()
		{
			global::UnityEngine.TextAsset textAsset = global::UnityEngine.Resources.Load<global::UnityEngine.TextAsset>("Manifest");
			if (null == textAsset)
			{
				logger.Error("Failed to load bundle resources manifest");
				return;
			}
			global::Kampai.Main.LocalResourcesManifest localResourcesManifest = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Main.LocalResourcesManifest>(textAsset.text);
			global::UnityEngine.Resources.UnloadAsset(textAsset);
			resourceNamesMap = new global::System.Collections.Generic.Dictionary<string, string>(localResourcesManifest.bundledAssets.Count);
			foreach (string bundledAsset in localResourcesManifest.bundledAssets)
			{
				string fileName = global::System.IO.Path.GetFileName(bundledAsset);
				if (!resourceNamesMap.ContainsKey(fileName))
				{
					resourceNamesMap.Add(fileName, bundledAsset);
				}
			}
			specificNamesMap = FilterQualityLODFiles(localResourcesManifest.separatedAssets);
			streamingAssets = new global::System.Collections.Generic.HashSet<string>(localResourcesManifest.streamingAssets);
			audioBanks = localResourcesManifest.audioBanks;
		}

		public void SetDLCQuality(string levelName)
		{
			int num = global::Kampai.Util.ResourceQualityHelper.ConvertQualityStringToLODlevel(levelName);
			if (num > -1)
			{
				qualityLevel = num;
			}
			else
			{
				qualityLevel = 0;
			}
		}

		private global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.Dictionary<string, string>> FilterQualityLODFiles(global::System.Collections.Generic.List<global::Kampai.Util.LODAsset> importedLODS)
		{
			global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.Dictionary<string, string>> dictionary = new global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.Dictionary<string, string>>();
			dictionary.Add(0, new global::System.Collections.Generic.Dictionary<string, string>());
			dictionary.Add(1, new global::System.Collections.Generic.Dictionary<string, string>());
			dictionary.Add(2, new global::System.Collections.Generic.Dictionary<string, string>());
			dictionary.Add(3, new global::System.Collections.Generic.Dictionary<string, string>());
			foreach (global::Kampai.Util.LODAsset importedLOD in importedLODS)
			{
				if (dictionary[importedLOD.level].ContainsKey(importedLOD.shortName))
				{
					logger.Error("LOD separated resource {0} is already mapped under level {1}", importedLOD.shortName, importedLOD.level);
				}
				else
				{
					dictionary[importedLOD.level].Add(importedLOD.shortName, importedLOD.path);
				}
			}
			return dictionary;
		}

		public bool IsLocalAsset(string name)
		{
			return resourceNamesMap.ContainsKey(name) || specificNamesMap[qualityLevel].ContainsKey(name);
		}

		public string GetAssetPath(string name)
		{
			if (!IsLocalAsset(name))
			{
				return string.Empty;
			}
			if (specificNamesMap[qualityLevel].ContainsKey(name))
			{
				return specificNamesMap[qualityLevel][name];
			}
			return resourceNamesMap[name];
		}

		public bool IsStreamingAsset(string name)
		{
			return streamingAssets.Contains(name);
		}

		public global::System.Collections.Generic.List<string> GetStreamingAudioBanks()
		{
			return audioBanks;
		}
	}
}
