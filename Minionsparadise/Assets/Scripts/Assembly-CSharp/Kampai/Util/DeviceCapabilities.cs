namespace Kampai.Util
{
	public class DeviceCapabilities
	{
		private static bool isTablet;

		public global::Kampai.Util.TargetPerformance GetTargetPerformance(global::Kampai.Util.IKampaiLogger logger, global::UnityEngine.RuntimePlatform platform, global::Kampai.Util.DeviceInformation info)
		{
			global::System.Collections.Generic.List<global::Kampai.Util.IDeviceInspector> list = new global::System.Collections.Generic.List<global::Kampai.Util.IDeviceInspector>();
			list.Add(new global::Kampai.Util.AndroidDeviceInspector());
			list.Add(new global::Kampai.Util.IOSDeviceInspector(logger));
			list.Add(new global::Kampai.Util.EditorDeviceInspector());
			global::System.Collections.Generic.List<global::Kampai.Util.IDeviceInspector> list2 = list;
			foreach (global::Kampai.Util.IDeviceInspector item in list2)
			{
				if (item.IsSupported(platform))
				{
					return item.CaluclateTargetPerformance(info);
				}
			}
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Unknown platform {0}; defaulting to low LOD", platform.ToString());
			return global::Kampai.Util.TargetPerformance.LOW;
		}

		public int GetTargetFrameRate(global::Kampai.Util.IKampaiLogger logger, global::UnityEngine.RuntimePlatform platform, global::Kampai.Util.DeviceInformation info)
		{
			global::System.Collections.Generic.List<global::Kampai.Util.IDeviceInspector> list = new global::System.Collections.Generic.List<global::Kampai.Util.IDeviceInspector>();
			list.Add(new global::Kampai.Util.AndroidDeviceInspector());
			list.Add(new global::Kampai.Util.IOSDeviceInspector(logger));
			list.Add(new global::Kampai.Util.EditorDeviceInspector());
			global::System.Collections.Generic.List<global::Kampai.Util.IDeviceInspector> list2 = list;
			foreach (global::Kampai.Util.IDeviceInspector item in list2)
			{
				if (item.IsSupported(platform))
				{
					return item.GetTargetFrameRate(info);
				}
			}
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Unknown platform {0}; defaulting to 30 FPS", platform.ToString());
			return 30;
		}

		public static bool IsTablet()
		{
			return isTablet;
		}

		public static void Initialize()
		{
			float num = 6f;
			float num2 = global::UnityEngine.Mathf.Sqrt(global::UnityEngine.Screen.width * global::UnityEngine.Screen.width + global::UnityEngine.Screen.height * global::UnityEngine.Screen.height);
			float num3 = num2 / global::UnityEngine.Screen.dpi;
			if (num3 > num)
			{
				isTablet = true;
			}
			else
			{
				isTablet = false;
			}
		}
	}
}
