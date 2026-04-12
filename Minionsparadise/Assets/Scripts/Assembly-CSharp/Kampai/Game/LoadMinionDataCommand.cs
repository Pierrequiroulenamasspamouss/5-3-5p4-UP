namespace Kampai.Game
{
	public class LoadMinionDataCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadMinionDataCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService player { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService characterService { get; set; }

		[Inject]
		public global::Kampai.Game.AllMinionLoadedSignal allMinionLoadedSignal { get; set; }

		[Inject]
		public global::Kampai.Util.ICoroutineProgressMonitor coroutineProgressMonitor { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		[Inject]
		public global::Kampai.Game.CreateMinionSignal createMinionSignal { get; set; }

		public override void Execute()
		{
			logger.EventStart("LoadMinionDataCommand.Execute");
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Minion> instancesByType = player.GetInstancesByType<global::Kampai.Game.Minion>();
			global::System.Collections.Generic.List<global::Kampai.Game.Minion> minions = new global::System.Collections.Generic.List<global::Kampai.Game.Minion>(instancesByType);
			SortMinions(minions);
			characterService.Initialize();
			coroutineProgressMonitor.StartTask(LoadMinions(minions), "load minions");
		}

		public void SortMinions(global::System.Collections.Generic.List<global::Kampai.Game.Minion> minions)
		{
			minions.Sort((global::Kampai.Game.Minion x, global::Kampai.Game.Minion y) => x.UTCTaskStartTime.CompareTo(y.UTCTaskStartTime));
		}

		private global::System.Collections.IEnumerator LoadMinions(global::System.Collections.Generic.ICollection<global::Kampai.Game.Minion> minions)
		{
			yield return null;
			pathFinder.AllowWalkableUpdates();
			pathFinder.UpdateWalkableRegion();
			foreach (global::Kampai.Game.Minion m in minions)
			{
				createMinionSignal.Dispatch(m);
				yield return null;
			}
			yield return null;
			allMinionLoadedSignal.Dispatch();
			logger.EventStop("LoadMinionDataCommand.Execute");
		}
	}
}
