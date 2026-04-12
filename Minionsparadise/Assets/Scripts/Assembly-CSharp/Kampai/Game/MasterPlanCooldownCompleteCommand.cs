namespace Kampai.Game
{
	public class MasterPlanCooldownCompleteCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayVolcanoLairVillainWayfinderSignal displayVolcanoWayfinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideFluxWayfinder hideFluxWayfinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayVillainSignal displayVillainSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanCooldownRewardDialogSignal displayCooldownRewardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CleanupMasterPlanSignal cleanupPlanSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateWayFinderSignal createWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ResetLairWayfinderIconSignal resetIconSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			cleanupPlanSignal.Dispatch(currentMasterPlan);
			currentMasterPlan.cooldownUTCStartTime = 0;
			displayVolcanoWayfinderSignal.Dispatch();
			hideFluxWayfinderSignal.Dispatch(false);
			global::Kampai.UI.View.WayFinderSettings type = new global::Kampai.UI.View.WayFinderSettings(374);
			createWayFinderSignal.Dispatch(type);
			resetIconSignal.Dispatch();
			currentMasterPlan.displayCooldownReward = true;
			if (villainLairModel.currentActiveLair != null)
			{
				displayVillainSignal.Dispatch(currentMasterPlan.Definition.VillainCharacterDefID, true);
				displayCooldownRewardSignal.Dispatch();
			}
			else
			{
				villainLairModel.seenCooldownAlert = false;
			}
		}
	}
}
