namespace Kampai.Game
{
	public class DCNToken
	{
		public string Token { get; set; }

		public global::System.DateTime Expires_In { get; set; }

		public bool IsValid()
		{
			if (string.IsNullOrEmpty(Token))
			{
				return false;
			}
			if (Expires_In <= global::System.DateTime.Now)
			{
				return false;
			}
			return true;
		}
	}
}
