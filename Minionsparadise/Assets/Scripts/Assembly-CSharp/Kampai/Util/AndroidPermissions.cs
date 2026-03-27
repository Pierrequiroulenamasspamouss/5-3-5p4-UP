using UnityEngine;
using System.Collections.Generic;
using System;

namespace Kampai.Util
{
    public class AndroidPermissions : MonoBehaviour
    {
        private static AndroidPermissions sInstance;

        public static void RequestPermissions()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (sInstance == null)
            {
                GameObject go = new GameObject("AndroidPermissions");
                sInstance = go.AddComponent<AndroidPermissions>();
                DontDestroyOnLoad(go);
            }
            sInstance.StartRequest();
#endif
        }

        private void StartRequest()
        {
            try {
                int sdkVersion = GetSDKVersion();
                Debug.Log("[AndroidPermissions] SDK Version: " + sdkVersion);
                
                List<string> permissionsToRequest = new List<string>();

                if (sdkVersion >= 23) // Android 6.0
                {
                    if (sdkVersion >= 33) // Android 13
                    {
                        // API 33+ uses granular media permissions
                        if (!HasPermission("android.permission.READ_MEDIA_AUDIO")) permissionsToRequest.Add("android.permission.READ_MEDIA_AUDIO");
                        if (!HasPermission("android.permission.READ_MEDIA_IMAGES")) permissionsToRequest.Add("android.permission.READ_MEDIA_IMAGES");
                        if (!HasPermission("android.permission.READ_MEDIA_VIDEO")) permissionsToRequest.Add("android.permission.READ_MEDIA_VIDEO");
                    }
                    else
                    {
                        if (!HasPermission("android.permission.READ_EXTERNAL_STORAGE")) permissionsToRequest.Add("android.permission.READ_EXTERNAL_STORAGE");
                        if (!HasPermission("android.permission.WRITE_EXTERNAL_STORAGE")) permissionsToRequest.Add("android.permission.WRITE_EXTERNAL_STORAGE");
                    }

                    if (!HasPermission("android.permission.READ_PHONE_STATE")) permissionsToRequest.Add("android.permission.READ_PHONE_STATE");

                    if (permissionsToRequest.Count > 0)
                    {
                        Debug.Log("[AndroidPermissions] Requesting: " + string.Join(", ", permissionsToRequest.ToArray()));
                        DoRequestPermissions(permissionsToRequest.ToArray());
                    }
                    else
                    {
                        Debug.Log("[AndroidPermissions] All permissions already granted.");
                    }
                }
            } catch (Exception e) {
                Debug.LogError("[AndroidPermissions] Error: " + e.Message);
            }
        }

        private int GetSDKVersion()
        {
            using (var versionClass = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                return versionClass.GetStatic<int>("SDK_INT");
            }
        }

        private bool HasPermission(string permission)
        {
            using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (var activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    // context.checkSelfPermission(permission)
                    int result = activity.Call<int>("checkSelfPermission", permission);
                    return result == 0; // 0 is PERMISSION_GRANTED
                }
            }
        }

        private void DoRequestPermissions(string[] permissions)
        {
            using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (var activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    activity.Call("requestPermissions", permissions, 1);
                }
            }
        }
    }
}
