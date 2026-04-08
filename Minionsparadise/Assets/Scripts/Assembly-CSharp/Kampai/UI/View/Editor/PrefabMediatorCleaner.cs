using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using strange.extensions.mediation.impl;

namespace Kampai.Editor
{
    public class PrefabMediatorCleaner : EditorWindow
    {
        private const string PREFAB_PATH = "Assets/Resources/content/shared/shared_ui/ui_prefabs/ui_common/hud/screen_HUD_Panel_Settings_Menu.prefab";

        [MenuItem("Kampai Tools/Clean Settings Prefab Mediators")]
        public static void CleanSettingsPrefab()
        {
            GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB_PATH);
            if (prefabAsset == null)
            {
                Debug.LogError("[Cleaner] Could not find prefab at: " + PREFAB_PATH);
                return;
            }

            // Create an instance of the prefab to edit
            GameObject instance = PrefabUtility.InstantiatePrefab(prefabAsset) as GameObject;
            if (instance == null) return;

            int removedCount = 0;
            
            // Recursively find and remove all Mediator components
            // We use 'GetComponentsInChildren' with 'true' to include inactive objects
            Mediator[] mediators = instance.GetComponentsInChildren<Mediator>(true);
            
            foreach (Mediator m in mediators)
            {
                // We keep the Views, but remove the Mediators because they are bound at runtime in UIContext.cs
                string name = m.GetType().Name;
                string goName = m.gameObject.name;
                
                Undo.DestroyObjectImmediate(m);
                removedCount++;
                Debug.Log("[Cleaner] Removed "+ name +" from "+ goName);
            }

            if (removedCount > 0)
            {
                // Save the changes back to the prefab
                PrefabUtility.ReplacePrefab(instance, prefabAsset, ReplacePrefabOptions.ConnectToPrefab);
                Debug.Log("<b>[Cleaner] SUCCESS: removed "+removedCount+" mediator instances from Settings prefab.</b>");
            }
            else
            {
                Debug.Log("[Cleaner] No redundant mediators found in prefab.");
            }

            // Clean up the temporary instance
            DestroyImmediate(instance);
        }
    }
}
