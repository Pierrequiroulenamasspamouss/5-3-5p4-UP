namespace Kampai.Game
{
	public class TemporaryMinionsService
	{
		private global::System.Collections.Generic.IDictionary<int, global::Kampai.Game.View.MinionObject> temporaryMinions = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.MinionObject>();

		public void addTemporaryMinion(global::Kampai.Game.View.MinionObject mo)
		{
			if (mo != null)
			{
				mo.isTemporaryMinion = true;
				temporaryMinions.Add(mo.ID, mo);
			}
		}

		public void removeTemporaryMinion(int minionId)
		{
			temporaryMinions.Remove(minionId);
		}

		public global::System.Collections.Generic.IDictionary<int, global::Kampai.Game.View.MinionObject> getTemporaryMinions()
		{
			return new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.MinionObject>(temporaryMinions);
		}

		public global::Kampai.Game.View.MinionObject GetMinion(int minionID)
		{
			global::Kampai.Game.View.MinionObject value;
			temporaryMinions.TryGetValue(minionID, out value);
			return value;
		}
	}
}
