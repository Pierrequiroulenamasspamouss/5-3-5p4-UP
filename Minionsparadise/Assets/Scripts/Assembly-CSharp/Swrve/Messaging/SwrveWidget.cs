namespace Swrve.Messaging
{
	public abstract class SwrveWidget
	{
		public global::Swrve.Messaging.Point Position;

		public global::Swrve.Messaging.Point Size;

		public global::UnityEngine.Texture2D Texture;

		public global::UnityEngine.Rect Rect;

		public float AnimationScale = 1f;

		public SwrveWidget()
		{
			Position = new global::Swrve.Messaging.Point(0, 0);
			Size = new global::Swrve.Messaging.Point(0, 0);
		}

		public global::Swrve.Messaging.Point GetCenteredPosition(float w, float h, float Scale, float FormatScale)
		{
			int x = (int)((double)((0f - w) * Scale) / 2.0 + (double)((float)Position.X * FormatScale));
			int y = (int)((double)((0f - h) * Scale) / 2.0 + (double)((float)Position.Y * FormatScale));
			return new global::Swrve.Messaging.Point(x, y);
		}
	}
}
