namespace Kampai.Game
{
	public class SocialEventError
	{
		public enum ErrorType
		{
			TEAM_FULL = 0,
			ORDER_ALREADY_FILLED = 1
		}

		public global::Kampai.Game.SocialEventError.ErrorType Type { get; set; }
	}
}
