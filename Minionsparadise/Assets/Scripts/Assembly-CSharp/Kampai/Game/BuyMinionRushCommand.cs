namespace Kampai.Game
{
	public class BuyMinionRushCommand : global::strange.extensions.command.impl.Command
	{
		private BuildingType.BuildingTypeIdentifier rushedBuildingType;

		[Inject]
		public int minionID { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playGlobalSFX { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.Game.HarvestReadySignal harvestReadySignal { get; set; }

		[Inject]
		public global::Kampai.Game.AwardLairBonusDropsThenSetHarvestReadySignal awardDropsThenHarvestReadySignal { get; set; }

		public override void Execute()
		{
			int rushCost = MinionUtil.RushCost(minionID, playerService, timeEventService, definitionService);
			global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionID);
			global::Kampai.Game.Building byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.Building>(byInstanceId.BuildingID);
			int instanceId = 0;
			switch (byInstanceId.State)
			{
			default:
				return;
			case global::Kampai.Game.MinionState.Leisure:
				rushedBuildingType = BuildingType.BuildingTypeIdentifier.LEISURE;
				instanceId = byInstanceId.BuildingID;
				break;
			case global::Kampai.Game.MinionState.Tasking:
			{
				if (byInstanceId2 is global::Kampai.Game.VillainLairResourcePlot)
				{
					rushedBuildingType = BuildingType.BuildingTypeIdentifier.LAIR_RESOURCEPLOT;
					instanceId = byInstanceId2.ID;
					break;
				}
				global::Kampai.Game.ResourceBuilding resourceBuilding = byInstanceId2 as global::Kampai.Game.ResourceBuilding;
				if (resourceBuilding != null)
				{
					rushedBuildingType = BuildingType.BuildingTypeIdentifier.RESOURCE;
					instanceId = resourceBuilding.Definition.ItemId;
				}
				break;
			}
			}
			playerService.ProcessRush(rushCost, true, RushTransactionCallback, instanceId);
		}

		private void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionID);
				playGlobalSFX.Dispatch("Play_button_premium_01");
				if (rushedBuildingType == BuildingType.BuildingTypeIdentifier.LAIR_RESOURCEPLOT || rushedBuildingType == BuildingType.BuildingTypeIdentifier.LEISURE)
				{
					LavaOrLeisureRush(byInstanceId.BuildingID, rushedBuildingType);
				}
				else if (rushedBuildingType == BuildingType.BuildingTypeIdentifier.RESOURCE)
				{
					timeEventService.RushEvent(minionID);
					bool alreadyRushed = playerService.GetByInstanceId<global::Kampai.Game.TaskableBuilding>(byInstanceId.BuildingID) != null;
					byInstanceId.AlreadyRushed = alreadyRushed;
				}
			}
		}

		private void LavaOrLeisureRush(int ID, BuildingType.BuildingTypeIdentifier type)
		{
			setPremiumCurrencySignal.Dispatch();
			if (timeEventService.HasEventID(ID))
			{
				timeEventService.RushEvent(ID);
				return;
			}
			switch (type)
			{
			case BuildingType.BuildingTypeIdentifier.LAIR_RESOURCEPLOT:
				awardDropsThenHarvestReadySignal.Dispatch(ID);
				break;
			case BuildingType.BuildingTypeIdentifier.LEISURE:
				harvestReadySignal.Dispatch(ID);
				break;
			}
		}
	}
}
