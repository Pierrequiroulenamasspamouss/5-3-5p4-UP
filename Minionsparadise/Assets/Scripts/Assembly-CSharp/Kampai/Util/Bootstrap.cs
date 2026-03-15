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
				global::UnityEngine.Handheld.ClearShaderCache();
			}
			global::FMOD.Studio.UnityUtil.ForceLoadLowLevelBinary();
			global::UnityEngine.Screen.sleepTimeout = -2;
		}
	}
}
