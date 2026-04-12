namespace Kampai.Game.Trigger
{
	public class TriggerRewardLayout
	{
		public enum Layout
		{
			None = 0,
			Horizontal = 1,
			Vertical = 2
		}

		public int index;

		public global::System.Collections.Generic.IList<int> itemIds;

		public global::Kampai.Game.Trigger.TriggerRewardLayout.Layout layout;
	}
}
