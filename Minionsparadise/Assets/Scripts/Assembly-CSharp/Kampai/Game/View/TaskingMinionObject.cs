namespace Kampai.Game.View
{
	public class TaskingMinionObject
	{
		private global::Kampai.Game.View.MinionObject minionObject;

		public bool IsResting;

		public int RoutingIndex;

		public int ID
		{
			get
			{
				return minionObject.ID;
			}
		}

		public global::Kampai.Game.View.MinionObject Minion
		{
			get
			{
				return minionObject;
			}
		}

		public TaskingMinionObject(global::Kampai.Game.View.MinionObject delegateMinionObject, int routingIndex)
		{
			minionObject = delegateMinionObject;
			RoutingIndex = routingIndex;
		}
	}
}
