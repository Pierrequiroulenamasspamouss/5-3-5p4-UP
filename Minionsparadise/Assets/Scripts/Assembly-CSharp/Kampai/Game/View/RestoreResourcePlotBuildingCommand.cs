namespace Kampai.Game.View
{
	public class RestoreResourcePlotBuildingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.VillainLairResourcePlot resourcePlot { get; set; }

		[Inject]
		public global::Kampai.Game.HarvestReadySignal harvestSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.AwardLairBonusDropsThenSetHarvestReadySignal awardDropThenHarvestReadySignal { get; set; }

		public override void Execute()
		{
			routineRunner.StartCoroutine(RestoreBuilding());
		}

		private global::System.Collections.IEnumerator RestoreBuilding()
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			int plotID = resourcePlot.ID;
			global::Kampai.Game.BuildingState oldState = resourcePlot.State;
			if (resourcePlot.harvestCount > 0 || resourcePlot.BonusMinionItems.Count > 0)
			{
				harvestSignal.Dispatch(plotID);
			}
			if (oldState == global::Kampai.Game.BuildingState.Working)
			{
				int secondsToHarvest = resourcePlot.parentLair.Definition.SecondsToHarvest;
				timeEventService.AddEvent(plotID, resourcePlot.UTCLastTaskingTimeStarted, secondsToHarvest, awardDropThenHarvestReadySignal, global::Kampai.Game.TimeEventType.ProductionBuff);
			}
		}
	}
}
