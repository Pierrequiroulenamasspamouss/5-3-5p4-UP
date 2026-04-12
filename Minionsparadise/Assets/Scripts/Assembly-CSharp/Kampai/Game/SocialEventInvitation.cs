namespace Kampai.Game
{
	public class SocialEventInvitation
	{
		[global::Newtonsoft.Json.JsonProperty("eventId")]
		public int EventID { get; set; }

		[global::Newtonsoft.Json.JsonProperty("team")]
		public global::Kampai.Game.SocialTeamInvitationView Team { get; set; }

		[global::Newtonsoft.Json.JsonProperty("inviter")]
		public global::Kampai.Game.UserIdentity inviter { get; set; }
	}
}
