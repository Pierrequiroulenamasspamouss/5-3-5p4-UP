namespace Kampai.Game
{
	public class StartLeisurePartyPointsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("StartLeisurePartyPointsCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.LeisureBuilding building;

		private global::Kampai.Game.LeisureBuildingDefintiion definition;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.HarvestReadySignal harvestSignal { get; set; }

		[Inject]
		public int buildingId { get; set; }

		public override void Execute()
		{
			building = playerService.GetByInstanceId<global::Kampai.Game.LeisureBuilding>(buildingId);
			if (building == null)
			{
				logger.Error("Invalid leisure building with id {0}", buildingId);
				return;
			}
			definition = building.Definition;
			if (definition.PartyPointsReward <= 0)
			{
				logger.Error("Invalid party points reward");
				return;
			}
			buildingStateChangeSignal.Dispatch(building.ID, global::Kampai.Game.BuildingState.Working);
			timeEventService.AddEvent(building.ID, building.UTCLastTaskingTimeStarted, building.Definition.LeisureTimeDuration, harvestSignal, global::Kampai.Game.TimeEventType.LeisureBuff);
		}
	}
}
