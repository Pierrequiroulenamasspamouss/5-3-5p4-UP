namespace Kampai.Game
{
	public class SendMinionToLairResourcePlotCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SendMinionToLairResourcePlotCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int buildingId { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RouteMinionToLairResourcePlotSignal routeMinionToPlotSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playMinionNoAnimAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.AwardLairBonusDropsThenSetHarvestReadySignal awardDropsThenHarvestReadySignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			global::Kampai.Game.View.BuildingManagerView component2 = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.VillainLairResourcePlotObjectView villainLairResourcePlotObjectView = component2.GetBuildingObject(buildingId) as global::Kampai.Game.View.VillainLairResourcePlotObjectView;
			global::Kampai.Game.VillainLairResourcePlot resourcePlot = villainLairResourcePlotObjectView.resourcePlot;
			if (resourcePlot != null && !(villainLairResourcePlotObjectView == null))
			{
				global::Kampai.Game.Minion untaskedMinionWithHighestLevel = playerService.GetUntaskedMinionWithHighestLevel();
				global::Kampai.Game.View.MinionObject minionObject = component.Get(untaskedMinionWithHighestLevel.ID);
				playMinionNoAnimAudioSignal.Dispatch("Play_minion_confirm_pathToBldg_01");
				HandlePathing(minionObject, resourcePlot);
				buildingStateChangeSignal.Dispatch(resourcePlot.ID, global::Kampai.Game.BuildingState.Working);
			}
		}

		private void HandlePathing(global::Kampai.Game.View.MinionObject minionObject, global::Kampai.Game.VillainLairResourcePlot resourcePlot)
		{
			global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionObject.ID);
			if (byInstanceId == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_NO_SUCH_MINION, "{0}", minionObject.ID);
			}
			byInstanceId.BuildingID = buildingId;
			minionStateChangeSignal.Dispatch(byInstanceId.ID, global::Kampai.Game.MinionState.Tasking);
			minionObject.transform.position = (global::UnityEngine.Vector3)resourcePlot.parentLair.Definition.MinionArrivalOffset + (global::UnityEngine.Vector3)resourcePlot.parentLair.Definition.Location;
			routeMinionToPlotSignal.Dispatch(minionObject, resourcePlot.ID);
			resourcePlot.AddMinion(minionObject.ID, timeService.CurrentTime());
			timeEventService.AddEvent(resourcePlot.ID, resourcePlot.UTCLastTaskingTimeStarted, resourcePlot.parentLair.Definition.SecondsToHarvest, awardDropsThenHarvestReadySignal, global::Kampai.Game.TimeEventType.ProductionBuff);
		}
	}
}
