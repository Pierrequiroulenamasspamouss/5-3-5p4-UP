namespace Kampai.Game
{
	public class PartyDefinition
	{
		private global::Kampai.Game.Area normalized;

		public global::Kampai.Game.Area PartyArea { get; set; }

		public global::Kampai.Game.Location Center { get; set; }

		public float Radius { get; set; }

		public int Duration { get; set; }

		public float Percent { get; set; }

		public int StartAnimations { get; set; }

		public PartyDefinition()
		{
		}

		public PartyDefinition(int w, int h)
		{
			PartyArea = new global::Kampai.Game.Area(0, 0, w, h);
		}

		public PartyDefinition(int x1, int y1, int x2, int y2)
		{
			PartyArea = new global::Kampai.Game.Area(x1, y1, x2, y2);
		}

		private void AssertNormalized()
		{
			if (normalized == null)
			{
				global::Kampai.Game.Location a = PartyArea.a;
				global::Kampai.Game.Location b = PartyArea.b;
				int x = global::System.Math.Min(a.x, b.x);
				int y = global::System.Math.Min(a.y, b.y);
				int x2 = global::System.Math.Max(a.x, b.x);
				int y2 = global::System.Math.Max(a.y, b.y);
				normalized = new global::Kampai.Game.Area(x, y, x2, y2);
			}
		}

		public bool Contains(global::Kampai.Util.Point point)
		{
			AssertNormalized();
			global::Kampai.Game.Location a = normalized.a;
			global::Kampai.Game.Location b = normalized.b;
			return point.x >= a.x && point.y >= a.y && point.x <= b.x && point.y <= b.y;
		}
	}
}
