namespace Kampai.Game
{
	public class AccountLinkErrorResponseContent
	{
		public int code { get; set; }

		public int responseCode { get; set; }

		public string description { get; set; }

		public string message { get; set; }

		public global::Kampai.Game.AccountLinkErrorResponseContentDetails details { get; set; }

		public string exceptionDetails { get; set; }
	}
}
