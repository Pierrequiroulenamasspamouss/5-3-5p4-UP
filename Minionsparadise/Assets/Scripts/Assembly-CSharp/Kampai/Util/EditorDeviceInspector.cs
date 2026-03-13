namespace Kampai.Util
{
	public class EditorDeviceInspector : global::Kampai.Util.IDeviceInspector
	{
		public bool IsSupported(global::UnityEngine.RuntimePlatform platform)
		{
			return false;
		}

		public global::Kampai.Util.TargetPerformance CaluclateTargetPerformance(global::Kampai.Util.DeviceInformation device)
		{
			return global::Kampai.Util.TargetPerformance.HIGH;
		}

		public int GetTargetFrameRate(global::Kampai.Util.DeviceInformation device)
		{
			return 60;
		}
	}
}
