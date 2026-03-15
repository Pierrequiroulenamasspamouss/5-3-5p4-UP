namespace Kampai.Util
{
	public static class ScreenUtils
	{
		public static void ToggleAutoRotation(bool isEnabled)
		{
			if (isEnabled)
			{
				isEnabled = global::Kampai.Util.Native.AutorotationIsOSAllowed();
			}
			global::UnityEngine.ScreenOrientation orientation = global::UnityEngine.Screen.orientation;
			global::UnityEngine.Screen.autorotateToLandscapeLeft = isEnabled || orientation == global::UnityEngine.ScreenOrientation.LandscapeLeft || orientation == global::UnityEngine.ScreenOrientation.LandscapeLeft;
			global::UnityEngine.Screen.autorotateToLandscapeRight = isEnabled || orientation == global::UnityEngine.ScreenOrientation.LandscapeRight;
		}
	}
}
