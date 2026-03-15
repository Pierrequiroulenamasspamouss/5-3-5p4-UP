namespace Kampai.Game
{
	public class StopProductionBuffCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Util.Tuple<int, int, float> tuple { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		public override void Execute()
		{
			int item = tuple.Item1;
			timeEventService.StopBuff(global::Kampai.Game.TimeEventType.ProductionBuff, tuple.Item2);
			AdjustMinionTaskingTimes(item, tuple.Item2, tuple.Item3);
			AdjustCraftingBuildingTimes(item, tuple.Item2, tuple.Item3);
		}

		private void AdjustMinionTaskingTimes(int buffStartTime, int buffDuration, float multipler)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Minion> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Minion>();
			foreach (global::Kampai.Game.Minion item in instancesByType)
			{
				item.IsInMinionParty = false;
				if (item.State != global::Kampai.Game.MinionState.Tasking)
				{
					continue;
				}
				if (buffStartTime < item.UTCTaskStartTime)
				{
					int num = buffDuration + buffStartTime - item.UTCTaskStartTime;
					if (num < 0)
					{
						num = 0;
					}
					item.PartyTimeReduction += (int)((float)num * multipler);
				}
				else
				{
					item.PartyTimeReduction += (int)((float)buffDuration * multipler);
				}
			}
		}

		private void AdjustCraftingBuildingTimes(int buffStartTime, int buffDuration, float multipler)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.CraftingBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.CraftingBuilding>();
			foreach (global::Kampai.Game.CraftingBuilding item in instancesByType)
			{
				if (item.State != global::Kampai.Game.BuildingState.Working && item.State != global::Kampai.Game.BuildingState.HarvestableAndWorking)
				{
					continue;
				}
				if (buffStartTime < item.CraftingStartTime)
				{
					int num = buffDuration + buffStartTime - item.CraftingStartTime;
					if (num < 0)
					{
						num = 0;
					}
					item.PartyTimeReduction += (int)((float)num * multipler);
				}
				else
				{
					item.PartyTimeReduction += (int)((float)buffDuration * multipler);
				}
			}
		}
	}
}
