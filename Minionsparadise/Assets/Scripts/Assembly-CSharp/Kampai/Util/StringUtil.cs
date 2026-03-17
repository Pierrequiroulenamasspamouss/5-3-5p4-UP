namespace Kampai.Util
{
	public static class StringUtil
	{
		public static string UnifiedPlatformName(global::UnityEngine.RuntimePlatform platform)
		{
			switch (platform)
			{
			case global::UnityEngine.RuntimePlatform.Android:
				return "android";
			case global::UnityEngine.RuntimePlatform.IPhonePlayer:
				return "iOS";
			case global::UnityEngine.RuntimePlatform.OSXEditor:
			case global::UnityEngine.RuntimePlatform.WindowsEditor:
				return "editor";
			default:
				return null;
			}
		}
	}
}
