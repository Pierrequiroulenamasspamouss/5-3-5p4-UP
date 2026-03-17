namespace Kampai.Game
{
	public class RushOneMinionInBuildingCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RushOneMinionInBuildingCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int BuildingID { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.RushTaskSignal rushMinionSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.TaskableBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TaskableBuilding>(BuildingID);
			if (byInstanceId == null)
			{
				logger.Error("Trying to rush minions in a non-taskable building or a non-building.");
				return;
			}
			int num = -1;
			float num2 = -1f;
			foreach (int minion in byInstanceId.MinionList)
			{
				global::Kampai.Game.Minion byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minion);
				if (byInstanceId2 != null && (num < 0 || (float)byInstanceId2.UTCTaskStartTime < num2))
				{
					num = minion;
					num2 = byInstanceId2.UTCTaskStartTime;
				}
			}
			if (num >= 0)
			{
				rushMinionSignal.Dispatch(num);
			}
		}
	}
}
