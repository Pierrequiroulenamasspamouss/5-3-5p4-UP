namespace Kampai.Game
{
	public class ScreenPosition
	{
		public float x { get; set; }

		public float z { get; set; }

		public float zoom { get; set; }

		public ScreenPosition(float x = 0.5f, float z = 0.5f, float zoom = -1f)
		{
			this.x = x;
			this.z = z;
			this.zoom = zoom;
		}

		public global::Kampai.Game.ScreenPosition Clone(global::Kampai.Game.ScreenPosition other)
		{
			global::Kampai.Game.ScreenPosition screenPosition = (global::Kampai.Game.ScreenPosition)MemberwiseClone();
			screenPosition.x = other.x;
			screenPosition.z = other.z;
			screenPosition.zoom = other.zoom;
			return screenPosition;
		}
	}
}
