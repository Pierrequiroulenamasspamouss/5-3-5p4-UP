namespace Kampai.Game.View
{
	public class RestoreLeisureBuildingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal changeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartLeisurePartyPointsSignal startLeisurePartyPointsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.LeisureBuilding building { get; set; }

		[Inject]
		public global::Kampai.Game.HarvestReadySignal harvestSignal { get; set; }

		public override void Execute()
		{
			if (building.State == global::Kampai.Game.BuildingState.Harvestable)
			{
				harvestSignal.Dispatch(building.ID);
				return;
			}
			global::Kampai.Game.BuildingState buildingState = global::Kampai.Game.BuildingState.Inactive;
			int num = timeService.CurrentTime() - building.UTCLastTaskingTimeStarted;
			if (num > building.Definition.LeisureTimeDuration)
			{
				if (building.UTCLastTaskingTimeStarted != 0)
				{
					startLeisurePartyPointsSignal.Dispatch(building.ID);
					buildingState = global::Kampai.Game.BuildingState.Harvestable;
				}
				else
				{
					buildingState = global::Kampai.Game.BuildingState.Idle;
				}
			}
			else
			{
				buildingState = global::Kampai.Game.BuildingState.Working;
			}
			changeStateSignal.Dispatch(building.ID, buildingState);
		}
	}
}
