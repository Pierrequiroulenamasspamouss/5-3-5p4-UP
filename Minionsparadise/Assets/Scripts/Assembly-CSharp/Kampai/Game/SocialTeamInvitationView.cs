namespace Kampai.Game
{
	public class SocialTeamInvitationView
	{
		[global::Newtonsoft.Json.JsonProperty("id")]
		public long TeamID { get; set; }

		public int SocialEventId { get; set; }

		public int MembersCount { get; set; }

		public int CompletedOrdersCount { get; set; }
	}
}
