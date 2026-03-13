namespace Kampai.Game
{
	public class RandomizeMinionPositionsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RandomizeMinionPositionsCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService player { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			global::System.Collections.Generic.Queue<int> minionListSortedByDistanceAndState = component.GetMinionListSortedByDistanceAndState(global::UnityEngine.Vector3.zero, false);
			while (minionListSortedByDistanceAndState.Count > 0)
			{
				int num = minionListSortedByDistanceAndState.Dequeue();
				global::Kampai.Game.View.MinionObject minionObject = component.Get(num);
				global::Kampai.Game.Minion byInstanceId = player.GetByInstanceId<global::Kampai.Game.Minion>(num);
				minionObject.transform.position = pathFinder.RandomPosition(byInstanceId.Partying);
			}
			logger.Debug("RandomizeMinionPositionsCommand: Completed");
		}
	}
}
