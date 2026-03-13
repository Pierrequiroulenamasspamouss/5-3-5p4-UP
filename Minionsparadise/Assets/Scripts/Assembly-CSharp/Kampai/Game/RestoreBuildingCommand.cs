namespace Kampai.Game
{
	public class RestoreBuildingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Building building { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreScaffoldingViewSignal restoreScaffoldingViewSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreRibbonViewSignal restoreRibbonViewSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestorePlatformViewSignal restorePlatformViewSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreBuildingViewSignal restoreBuildingViewSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardRefillTicketSignal refillTicketSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardSetNewTicketSignal setNewTicketSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreTaskableBuildingSignal restoreTaskingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreCraftingBuildingsSignal craftingRestoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreLeisureBuildingSignal restoreLeisureBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreResourcePlotBuildingSignal restoreResourcePlotBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CleanupDebrisSignal cleanupDebris { get; set; }

		[Inject]
		public global::Kampai.Common.ScheduleCooldownSignal scheduleCooldownSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.BuildingState state = building.State;
			int num = timeService.CurrentTime() - building.StateStartTime;
			global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = building as global::Kampai.Game.VillainLairResourcePlot;
			if (villainLairResourcePlot != null)
			{
				restoreBuildingViewSignal.Dispatch(building);
				restoreResourcePlotBuildingSignal.Dispatch(villainLairResourcePlot);
			}
			else
			{
				switch (state)
				{
				case global::Kampai.Game.BuildingState.Inactive:
				case global::Kampai.Game.BuildingState.Construction:
					HandleInConstruction(num);
					break;
				case global::Kampai.Game.BuildingState.Complete:
					HandleCompletedConstruction();
					break;
				case global::Kampai.Game.BuildingState.Working:
				case global::Kampai.Game.BuildingState.Harvestable:
				case global::Kampai.Game.BuildingState.HarvestableAndWorking:
				{
					restoreBuildingViewSignal.Dispatch(building);
					global::Kampai.Game.TaskableBuilding taskableBuilding = building as global::Kampai.Game.TaskableBuilding;
					if (taskableBuilding != null)
					{
						restoreTaskingSignal.Dispatch(taskableBuilding);
					}
					global::Kampai.Game.CraftingBuilding craftingBuilding = building as global::Kampai.Game.CraftingBuilding;
					if (craftingBuilding != null)
					{
						craftingRestoreSignal.Dispatch(craftingBuilding);
					}
					global::Kampai.Game.LeisureBuilding leisureBuilding = building as global::Kampai.Game.LeisureBuilding;
					if (leisureBuilding != null)
					{
						restoreLeisureBuildingSignal.Dispatch(leisureBuilding);
					}
					break;
				}
				default:
					restoreBuildingViewSignal.Dispatch(building);
					break;
				case global::Kampai.Game.BuildingState.Inventory:
					break;
				}
			}
			if (building.GetType() == typeof(global::Kampai.Game.OrderBoard) && state != global::Kampai.Game.BuildingState.Broken && state != global::Kampai.Game.BuildingState.Complete && state != global::Kampai.Game.BuildingState.Construction && state != global::Kampai.Game.BuildingState.Inactive)
			{
				global::Kampai.Game.OrderBoard orderBoard = building as global::Kampai.Game.OrderBoard;
				routineRunner.StartCoroutine(WaitTwoFrames(delegate
				{
					RestoreBlackMarket(orderBoard);
				}));
			}
			CheckCooldownState(state, num);
			CleanupDebris();
		}

		private void HandleInConstruction(float passedTime)
		{
			global::Kampai.Game.BuildingDefinition definition = building.Definition;
			int num = definition.ConstructionTime;
			if (definition.IncrementalConstructionTime > 0)
			{
				num += (building.BuildingNumber - 1) * definition.IncrementalConstructionTime;
			}
			restoreBuildingViewSignal.Dispatch(building);
			restorePlatformViewSignal.Dispatch(building);
			if (passedTime > (float)num)
			{
				restoreScaffoldingViewSignal.Dispatch(building, false);
				restoreRibbonViewSignal.Dispatch(building);
				buildingChangeStateSignal.Dispatch(building.ID, global::Kampai.Game.BuildingState.Complete);
			}
			else
			{
				restoreScaffoldingViewSignal.Dispatch(building, true);
			}
		}

		private void HandleCompletedConstruction()
		{
			restoreBuildingViewSignal.Dispatch(building);
			restorePlatformViewSignal.Dispatch(building);
			restoreScaffoldingViewSignal.Dispatch(building, false);
			restoreRibbonViewSignal.Dispatch(building);
		}

		private void CleanupDebris()
		{
			global::Kampai.Game.DebrisBuilding debrisBuilding = building as global::Kampai.Game.DebrisBuilding;
			if (debrisBuilding != null && debrisBuilding.PaidInputCostToClear)
			{
				cleanupDebris.Dispatch(debrisBuilding.ID, false);
			}
		}

		private void CheckCooldownState(global::Kampai.Game.BuildingState buildingState, int passedTime)
		{
			if (buildingState != global::Kampai.Game.BuildingState.Cooldown || !(building is global::Kampai.Game.IBuildingWithCooldown))
			{
				return;
			}
			int cooldown = ((global::Kampai.Game.IBuildingWithCooldown)building).GetCooldown();
			if (passedTime >= cooldown)
			{
				buildingChangeStateSignal.Dispatch(building.ID, global::Kampai.Game.BuildingState.Idle);
				return;
			}
			bool second = false;
			global::Kampai.Game.MignetteBuilding mignetteBuilding = building as global::Kampai.Game.MignetteBuilding;
			if (mignetteBuilding != null)
			{
				second = true;
			}
			scheduleCooldownSignal.Dispatch(new global::Kampai.Util.Tuple<int, bool>(building.ID, second), false);
		}

		private void RestoreBlackMarket(global::Kampai.Game.OrderBoard blackMarketBuilding)
		{
			int num = 0;
			num = blackMarketBuilding.Definition.RefillTime;
			foreach (global::Kampai.Game.OrderBoardTicket ticket in blackMarketBuilding.tickets)
			{
				if (ticket.StartTime > 0)
				{
					int num2 = -ticket.BoardIndex;
					if (timeService.CurrentTime() >= ticket.StartTime + num)
					{
						setNewTicketSignal.Dispatch(num2, false);
					}
					else if (timeService.CurrentTime() < ticket.StartTime + num)
					{
						timeEventService.AddEvent(num2, ticket.StartTime, num, refillTicketSignal);
					}
				}
			}
		}

		private global::System.Collections.IEnumerator WaitTwoFrames(global::System.Action a)
		{
			yield return null;
			yield return null;
			a();
		}
	}
}
