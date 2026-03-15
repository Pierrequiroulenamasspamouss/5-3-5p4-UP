using UnityEngine;

public class TestIO : MonoBehaviour
{
    void Start()
    {
#if !UNITY_WEBPLAYER
        System.IO.FileInfo fi = new System.IO.FileInfo("test");
        fi.CopyTo("test2", true);
        fi.Delete();
        System.IO.Directory.Delete("test", true);
#endif
    }
}
