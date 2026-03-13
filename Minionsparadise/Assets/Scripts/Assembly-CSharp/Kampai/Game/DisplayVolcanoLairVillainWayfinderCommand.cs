namespace Kampai.Game
{
	public class DisplayVolcanoLairVillainWayfinderCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService planService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateWayFinderSignal createWayFinderSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.NamedCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.NamedCharacter>(70004);
			if (firstInstanceByDefinitionId == null)
			{
				return;
			}
			global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(40001);
			if (prestige == null)
			{
				return;
			}
			global::Kampai.Game.VillainLair firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.VillainLair>(3137);
			if (firstInstanceByDefinitionId2 == null || !firstInstanceByDefinitionId2.hasVisited)
			{
				return;
			}
			global::Kampai.Game.MasterPlan currentMasterPlan = planService.CurrentMasterPlan;
			if (currentMasterPlan == null)
			{
				return;
			}
			global::Kampai.Game.MasterPlanDefinition definition = currentMasterPlan.Definition;
			global::Kampai.Game.MasterPlanComponentBuilding firstInstanceByDefinitionId3 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponentBuilding>(definition.BuildingDefID);
			if (firstInstanceByDefinitionId3 == null || firstInstanceByDefinitionId3.State != global::Kampai.Game.BuildingState.Complete)
			{
				global::Kampai.Game.MasterPlanComponent activeComponentFromPlanDefinition = planService.GetActiveComponentFromPlanDefinition(definition.ID);
				if (activeComponentFromPlanDefinition == null || activeComponentFromPlanDefinition.State != global::Kampai.Game.MasterPlanComponentState.Scaffolding)
				{
					createWayFinderSignal.Dispatch(new global::Kampai.UI.View.WayFinderSettings(firstInstanceByDefinitionId.ID));
				}
			}
		}
	}
}
