namespace Kampai.Util.Logging.Hosted
{
	public class LogglyDto
	{
		public string LogLevel { get; set; }

		public string Timestamp { get; set; }

		public string Message { get; set; }

		public string ClientVersion { get; set; }

		public string ClientDeviceType { get; set; }

		public string ClientPlatform { get; set; }

		public string NewUser { get; set; }

		public string UserId { get; set; }

		public string SynergyId { get; set; }

		public string ConfigUrl { get; set; }

		public string ConfigVariant { get; set; }

		public string DefinitionId { get; set; }

		public global::System.Collections.Generic.IList<string> DefinitionVariants { get; set; }

		public string Geolocation { get; set; }
	}
}
