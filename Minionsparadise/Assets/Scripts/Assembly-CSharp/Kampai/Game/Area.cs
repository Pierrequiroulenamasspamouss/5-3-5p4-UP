namespace Kampai.Game
{
	public class Area
	{
		public global::Kampai.Game.Location a { get; set; }

		public global::Kampai.Game.Location b { get; set; }

		public Area()
		{
		}

		public Area(int x1, int y1, int x2, int y2)
		{
			a = new global::Kampai.Game.Location(x1, y1);
			b = new global::Kampai.Game.Location(x2, y2);
		}
	}
}
