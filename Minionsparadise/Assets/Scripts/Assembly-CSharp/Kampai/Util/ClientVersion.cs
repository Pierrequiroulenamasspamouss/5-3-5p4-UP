namespace Kampai.Util
{
	public class ClientVersion : global::Kampai.Util.IClientVersion
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ClientVersion") as global::Kampai.Util.IKampaiLogger;

		private string clientVersion;

		[Inject]
		public ILocalPersistanceService localPersistance { get; set; }

		public string GetClientVersion()
		{
			if (clientVersion == null)
			{
				if (localPersistance.HasKey("OverrideVersion"))
				{
					clientVersion = localPersistance.GetDataInt("OverrideVersion").ToString();
					logger.Warning("Overriding client version: {0}", clientVersion);
					logger.Warning(string.Format("Stack trace: {0}", global::System.Environment.StackTrace));
				}
				else
				{
					clientVersion = global::Kampai.Util.Native.BundleVersion.Split('.')[2];
				}
			}
			return clientVersion;
		}

		public string GetClientPlatform()
		{
			string text = "unknown";
			return "android";
		}

		public string GetClientDeviceType()
		{
			return global::UnityEngine.SystemInfo.deviceModel;
		}

		public void RemoveOverrideVersion()
		{
			if (localPersistance.HasKey("OverrideVersion"))
			{
				switch (localPersistance.GetData("OverrideVersionPersistState"))
				{
				case "keep":
					logger.Info("Keeping the override client version for the next session.");
					localPersistance.PutData("OverrideVersionPersistState", "remove");
					break;
				default:
					logger.Info("Removing the override client version.");
					localPersistance.DeleteKey("OverrideVersionPersistState");
					localPersistance.DeleteKey("OverrideVersion");
					break;
				}
			}
		}
	}
}
