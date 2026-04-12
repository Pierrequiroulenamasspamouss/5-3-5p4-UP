namespace Kampai.Util
{
	public class GracefulMessage
	{
		public string Title { get; private set; }

		public string Description { get; private set; }

		public GracefulMessage(string title, string descripiton)
		{
			Title = title;
			Description = descripiton;
		}
	}
}
