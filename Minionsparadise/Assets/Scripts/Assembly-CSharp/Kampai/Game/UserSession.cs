namespace Kampai.Game
{
	public class UserSession
	{
		[global::Newtonsoft.Json.JsonProperty("userId")]
		public string UserID { get; set; }

		[global::Newtonsoft.Json.JsonProperty("sessionId")]
		public string SessionID { get; set; }

		[global::Newtonsoft.Json.JsonProperty("synergyId")]
		public string SynergyID { get; set; }

		[global::Newtonsoft.Json.JsonProperty("socialIdentities")]
		public global::System.Collections.Generic.IList<global::Kampai.Game.UserIdentity> SocialIdentities { get; set; }

		[global::Newtonsoft.Json.JsonProperty("logEnabled")]
		public bool LogEnabled { get; set; }

		[global::Newtonsoft.Json.JsonProperty("logLevel")]
		public int LogLevel { get; set; }
	}
}
