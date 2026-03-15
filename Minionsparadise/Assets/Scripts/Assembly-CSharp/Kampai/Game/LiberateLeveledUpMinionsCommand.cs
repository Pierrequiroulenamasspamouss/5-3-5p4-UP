namespace Kampai.Game
{
	public class LiberateLeveledUpMinionsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LiberateLeveledUpMinionsCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int minLevel { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.RushTaskSignal rushTaskSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(3002);
			foreach (global::Kampai.Game.Minion item in playerService.GetInstancesByType<global::Kampai.Game.Minion>())
			{
				if (item.BuildingID != firstInstanceByDefinitionId.ID && item.Level >= minLevel && item.State == global::Kampai.Game.MinionState.Tasking)
				{
					logger.Info("Liberating {0}", item.ID);
					rushTaskSignal.Dispatch(item.ID);
				}
			}
		}
	}
}
