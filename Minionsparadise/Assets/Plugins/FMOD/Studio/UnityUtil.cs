namespace FMOD.Studio
{
	public static class UnityUtil
	{
		public static global::FMOD.VECTOR toFMODVector(this global::UnityEngine.Vector3 vec)
		{
			global::FMOD.VECTOR result = default(global::FMOD.VECTOR);
			result.x = vec.x;
			result.y = vec.y;
			result.z = vec.z;
			return result;
		}

		public static global::FMOD.ATTRIBUTES_3D to3DAttributes(this global::UnityEngine.Vector3 pos)
		{
			return new global::FMOD.ATTRIBUTES_3D
			{
				forward = global::UnityEngine.Vector3.forward.toFMODVector(),
				up = global::UnityEngine.Vector3.up.toFMODVector(),
				position = pos.toFMODVector()
			};
		}

		public static global::FMOD.ATTRIBUTES_3D to3DAttributes(global::UnityEngine.GameObject go, global::UnityEngine.Rigidbody rigidbody = null)
		{
			global::FMOD.ATTRIBUTES_3D result = new global::FMOD.ATTRIBUTES_3D
			{
				forward = go.transform.forward.toFMODVector(),
				up = go.transform.up.toFMODVector(),
				position = go.transform.position.toFMODVector()
			};
			if ((bool)rigidbody)
			{
				result.velocity = rigidbody.linearVelocity.toFMODVector();
			}
			return result;
		}

		public static void Log(string msg)
		{
		}

		public static void LogWarning(string msg)
		{
			global::UnityEngine.Debug.LogWarning(msg);
		}

		public static void LogError(string msg)
		{
			global::UnityEngine.Debug.LogError(msg);
		}

		public static bool ForceLoadLowLevelBinary()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			global::UnityEngine.AndroidJavaObject androidJavaObject = null;
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				androidJavaObject = androidJavaClass.GetStatic<global::UnityEngine.AndroidJavaObject>("currentActivity");
			}
			global::UnityEngine.Debug.Log("FMOD ANDROID AUDIOTRACK: " + ((androidJavaObject != null) ? "VALID ACTIVITY!" : "ERROR NO ACTIVITY"));
			using (global::UnityEngine.AndroidJavaClass androidJavaClass2 = new global::UnityEngine.AndroidJavaClass("org.fmod.FMOD"))
			{
				if (androidJavaClass2 != null)
				{
					global::UnityEngine.Debug.Log("FMOD ANDROID AUDIOTRACK: assigning activity to fmod java");
					androidJavaClass2.CallStatic("init", androidJavaObject);
				}
				else
				{
					global::UnityEngine.Debug.Log("FMOD ANDROID AUDIOTRACK: ERROR NO FMOD JAVA");
				}
			}
			Log("loading binaries: fmodstudio and fmod");
			global::UnityEngine.AndroidJavaClass androidJavaClass3 = new global::UnityEngine.AndroidJavaClass("java.lang.System");
			androidJavaClass3.CallStatic("loadLibrary", "fmod");
			androidJavaClass3.CallStatic("loadLibrary", "fmodstudio");

			Log("Attempting to call Memory_GetStats");
			int currentalloced;
			int maxalloced;
			if (!ERRCHECK(global::FMOD.Memory.GetStats(out currentalloced, out maxalloced)))
			{
				LogError("Memory_GetStats returned an error");
				return false;
			}
#endif
			Log("Calling Memory_GetStats succeeded!");
			return true;
		}

		public static bool ERRCHECK(global::FMOD.RESULT result)
		{
			if (result != global::FMOD.RESULT.OK)
			{
				LogWarning("FMOD Error (" + result.ToString() + "): " + global::FMOD.Error.String(result));
			}
			return result == global::FMOD.RESULT.OK;
		}
	}
}
