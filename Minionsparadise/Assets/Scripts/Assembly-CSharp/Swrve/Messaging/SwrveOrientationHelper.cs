namespace Swrve.Messaging
{
	public static class SwrveOrientationHelper
	{
		public static global::Swrve.Messaging.SwrveOrientation Parse(string orientation)
		{
			if (orientation.ToLower().Equals("portrait"))
			{
				return global::Swrve.Messaging.SwrveOrientation.Portrait;
			}
			if (orientation.ToLower().Equals("both"))
			{
				return global::Swrve.Messaging.SwrveOrientation.Both;
			}
			return global::Swrve.Messaging.SwrveOrientation.Landscape;
		}
	}
}
