using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Kampai.Common;
using Kampai.Main;
using Kampai.Splash;
using Object = UnityEngine.Object;

namespace Kampai.Util
{
    public static class KampaiResources
    {
        private sealed class AssetsCache
        {
            private readonly Dictionary<string, Dictionary<Type, Object>> _cache = new Dictionary<string, Dictionary<Type, Object>>(4096);

            public Object Get(string name, Type type)
            {
                if (string.IsNullOrEmpty(name)) return null;
                
                Dictionary<Type, Object> typeDict;
                if (!_cache.TryGetValue(name, out typeDict)) return null;
                
                Object obj;
                return typeDict.TryGetValue(type, out obj) ? obj : null;
            }

            public void Add(string name, Object obj, Type type)
            {
                if (obj == null || string.IsNullOrEmpty(name)) return;
                
                Dictionary<Type, Object> typeDict;
                if (!_cache.TryGetValue(name, out typeDict))
                {
                    typeDict = new Dictionary<Type, Object>(1);
                    _cache.Add(name, typeDict);
                }
                
                typeDict[type] = obj;
            }

            public void Clear()
            {
                foreach (Dictionary<Type, Object> typeDict in _cache.Values)
                {
                    foreach (Object obj in typeDict.Values)
                    {
                        if (obj != null && !(obj is GameObject) && !(obj is Component))
                        {
                            Resources.UnloadAsset(obj);
                        }
                    }
                }
                _cache.Clear();
            }
        }

        private static IManifestService _manifestService;
        private static ILocalContentService _localContentService;
        private static IKampaiLogger _logger;
        private static readonly AssetsCache _cachedObjects = new AssetsCache();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        private static Dictionary<string, string> _editorAssetPathMap;

        private static void InitializeEditorAssetMap()
        {
            if (_editorAssetPathMap != null) return;
            
            _editorAssetPathMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string dataPath = Application.dataPath.Replace('\\', '/');
            
            // List of directories to scan for local assets
            string[] searchDirs = { "/Shader", "/Resources" };

                foreach (string searchDir in searchDirs)
                {
                    string fullPath = dataPath + searchDir;
                    if (!Directory.Exists(fullPath))
                    {
                        // Try fallback relative to executable for standalone
                        string parentPath = Path.GetDirectoryName(dataPath);
                        if (parentPath != null)
                        {
                            // Check if it's in the root directly (some custom builds/5.3 behavior)
                            string rootPath = parentPath.Replace('\\', '/') + searchDir;
                            if (Directory.Exists(rootPath)) 
                            {
                                fullPath = rootPath;
                            }
                            else
                            {
                                // Try Assets relative path (old behavior)
                                string fallbackPath = parentPath.Replace('\\', '/') + "/Assets" + searchDir;
                                if (Directory.Exists(fallbackPath)) fullPath = fallbackPath;
                            }
                        }
                    }

                    if (Directory.Exists(fullPath))
                    {
                        if (_logger != null) _logger.Debug(string.Format("KampaiResources: Scanning local directory '{0}'", fullPath));
                        string[] files = Directory.GetFiles(fullPath, "*.*", SearchOption.AllDirectories);
                        foreach (string file in files)
                        {
                            string normalizedFile = file.Replace('\\', '/');
                            if (normalizedFile.EndsWith(".meta")) continue;

                            string fileName = Path.GetFileNameWithoutExtension(normalizedFile);
                            if (!_editorAssetPathMap.ContainsKey(fileName))
                            {
                                int assetsIdx = normalizedFile.IndexOf("/Assets/", StringComparison.OrdinalIgnoreCase);
                                if (assetsIdx >= 0)
                                {
                                    string relativePath = normalizedFile.Substring(assetsIdx + 1);
                                    _editorAssetPathMap.Add(fileName, relativePath);
                                }
                                else if (normalizedFile.Contains("/Resources/"))
                                {
                                    // If it's in a physical Resources folder in a build, treat as Resource path
                                    _editorAssetPathMap.Add(fileName, normalizedFile);
                                }
                                else
                                {
                                    // If mapping failed to find /Assets/, try a path relative to scanDir
                                    string relativePath = "Assets" + searchDir + normalizedFile.Substring(fullPath.Length);
                                    _editorAssetPathMap.Add(fileName, relativePath);
                                }
                                if (_logger != null) _logger.Debug(string.Format("KampaiResources: Mapped '{0}' -> '{1}'", fileName, _editorAssetPathMap[fileName]));
                            }
                        }
                    }
                    else
                    {
                        if (_logger != null) _logger.Debug(string.Format("KampaiResources: Optional local directory '{0}' NOT FOUND", fullPath));
                    }
                }
            if (_logger != null) _logger.Info(string.Format("KampaiResources: Initialized local asset map with {0} entries", _editorAssetPathMap.Count));

            // Load manifest if it exists
            TextAsset manifest = Resources.Load<TextAsset>("KampaiAssetManifest");
            if (manifest != null)
            {
                try
                {
                    Dictionary<string, string> savedMap = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(manifest.text);
                    if (savedMap != null)
                    {
                        int addedCount = 0;
                        foreach (KeyValuePair<string, string> kvp in savedMap)
                        {
                            if (!_editorAssetPathMap.ContainsKey(kvp.Key))
                            {
                                _editorAssetPathMap.Add(kvp.Key, kvp.Value);
                                addedCount++;
                            }
                        }
                        if (_logger != null) _logger.Info(string.Format("KampaiResources: Loaded {0} entries from KampaiAssetManifest ({1} new)", savedMap.Count, addedCount));
                    }
                }
                catch (Exception ex)
                {
                    if (_logger != null) _logger.Error(string.Format("KampaiResources: Failed to parse KampaiAssetManifest: {0}", ex.Message));
                }
            }
            else
            {
                if (_logger != null) _logger.Warning("KampaiResources: KampaiAssetManifest.json not found in Resources. Build asset loading may fail.");
            }
        }
#endif

        public static void SetManifestService(IManifestService service) 
        { 
            _manifestService = service; 
        }
        

        
        public static void SetLocalContentService(ILocalContentService service) 
        { 
            _localContentService = service; 
        }
        
        public static void SetLogger() 
        {
            _logger = Elevation.Logging.LogManager.GetClassLogger("KampaiResources") as IKampaiLogger;
        }

        public static void ClearCache() 
        { 
            _cachedObjects.Clear(); 
        }

        public static bool FileExists(string path)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            InitializeEditorAssetMap();
            if (_editorAssetPathMap != null && _editorAssetPathMap.ContainsKey(Path.GetFileNameWithoutExtension(path)))
            {
                return true;
            }
#endif
            bool exists = false;
            if (_manifestService != null)
            {
                exists |= _manifestService.GetAssetLocation(path).Length > 0;
            }
            if (_localContentService != null)
            {
                exists |= _localContentService.IsLocalAsset(path);
            }
            return exists;
        }

        public static bool FileDownloaded(string path, DLCModel dlcModel)
        {
            return true;
        }

        public static bool IsAssetTierGated(string asset) 
        {
            return false;
        }

        public static T Load<T>(string path) where T : class 
        {
            return Load(path, typeof(T)) as T;
        }

        public static Object Load(string path) 
        {
            return Load(path, typeof(Object));
        }

        public static Object Load(string path, Type type)
        {
            if (_logger != null) _logger.Debug(string.Format("KampaiResources.Load('{0}', type={1})", path, type != null ? type.Name : "null"));
            
            Object cached = _cachedObjects.Get(path, type);
            if (cached != null) return cached;

            TimeProfiler.StartAssetLoadSection(path);
            Object result = null;
            string resolvedPath;
            bool isEditorDatabasePath;

            if (TryGetLocalAssetPath(path, out resolvedPath, out isEditorDatabasePath))
            {
                if (_logger != null) _logger.Debug(string.Format("  - Local path found: '{0}' (isEditor={1})", resolvedPath, isEditorDatabasePath));
#if UNITY_EDITOR
                if (isEditorDatabasePath)
                {
                    result = UnityEditor.AssetDatabase.LoadAssetAtPath(resolvedPath, type);
                    if (result == null && _logger != null) _logger.Warning(string.Format("  - AssetDatabase failed to load at '{0}'", resolvedPath));
                }
#endif
                if (result == null)
                {
                    string resourcePath = resolvedPath;
                    if (resourcePath.Contains("/Resources/"))
                    {
                        int resIndex = resourcePath.IndexOf("/Resources/", StringComparison.Ordinal) + 11;
                        resourcePath = Path.ChangeExtension(resourcePath.Substring(resIndex), null);
                    }
                    result = Resources.Load(resourcePath, type);
                    if (result != null && _logger != null) _logger.Debug(string.Format("  - Resources.Load success: '{0}'", resourcePath));
                }

                if (result != null)
                {
                    _cachedObjects.Add(path, result, type);
                    TimeProfiler.EndAssetLoadSection();
                    return result;
                }
            }
            else
            {
                if (_logger != null) _logger.Debug(string.Format("  - No local path for '{0}'", path));
            }

#if !(UNITY_EDITOR || UNITY_STANDALONE_WIN)
            if (_logger != null) _logger.Debug(string.Format("  - Assets are expected to be local or in Resources. Skipping bundle check for '{0}'", path));
#else
            if (_logger != null) _logger.Debug(string.Format("  - Skipping bundle check for '{0}' on Windows/Editor", path));
#endif

            TimeProfiler.EndAssetLoadSection();
            return result;
        }

        public static AsyncOperation LoadAsync(string path, IRoutineRunner routineRunner, Action<Object> onComplete = null)
        {
            return LoadAsync(path, typeof(Object), routineRunner, onComplete);
        }

        public static AsyncOperation LoadAsync(string path, Type type, IRoutineRunner routineRunner, Action<Object> onComplete = null)
        {
            Object cached = _cachedObjects.Get(path, type);
            if (cached != null)
            {
                if (onComplete != null) onComplete(cached);
                return null;
            }

            string resolvedPath;
            bool isEditorPath;

            if (TryGetLocalAssetPath(path, out resolvedPath, out isEditorPath))
            {
#if UNITY_EDITOR
                if (isEditorPath)
                {
                    Object editorObj = UnityEditor.AssetDatabase.LoadAssetAtPath(resolvedPath, type);
                    _cachedObjects.Add(path, editorObj, type);
                    if (onComplete != null) onComplete(editorObj);
                    return null;
                }
#endif
                ResourceRequest request = Resources.LoadAsync(resolvedPath, type);
                routineRunner.StartCoroutine(LoadAsyncWait(request, onComplete, path, type));
                return request;
            }

            if (onComplete != null) onComplete(null);
            return null;
        }

        private static IEnumerator LoadAsyncWait(AsyncOperation request, Action<Object> onComplete, string name, Type type)
        {
            if (request == null) yield break;
            yield return request;

            Object obj = null;
            
            ResourceRequest resReq = request as ResourceRequest;
            if (resReq != null)
            {
                obj = resReq.asset;
            }

            if (obj != null)
            {
                _cachedObjects.Add(name, obj, type);
                if (onComplete != null) onComplete(obj);
            }
        }

        private static bool TryGetLocalAssetPath(string path, out string resolvedPath, out bool isEditorPath)
        {
            resolvedPath = null;
            isEditorPath = false;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            InitializeEditorAssetMap();
            string fileName = Path.GetFileNameWithoutExtension(path);
            
            string editorPath = null;
            if (_editorAssetPathMap != null)
            {
                if (_editorAssetPathMap.TryGetValue(fileName, out editorPath))
                {
                    // Direct match
                }
                else if (_editorAssetPathMap.TryGetValue(fileName + "_Phone", out editorPath))
                {
                    // Device suffix match
                    if (_logger != null) _logger.Debug(string.Format("KampaiResources: Resolved '{0}' to '{1}'", fileName, fileName + "_Phone"));
                }
                else if (_editorAssetPathMap.TryGetValue(fileName + "_Tablet", out editorPath))
                {
                    // Device suffix match
                    if (_logger != null) _logger.Debug(string.Format("KampaiResources: Resolved '{0}' to '{1}'", fileName, fileName + "_Tablet"));
                }
            }

            if (editorPath != null)
            {
#if UNITY_EDITOR
                resolvedPath = editorPath;
                isEditorPath = true;
                return true;
#else
                if (editorPath.Contains("/Resources/"))
                {
                    int resIndex = editorPath.IndexOf("/Resources/", StringComparison.Ordinal) + 11;
                    resolvedPath = Path.ChangeExtension(editorPath.Substring(resIndex), null);
                    return true;
                }
#endif
            }
#endif

            string localKey = global::System.IO.Path.GetFileName(path);
            if (_localContentService != null && _localContentService.IsLocalAsset(localKey))
            {
                resolvedPath = _localContentService.GetAssetPath(localKey);
                return true;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            // On Android with bundles disabled, we might want to try Resources.Load directly as a fallback
            resolvedPath = path; 
            return true;
#endif
            return false;
        }


    }
}