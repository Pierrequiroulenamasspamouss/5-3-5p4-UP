namespace Kampai.Game
{
	public class Notification
	{
		public string type { get; set; }

		public int secondsFromNow { get; set; }

		public string title { get; set; }

		public string text { get; set; }

		public string stackedTitle { get; set; }

		public string stackedText { get; set; }

		public string sound { get; set; }

		public int badgeNumber { get; set; }

		public Notification()
		{
			sound = string.Empty;
			badgeNumber = 1;
		}

		public override string ToString()
		{
			return string.Format("Type = {0}, SecondsFromNow = {1}, Title = {2}, Message = {3}, Sound = {4}, Badges = {5}", type, secondsFromNow, title, text, sound, badgeNumber);
		}
	}
}
