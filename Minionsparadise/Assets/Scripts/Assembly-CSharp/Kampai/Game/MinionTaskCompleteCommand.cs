namespace Kampai.Game
{
	public class MinionTaskCompleteCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int minionId { get; set; }

		[Inject]
		public global::Kampai.Game.EjectMinionFromBuildingSignal ejectMinionFromBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionStateChange { get; set; }

		[Inject]
		public global::Kampai.Game.ToggleMinionRendererSignal toggleMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.HarvestReadySignal harvestReadySignal { get; set; }

		[Inject]
		public global::Kampai.Game.StopGagAnimationSignal stopGagAnimationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowHarvestReadySignal showHarvestReadySignal { get; set; }

		[Inject]
		public global::Kampai.Game.AddMinionToTikiBarSignal addMinionToTikiBarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionId);
			if (byInstanceId != null)
			{
				byInstanceId.TaskDuration = 0;
				int buildingID = byInstanceId.BuildingID;
				global::Kampai.Game.TaskableBuilding byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.TaskableBuilding>(buildingID);
				if (byInstanceId2 != null)
				{
					TaskCompleted(byInstanceId, byInstanceId2, buildingID);
				}
			}
		}

		private void TaskCompleted(global::Kampai.Game.Minion minion, global::Kampai.Game.TaskableBuilding building, int buildingId)
		{
			int utcTime = timeService.CurrentTime();
			global::Kampai.Game.ResourceBuilding resourceBuilding = building as global::Kampai.Game.ResourceBuilding;
			global::Kampai.Game.DebrisBuilding debrisBuilding = building as global::Kampai.Game.DebrisBuilding;
			if (resourceBuilding != null)
			{
				resourceBuilding.PrepareForHarvest(utcTime, minion.ID);
				CheckBonusItems(resourceBuilding, minion);
				minion.AlreadyRushed = false;
				ejectMinionFromBuildingSignal.Dispatch(building, minionId);
				minionStateChange.Dispatch(minionId, global::Kampai.Game.MinionState.Idle);
				toggleMinionSignal.Dispatch(minionId, true);
				stopGagAnimationSignal.Dispatch(buildingId);
				int prestigeId = minion.PrestigeId;
				if (prestigeId > 0)
				{
					global::Kampai.Game.TikiBarBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TikiBarBuilding>(313);
					global::Kampai.Game.Prestige byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.Prestige>(prestigeId);
					int minionSlotIndex = byInstanceId.GetMinionSlotIndex(byInstanceId2.Definition.ID);
					if (minionSlotIndex != -1 && minionSlotIndex < 3)
					{
						addMinionToTikiBarSignal.Dispatch(byInstanceId, minion, byInstanceId2, minionSlotIndex);
					}
				}
			}
			else
			{
				if (debrisBuilding != null)
				{
					ejectMinionFromBuildingSignal.Dispatch(building, minionId);
					minionStateChange.Dispatch(minionId, global::Kampai.Game.MinionState.Idle);
					timeEventService.RemoveEvent(buildingId);
					return;
				}
				building.AddToCompletedMinions(minionId, utcTime);
				showHarvestReadySignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(building.ID, minionId));
			}
			buildingChangeStateSignal.Dispatch(building.ID, global::Kampai.Game.BuildingState.Harvestable);
			harvestReadySignal.Dispatch(buildingId);
			timeEventService.RemoveEvent(buildingId);
		}

		public void CheckBonusItems(global::Kampai.Game.ResourceBuilding resourceBuilding, global::Kampai.Game.Minion minion)
		{
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.MYSTERY_BOXES_OPENED);
			if (quantity < 1)
			{
				return;
			}
			global::Kampai.Game.MinionBenefitLevelBandDefintion minionBenefitLevelBandDefintion = definitionService.Get<global::Kampai.Game.MinionBenefitLevelBandDefintion>(89898);
			if (quantity != 1 || resourceBuilding.Definition.ID == minionBenefitLevelBandDefintion.FirstBuildingId)
			{
				global::System.Collections.Generic.List<int> bonusMinionItems = resourceBuilding.BonusMinionItems;
				bool flag = false;
				global::Kampai.Game.MinionBenefitLevel minionBenefit = minionBenefitLevelBandDefintion.GetMinionBenefit(minion.Level);
				if (((quantity != 1) ? randomService.NextFloat() : 0f) < minionBenefit.doubleDropPercentage)
				{
					bonusMinionItems.Add(resourceBuilding.Definition.ItemId);
					flag = true;
				}
				if (((quantity != 1) ? randomService.NextFloat() : 0f) < minionBenefit.premiumDropPercentage)
				{
					bonusMinionItems.Add(1);
					flag = true;
				}
				if (((quantity != 1) ? randomService.NextFloat() : 0f) < minionBenefit.rareDropPercentage)
				{
					int iD = playerService.GetWeightedInstance(4000).NextPick(randomService).ID;
					bonusMinionItems.Add(iD);
					flag = true;
				}
				if (flag)
				{
					playerService.AlterQuantity(global::Kampai.Game.StaticItem.MYSTERY_BOXES_OPENED, 1);
				}
			}
		}
	}
}
