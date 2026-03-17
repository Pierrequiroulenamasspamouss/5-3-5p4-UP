namespace Kampai.Game
{
	public class DCNContent
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Type { get; set; }

		public string Mime_Type { get; set; }

		public global::System.DateTime Created_At { get; set; }

		public global::System.DateTime Updated_At { get; set; }

		public global::System.DateTime Expires_In { get; set; }

		public global::System.Collections.Generic.IDictionary<string, string> Urls { get; set; }

		public bool Featured { get; set; }
	}
}
