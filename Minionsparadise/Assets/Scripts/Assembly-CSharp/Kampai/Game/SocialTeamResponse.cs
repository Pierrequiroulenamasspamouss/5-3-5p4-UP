namespace Kampai.Game
{
	public class SocialTeamResponse
	{
		public int EventId { get; set; }

		public global::Kampai.Game.SocialTeam Team { get; set; }

		public global::Kampai.Game.SocialTeamUserEvent UserEvent { get; set; }

		public global::Kampai.Game.SocialEventError Error { get; set; }
	}
}
