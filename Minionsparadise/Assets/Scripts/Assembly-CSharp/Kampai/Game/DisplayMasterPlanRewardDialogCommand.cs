namespace Kampai.Game
{
	internal sealed class DisplayMasterPlanRewardDialogCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MasterPlan masterPlan { get; set; }

		[Inject]
		public global::Kampai.Game.StartMasterPlanCooldownSignal startCooldownSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayfinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			startCooldownSignal.Dispatch(masterPlan);
			global::Kampai.Game.Character firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Character>(70004);
			removeWayfinderSignal.Dispatch(firstInstanceByDefinitionId.ID);
		}
	}
}
