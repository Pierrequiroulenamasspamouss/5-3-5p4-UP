namespace Kampai.Game.View
{
	public class MasterPlanComponentBuildingObjectMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.Game.View.MasterPlanComponentBuildingObject view { get; set; }

		[Inject]
		public global::Kampai.Game.CleanupMasterPlanComponentsSignal cleanupComponentSignal { get; set; }

		public override void OnRegister()
		{
			view.cleanupComponentSignal = cleanupComponentSignal;
		}
	}
}
