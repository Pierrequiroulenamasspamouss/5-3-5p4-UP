namespace Kampai.Game.View
{
	public class RestoreTaskableBuildingCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RestoreTaskableBuildingCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.TaskableBuilding building { get; set; }

		[Inject]
		public global::Kampai.Game.HarvestReadySignal harvestSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal changeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		public override void Execute()
		{
			logger.Debug("Restoring a Tasking Building");
			global::Kampai.Game.BuildingState newState = global::Kampai.Game.BuildingState.Inactive;
			if (building is global::Kampai.Game.MignetteBuilding)
			{
				newState = global::Kampai.Game.BuildingState.Idle;
			}
			else
			{
				global::Kampai.Game.ResourceBuilding resourceBuilding = building as global::Kampai.Game.ResourceBuilding;
				if (building.GetMinionsInBuilding() > 0)
				{
					newState = global::Kampai.Game.BuildingState.Working;
				}
				else if (resourceBuilding != null && resourceBuilding.GetTotalHarvests() > 0)
				{
					newState = global::Kampai.Game.BuildingState.Harvestable;
					harvestSignal.Dispatch(building.ID);
				}
			}
			if (newState != global::Kampai.Game.BuildingState.Inactive)
			{
				routineRunner.StartCoroutine(WaitAFrame(delegate
				{
					changeStateSignal.Dispatch(building.ID, newState);
				}));
			}
		}

		private global::System.Collections.IEnumerator WaitAFrame(global::System.Action a)
		{
			yield return null;
			a();
		}
	}
}
