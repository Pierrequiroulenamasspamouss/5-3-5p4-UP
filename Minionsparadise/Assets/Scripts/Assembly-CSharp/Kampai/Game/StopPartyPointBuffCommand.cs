namespace Kampai.Game
{
	public class StopPartyPointBuffCommand : global::strange.extensions.command.impl.Command
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
			timeEventService.StopBuff(global::Kampai.Game.TimeEventType.LeisureBuff, tuple.Item2);
			AdjustLeisureBuildingTimes(item, tuple.Item2, tuple.Item3);
		}

		private void AdjustLeisureBuildingTimes(int buffStartTime, int buffDuration, float multipler)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.LeisureBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.LeisureBuilding>();
			foreach (global::Kampai.Game.LeisureBuilding item in instancesByType)
			{
				if (item.State == global::Kampai.Game.BuildingState.Working)
				{
					if (buffStartTime < item.UTCLastTaskingTimeStarted)
					{
						int num = buffDuration + buffStartTime - item.UTCLastTaskingTimeStarted;
						item.UTCLastTaskingTimeStarted -= (int)((float)num * multipler);
					}
					else
					{
						item.UTCLastTaskingTimeStarted -= (int)((float)buffDuration * multipler);
					}
				}
			}
		}
	}
}
