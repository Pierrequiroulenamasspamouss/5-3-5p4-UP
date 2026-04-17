#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Kampai.Util.AssetEditor
{
    public class KampaiAssetManifestGenerator : AssetPostprocessor
    {
        [MenuItem("Kampai/Generate Asset Manifest")]
        public static void GenerateManifest()
        {
            Dictionary<string, string> map = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
            string dataPath = Application.dataPath.Replace('\\', '/');
            string[] searchDirs = { "/content", "/Shader", "/Resources" };

            foreach (string searchDir in searchDirs)
            {
                string fullPath = dataPath + searchDir;
                if (!Directory.Exists(fullPath)) continue;

                string[] files = Directory.GetFiles(fullPath, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    string normalizedFile = file.Replace('\\', '/');
                    if (normalizedFile.EndsWith(".meta")) continue;

                    string fileName = Path.GetFileNameWithoutExtension(normalizedFile);
                    if (!map.ContainsKey(fileName))
                    {
                        int assetsIdx = normalizedFile.IndexOf("/Assets/", System.StringComparison.OrdinalIgnoreCase);
                        if (assetsIdx >= 0)
                        {
                            string relativePath = normalizedFile.Substring(assetsIdx + 1);
                            map.Add(fileName, relativePath);
                        }
                    }
                }
            }

            string json = SerializeManifest(map);
            string resourcesPath = Path.Combine(Application.dataPath, "Resources");
            if (!Directory.Exists(resourcesPath)) Directory.CreateDirectory(resourcesPath);
            
            string manifestPath = Path.Combine(resourcesPath, "KampaiAssetManifest.json");
            File.WriteAllText(manifestPath, json);
            AssetDatabase.Refresh();

            Debug.Log("KampaiAssetManifest: Generated manifest with " + map.Count + " entries at " + manifestPath);
        }

        private static string SerializeManifest(Dictionary<string, string> map)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");

            int index = 0;
            foreach (KeyValuePair<string, string> entry in map)
            {
                builder.Append("  \"");
                builder.Append(EscapeJson(entry.Key));
                builder.Append("\": \"");
                builder.Append(EscapeJson(entry.Value));
                builder.Append('"');

                if (index < map.Count - 1)
                {
                    builder.Append(',');
                }

                builder.AppendLine();
                index++;
            }

            builder.Append('}');
            return builder.ToString();
        }

        private static string EscapeJson(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\t", "\\t");
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            // Auto-regenerate if things change in key folders
            foreach (string asset in importedAssets)
            {
                if (asset.Contains("/content/") || asset.Contains("/Resources/") || asset.Contains("/Shader/"))
                {
                    GenerateManifest();
                    return;
                }
            }
        }
    }
}
#endif
