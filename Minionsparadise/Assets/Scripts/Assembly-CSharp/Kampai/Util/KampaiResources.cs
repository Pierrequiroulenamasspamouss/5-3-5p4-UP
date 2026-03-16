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
        private static IAssetBundlesService _assetBundlesService;
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
            string[] searchDirs = { "/content", "/Shader", "/Resources" };

            foreach (string searchDir in searchDirs)
            {
                string fullPath = dataPath + searchDir;
                if (!Directory.Exists(fullPath))
                {
                    // Try fallback relative to executable for standalone
                    string parentPath = Path.GetDirectoryName(dataPath);
                    if (parentPath != null)
                    {
                        fullPath = parentPath.Replace('\\', '/') + "/Assets" + searchDir;
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
                            else
                            {
                                // If mapping failed to find /Assets/, try a simpler "Assets/..." path
                                string relativePath = "Assets" + searchDir + normalizedFile.Substring(fullPath.Length);
                                _editorAssetPathMap.Add(fileName, relativePath);
                            }
                        }
                    }
                }
                else
                {
                    if (_logger != null) _logger.Warning(string.Format("KampaiResources: Local directory '{0}' NOT FOUND", fullPath));
                }
            }
            if (_logger != null) _logger.Info(string.Format("KampaiResources: Initialized local asset map with {0} entries", _editorAssetPathMap.Count));
        }
#endif

        public static void SetManifestService(IManifestService service) 
        { 
            _manifestService = service; 
        }
        
        public static void SetAssetBundlesService(IAssetBundlesService service) 
        { 
            _assetBundlesService = service; 
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
            if (_manifestService == null || _localContentService == null) return false;
            return _manifestService.GetAssetLocation(path).Length > 0 || _localContentService.IsLocalAsset(path);
        }

        public static bool FileDownloaded(string path, DLCModel dlcModel)
        {
            string assetLocation = _manifestService.GetAssetLocation(path);
            if (string.IsNullOrEmpty(assetLocation))
            {
                return _localContentService.IsLocalAsset(path);
            }
            int bundleTier = _manifestService.GetBundleTier(assetLocation);
            return dlcModel.HighestTierDownloaded >= bundleTier;
        }

        public static bool IsAssetTierGated(string asset) 
        {
            return _manifestService.IsAssetTierTooHigh(asset);
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
                    result = Resources.Load(resolvedPath, type);
                    if (result != null && _logger != null) _logger.Debug(string.Format("  - Resources.Load success: '{0}'", resolvedPath));
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

            string assetLocation = _manifestService.GetAssetLocation(path);
            if (_logger != null) _logger.Debug(string.Format("  - Manifest location: '{0}'", assetLocation));
            
            bool isGated = !string.IsNullOrEmpty(assetLocation) && _manifestService.IsBundleTierTooHigh(assetLocation);

            if (string.IsNullOrEmpty(assetLocation))
            {
                _logger.Error(string.Format("Unable to find bundle or local asset for '{0}'.", path));
            }
            else if (!isGated)
            {
                AssetBundle bundle = GetBundleFromService(assetLocation);
                if (bundle != null)
                {
                    result = bundle.LoadAsset(path, type);
                }
            }

            if (result != null)
            {
                _cachedObjects.Add(path, result, type);
            }
            else if (!isGated)
            {
                _logger.Error(string.Format("KampaiResources.Load is returning NULL for object '{0}'", path));
            }
            else
            {
                _logger.Info(string.Format("Asset '{0}' is not available for the current tier", path));
            }

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

            string assetLocation = _manifestService.GetAssetLocation(path);
            if (string.IsNullOrEmpty(assetLocation))
            {
                Object obj = Load(path, type); 
                if (onComplete != null) onComplete(obj);
                return null;
            }

            AssetBundle bundle = GetBundleFromService(assetLocation);
            AsyncOperation bundleOp = null;
            
            if (bundle != null) 
            {
                bundleOp = bundle.LoadAssetAsync(path, type);
            }

            if (bundleOp != null)
            {
                routineRunner.StartCoroutine(LoadAsyncWait(bundleOp, onComplete, path, type));
            }
            else
            {
                _logger.Error(string.Format("KampaiResources.LoadAsync failed for {0}", path));
            }

            return bundleOp;
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
            else 
            {
                AssetBundleRequest abReq = request as AssetBundleRequest;
                if (abReq != null)
                {
                    obj = abReq.asset;
                }
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
            
            string editorPath;
            if (_editorAssetPathMap != null && _editorAssetPathMap.TryGetValue(fileName, out editorPath))
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

            string localKey = Path.GetFileName(path);
            if (_localContentService != null && _localContentService.IsLocalAsset(localKey))
            {
                resolvedPath = _localContentService.GetAssetPath(localKey);
                return true;
            }
#endif
            return false;
        }

        private static AssetBundle GetBundleFromService(string location)
        {
            if (_assetBundlesService.IsSharedBundle(location))
            {
                AssetBundle b = _assetBundlesService.GetSharedBundle(location);
                if (b == null) _logger.Debug(string.Format("Shared bundle {0} is null", location));
                return b;
            }
            else
            {
                AssetBundle b = _assetBundlesService.GetDLCBundle(location);
                if (b == null) _logger.Debug(string.Format("DLC bundle {0} is null", location));
                return b;
            }
        }
    }
}