namespace Kampai.Game
{
	public class AwardLairBonusDropsThenSetHarvestReadyCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int buildingID { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.HarvestReadySignal harvestReadySignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveMinionFromLairResourcePlotSignal removeMinionFromLairResourcePlotSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateVillainLairMenuViewSignal updateVillainLairMenuViewSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.VillainLairResourcePlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairResourcePlot>(buildingID);
			global::Kampai.Game.Minion byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.Minion>(byInstanceId.LastMinionTasked);
			removeMinionFromLairResourcePlotSignal.Dispatch(byInstanceId.ID);
			buildingStateChangeSignal.Dispatch(byInstanceId.ID, global::Kampai.Game.BuildingState.Harvestable);
			updateVillainLairMenuViewSignal.Dispatch();
			CheckBonusItems(byInstanceId, byInstanceId2);
			byInstanceId.harvestCount++;
			harvestReadySignal.Dispatch(byInstanceId.ID);
		}

		private void CheckBonusItems(global::Kampai.Game.VillainLairResourcePlot resourcePlot, global::Kampai.Game.Minion minion)
		{
			global::Kampai.Game.MinionBenefitLevelBandDefintion minionBenefitLevelBandDefintion = definitionService.Get<global::Kampai.Game.MinionBenefitLevelBandDefintion>(global::Kampai.Game.StaticItem.MINION_BENEFITS_DEF_ID);
			global::Kampai.Game.MinionBenefitLevel minionBenefit = minionBenefitLevelBandDefintion.GetMinionBenefit(minion.Level);
			if (randomService.NextFloat() < minionBenefit.doubleDropPercentage)
			{
				resourcePlot.BonusMinionItems.Add(resourcePlot.parentLair.Definition.ResourceItemID);
			}
			if (randomService.NextFloat() < minionBenefit.premiumDropPercentage)
			{
				resourcePlot.BonusMinionItems.Add(1);
			}
			if (randomService.NextFloat() < minionBenefit.rareDropPercentage)
			{
				int iD = playerService.GetWeightedInstance(4000).NextPick(randomService).ID;
				resourcePlot.BonusMinionItems.Add(iD);
			}
		}
	}
}
