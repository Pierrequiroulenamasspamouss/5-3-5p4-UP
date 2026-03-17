namespace Kampai.Util
{
	public class AndroidDeviceInspector : global::Kampai.Util.IDeviceInspector
	{
		public bool IsSupported(global::UnityEngine.RuntimePlatform platform)
		{
			return platform == global::UnityEngine.RuntimePlatform.Android;
		}

		public global::Kampai.Util.TargetPerformance CaluclateTargetPerformance(global::Kampai.Util.DeviceInformation device)
		{
			int ram = device.ram;
			if (ram > 1536)
			{
				return global::Kampai.Util.TargetPerformance.HIGH;
			}
			if (ram > 1024)
			{
				return global::Kampai.Util.TargetPerformance.MED;
			}
			if (ram > 768)
			{
				return global::Kampai.Util.TargetPerformance.LOW;
			}
			return global::Kampai.Util.TargetPerformance.VERYLOW;
		}

		public int GetTargetFrameRate(global::Kampai.Util.DeviceInformation device)
		{
			return 30;
		}
	}
}
