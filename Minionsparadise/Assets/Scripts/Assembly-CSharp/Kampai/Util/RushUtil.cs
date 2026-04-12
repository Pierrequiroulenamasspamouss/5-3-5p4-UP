namespace Kampai.Util
{
	public static class RushUtil
	{
		public static void SortByTime(global::System.Collections.Generic.IList<global::Kampai.Game.RushTimeBandDefinition> items, bool ascending = true)
		{
			(items as global::System.Collections.Generic.List<global::Kampai.Game.RushTimeBandDefinition>).Sort((global::Kampai.Game.RushTimeBandDefinition p1, global::Kampai.Game.RushTimeBandDefinition p2) => (!ascending) ? (p2.TimeRemainingInSeconds - p1.TimeRemainingInSeconds) : (p1.TimeRemainingInSeconds - p2.TimeRemainingInSeconds));
		}
	}
}
