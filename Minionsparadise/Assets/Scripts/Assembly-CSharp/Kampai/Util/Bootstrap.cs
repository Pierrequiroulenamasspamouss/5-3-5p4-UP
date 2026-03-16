namespace Kampai.Util
{
	public class Bootstrap : global::UnityEngine.MonoBehaviour
	{
		private static bool HasBuggyBinarysShader()
		{
			string graphicsDeviceName = global::UnityEngine.SystemInfo.graphicsDeviceName;
			return graphicsDeviceName.Contains("SGX") || graphicsDeviceName.Contains("225");
		}

		private void Awake()
		{
			if (HasBuggyBinarysShader())
			{
#if UNITY_IOS || UNITY_ANDROID
				global::UnityEngine.Handheld.ClearShaderCache();
#endif
			}
			global::FMOD.Studio.UnityUtil.ForceLoadLowLevelBinary();
			global::UnityEngine.Screen.sleepTimeout = -2;
		}
	}
}
