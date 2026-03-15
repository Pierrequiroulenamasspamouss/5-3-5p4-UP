namespace Kampai.Game.View
{
	public class VolcanoEntranceMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.CreateWayFinderSignal createWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			CreateWayfinder();
		}

		private void CreateWayfinder()
		{
			global::Kampai.Game.VillainLair firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.VillainLair>(3137);
			if (firstInstanceByDefinitionId == null)
			{
				return;
			}
			global::Kampai.Game.VillainLairEntranceBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(firstInstanceByDefinitionId.portalInstanceID);
			if (byInstanceId == null || byInstanceId.State != global::Kampai.Game.BuildingState.Inaccessible)
			{
				global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
				if (currentMasterPlan == null || currentMasterPlan.cooldownUTCStartTime <= 0)
				{
					global::Kampai.UI.View.WayFinderSettings type = new global::Kampai.UI.View.WayFinderSettings(374);
					createWayFinderSignal.Dispatch(type);
				}
			}
		}
	}
}
