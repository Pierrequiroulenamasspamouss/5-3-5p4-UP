namespace Kampai.Util
{
	public class IOSDeviceInspector : global::Kampai.Util.IDeviceInspector
	{
		private sealed class DeviceIdentifier
		{
			public string family;

			public float majorMinor;
		}

		private global::Kampai.Util.IKampaiLogger logger;

		private global::System.Collections.Generic.Dictionary<string, float> min = new global::System.Collections.Generic.Dictionary<string, float>
		{
			{ "iPhone", 4.1f },
			{ "iPad", 2.1f },
			{ "iPod", 5.1f }
		};

		private global::System.Collections.Generic.Dictionary<string, float> med = new global::System.Collections.Generic.Dictionary<string, float>
		{
			{ "iPhone", 5.1f },
			{ "iPad", 3.4f },
			{ "iPod", 6.1f }
		};

		private global::System.Collections.Generic.Dictionary<string, float> high = new global::System.Collections.Generic.Dictionary<string, float>
		{
			{ "iPhone", 6.1f },
			{ "iPad", 4.1f },
			{ "iPod", 7.1f }
		};

		private global::System.Collections.Generic.Dictionary<string, float> xhigh = new global::System.Collections.Generic.Dictionary<string, float>
		{
			{ "iPhone", 8.1f },
			{ "iPad", 5.3f },
			{ "iPod", 8.1f }
		};

		public IOSDeviceInspector(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public bool IsSupported(global::UnityEngine.RuntimePlatform platform)
		{
			return platform == global::UnityEngine.RuntimePlatform.IPhonePlayer;
		}

		public global::Kampai.Util.TargetPerformance CaluclateTargetPerformance(global::Kampai.Util.DeviceInformation device)
		{
			global::Kampai.Util.IOSDeviceInspector.DeviceIdentifier deviceIdentifier = ParseDeviceInfo(device);
			if (deviceIdentifier.family != null)
			{
				if (!min.ContainsKey(deviceIdentifier.family))
				{
					return NotFound(string.Format("Unknown family: {0} - {1}", deviceIdentifier.family, deviceIdentifier.majorMinor.ToString()));
				}
				if (deviceIdentifier.majorMinor < min[deviceIdentifier.family])
				{
					return global::Kampai.Util.TargetPerformance.UNSUPPORTED;
				}
				if (deviceIdentifier.majorMinor < med[deviceIdentifier.family])
				{
					return global::Kampai.Util.TargetPerformance.LOW;
				}
				if (deviceIdentifier.majorMinor < high[deviceIdentifier.family])
				{
					return global::Kampai.Util.TargetPerformance.MED;
				}
				return global::Kampai.Util.TargetPerformance.HIGH;
			}
			return NotFound(string.Format("Unrecognized device model format: {0}", device.model));
		}

		public int GetTargetFrameRate(global::Kampai.Util.DeviceInformation device)
		{
			global::Kampai.Util.IOSDeviceInspector.DeviceIdentifier deviceIdentifier = ParseDeviceInfo(device);
			if (deviceIdentifier.family != null && xhigh.ContainsKey(deviceIdentifier.family) && deviceIdentifier.majorMinor >= xhigh[deviceIdentifier.family])
			{
				return 60;
			}
			return 30;
		}

		private global::Kampai.Util.TargetPerformance NotFound(string message)
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Warning, "{0} - Defaulting to high performance", message);
			return global::Kampai.Util.TargetPerformance.HIGH;
		}

		private global::Kampai.Util.IOSDeviceInspector.DeviceIdentifier ParseDeviceInfo(global::Kampai.Util.DeviceInformation device)
		{
			global::Kampai.Util.IOSDeviceInspector.DeviceIdentifier deviceIdentifier = new global::Kampai.Util.IOSDeviceInspector.DeviceIdentifier();
			deviceIdentifier.family = null;
			global::System.Text.RegularExpressions.MatchCollection matchCollection = global::System.Text.RegularExpressions.Regex.Matches(device.model, "([a-zA-Z]+)(\\d+),(\\d+)");
			if (matchCollection != null && matchCollection.Count == 1)
			{
				global::System.Collections.IEnumerator enumerator = matchCollection.GetEnumerator();
				enumerator.MoveNext();
				global::System.Text.RegularExpressions.Match match = enumerator.Current as global::System.Text.RegularExpressions.Match;
				global::System.Text.RegularExpressions.GroupCollection groups = match.Groups;
				deviceIdentifier.family = groups[1].Value;
				int num = global::System.Convert.ToInt32(groups[2].Value);
				int num2 = global::System.Convert.ToInt32(groups[3].Value);
				deviceIdentifier.majorMinor = (float)num + (float)num2 * 0.1f;
			}
			return deviceIdentifier;
		}
	}
}
