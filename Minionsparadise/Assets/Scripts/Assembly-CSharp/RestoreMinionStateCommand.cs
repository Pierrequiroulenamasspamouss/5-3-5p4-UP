public class RestoreMinionStateCommand : global::strange.extensions.command.impl.Command
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RestoreMinionStateCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public int minionID { get; set; }

	[Inject]
	public global::Kampai.Game.IPlayerService playerService { get; set; }

	[Inject]
	public global::Kampai.Game.IDefinitionService definitionService { get; set; }

	[Inject]
	public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

	[Inject]
	public global::Kampai.Game.ITimeService timeService { get; set; }

	[Inject]
	public global::Kampai.Common.MinionTaskCompleteSignal taskCompleteSignal { get; set; }

	[Inject]
	public global::Kampai.Game.StartTeleportTaskSignal teleportTaskSignal { get; set; }

	[Inject]
	public global::Kampai.Game.EnableMinionRendererSignal enableRendererSignal { get; set; }

	[Inject]
	public global::Kampai.Game.RestoreMinionAtTikiBarSignal restoreMinionAtTikiBarSignal { get; set; }

	[Inject]
	public global::Kampai.Game.TeleportMinionToLeisureSignal teleportMinionToLeisureSignal { get; set; }

	public override void Execute()
	{
		global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionID);
		if (byInstanceId == null)
		{
			logger.Fatal(global::Kampai.Util.FatalCode.CMD_RESTORE_MINION);
			return;
		}
		if (byInstanceId.IsInMinionParty)
		{
			if (byInstanceId.UTCTaskStartTime > 0 && byInstanceId.TaskDuration > 0)
			{
				byInstanceId.IsInMinionParty = false;
				byInstanceId.State = global::Kampai.Game.MinionState.Tasking;
			}
			else if (byInstanceId.State == global::Kampai.Game.MinionState.Idle && byInstanceId.BuildingID != -1)
			{
				global::Kampai.Game.Building byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.Building>(byInstanceId.BuildingID);
				if (byInstanceId2 is global::Kampai.Game.LeisureBuilding)
				{
					byInstanceId.IsInMinionParty = false;
					byInstanceId.State = global::Kampai.Game.MinionState.Leisure;
				}
			}
		}
		if (byInstanceId.State == global::Kampai.Game.MinionState.Tasking)
		{
			if (!RestoreResourcePlotTaskingMinionState(byInstanceId))
			{
				RestoreTaskingMinionState(byInstanceId);
			}
		}
		else if (byInstanceId.State == global::Kampai.Game.MinionState.Selected || byInstanceId.State == global::Kampai.Game.MinionState.Selectable || byInstanceId.State == global::Kampai.Game.MinionState.WaitingOnMagnetFinger || byInstanceId.State == global::Kampai.Game.MinionState.PlayingMignette || byInstanceId.State == global::Kampai.Game.MinionState.Unselectable)
		{
			byInstanceId.State = global::Kampai.Game.MinionState.Idle;
		}
		else if (byInstanceId.State == global::Kampai.Game.MinionState.Questing)
		{
			if (!byInstanceId.HasPrestige)
			{
				byInstanceId.State = global::Kampai.Game.MinionState.Idle;
			}
			else
			{
				restoreMinionAtTikiBarSignal.Dispatch(byInstanceId);
			}
		}
		else if (byInstanceId.State == global::Kampai.Game.MinionState.Leisure)
		{
			RestoreLeisureMinionState(byInstanceId);
		}
		if (byInstanceId.State == global::Kampai.Game.MinionState.Idle || byInstanceId.State.Equals(global::Kampai.Game.MinionState.Uninitialized))
		{
			enableRendererSignal.Dispatch(byInstanceId.ID, true);
		}
	}

	private bool RestoreResourcePlotTaskingMinionState(global::Kampai.Game.Minion minion)
	{
		global::Kampai.Game.VillainLairResourcePlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairResourcePlot>(minion.BuildingID);
		if (byInstanceId == null)
		{
			return false;
		}
		if (byInstanceId.State == global::Kampai.Game.BuildingState.Working && byInstanceId.UTCLastTaskingTimeStarted > 0)
		{
			teleportMinionToLeisureSignal.Dispatch(minion);
		}
		else
		{
			minion.State = global::Kampai.Game.MinionState.Idle;
		}
		return true;
	}

	private void RestoreTaskingMinionState(global::Kampai.Game.Minion minion)
	{
		global::Kampai.Game.TaskableBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TaskableBuilding>(minion.BuildingID);
		if (minion.TaskDuration == 0 || minion.UTCTaskStartTime + minion.TaskDuration - minion.PartyTimeReduction <= timeService.CurrentTime())
		{
			if (byInstanceId != null)
			{
				if (!(byInstanceId is global::Kampai.Game.ResourceBuilding))
				{
					minion.AlreadyRushed = true;
					teleportTaskSignal.Dispatch(minion, byInstanceId);
					byInstanceId.AddMinion(minionID, minion.UTCTaskStartTime);
				}
			}
			else
			{
				minion.State = global::Kampai.Game.MinionState.Idle;
			}
			taskCompleteSignal.Dispatch(minionID);
		}
		else
		{
			teleportTaskSignal.Dispatch(minion, byInstanceId);
			int eventTime = BuildingUtil.GetHarvestTimeForTaskableBuilding(byInstanceId, definitionService) - minion.PartyTimeReduction;
			timeEventService.AddEvent(minionID, minion.UTCTaskStartTime, eventTime, taskCompleteSignal, global::Kampai.Game.TimeEventType.ProductionBuff);
			byInstanceId.AddMinion(minionID, minion.UTCTaskStartTime);
		}
	}

	private void RestoreLeisureMinionState(global::Kampai.Game.Minion minion)
	{
		global::Kampai.Game.LeisureBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.LeisureBuilding>(minion.BuildingID);
		if (byInstanceId.State == global::Kampai.Game.BuildingState.Working && byInstanceId.UTCLastTaskingTimeStarted > 0)
		{
			byInstanceId.AddMinion(minion.ID, byInstanceId.UTCLastTaskingTimeStarted);
			teleportMinionToLeisureSignal.Dispatch(minion);
		}
		else
		{
			minion.State = global::Kampai.Game.MinionState.Idle;
		}
	}
}
