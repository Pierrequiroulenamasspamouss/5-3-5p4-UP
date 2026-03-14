using UnityEngine;
using UnityEditor;
using System.IO;

public class ClearUserPrefs : EditorWindow
{
    [MenuItem("Kampai/Debug/Clear User Preferences")]
    public static void ClearAll()
    {
        if (EditorUtility.DisplayDialog("Clear User Preferences", 
            "This will delete all PlayerPrefs and all files in the Persistent Data Path. Are you sure?", 
            "Yes, Clear Everything", "Cancel"))
        {
            // Clear PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("[ClearUserPrefs] PlayerPrefs cleared.");

            // Clear Persistent Data Path
            string path = Application.persistentDataPath;
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                Debug.Log("[ClearUserPrefs] Persistent data path cleared: " + path);
            }
            
            EditorUtility.DisplayDialog("Success", "All user preferences and local data have been cleared.", "OK");
        }
    }

    [MenuItem("Kampai/Debug/Open Persistent Data Path")]
    public static void OpenPersistentDataPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}
