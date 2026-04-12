namespace Kampai.Game
{
	public class UserIdentity
	{
		[global::Newtonsoft.Json.JsonProperty("id")]
		public string ID { get; set; }

		[global::Newtonsoft.Json.JsonProperty("externalId")]
		public string ExternalID { get; set; }

		[global::Newtonsoft.Json.JsonProperty("userId")]
		public string UserID { get; set; }

		[global::Newtonsoft.Json.JsonProperty("type")]
		public global::Kampai.Game.IdentityType Type { get; set; }
	}
}
