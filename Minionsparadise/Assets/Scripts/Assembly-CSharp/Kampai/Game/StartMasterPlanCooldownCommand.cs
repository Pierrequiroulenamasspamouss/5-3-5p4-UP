namespace Kampai.Game
{
	public class StartMasterPlanCooldownCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MasterPlan masterPlan { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanCooldownCompleteSignal cooldownCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayfinderSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		public override void Execute()
		{
			int num = timeService.CurrentTime();
			masterPlan.cooldownUTCStartTime = num;
			timeEventService.AddEvent(masterPlan.ID, num, masterPlan.Definition.CooldownDuration, cooldownCompleteSignal);
			masterPlan.displayCooldownAlert = true;
			global::Kampai.Game.Villain firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Villain>(70004);
			removeWayfinderSignal.Dispatch(firstInstanceByDefinitionId.ID);
			removeWayfinderSignal.Dispatch(374);
			int gameTimeDuration = playerDurationService.GetGameTimeDuration(masterPlan);
			global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(masterPlan.Definition.BuildingDefID);
			telemetryService.Send_Telemetry_EVT_MASTER_PLAN_COMPLETE(masterPlanComponentBuildingDefinition.TaxonomySpecific, firstInstanceByDefinitionId.Definition.LocalizedKey, gameTimeDuration);
		}
	}
}
