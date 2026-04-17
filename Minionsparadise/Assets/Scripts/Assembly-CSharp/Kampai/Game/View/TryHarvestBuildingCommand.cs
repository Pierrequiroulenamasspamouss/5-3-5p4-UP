namespace Kampai.Game.View
{
	public class TryHarvestBuildingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.View.BuildingObject buildingObj { get; set; }

		[Inject]
		public global::System.Action callback { get; set; }

		[Inject]
		public bool sentFromUI { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal spawnDooberSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingHarvestSignal buildingHarvestSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateResourceIconCountSignal updateResourceIconCountSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MIBBuildingResourceIconSelectedSignal mibBuildingResourceIconSelectedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenStorageBuildingSignal openStorageBuildingSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel lairModel { get; set; }

		public override void Execute()
		{
			if (playerService.HasStorageBuilding())
			{
				global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingObj.ID);
				if (!TryHarvestMIB(byInstanceId) && !TryHarvestTaskable(byInstanceId) && !TryHarvestVillainLairPortal(byInstanceId) && !TryHarvestVillainLairResource(byInstanceId))
				{
					TryHarvestCrafting(byInstanceId);
				}
			}
		}

		private bool TryHarvestMIB(global::Kampai.Game.Building building)
		{
			global::Kampai.Game.MIBBuilding mIBBuilding = building as global::Kampai.Game.MIBBuilding;
			if (mIBBuilding == null)
			{
				return false;
			}
			mibBuildingResourceIconSelectedSignal.Dispatch();
			return true;
		}

		private bool TryHarvestTaskable(global::Kampai.Game.Building building)
		{
			global::Kampai.Game.TaskableBuilding taskableBuilding = building as global::Kampai.Game.TaskableBuilding;
			if (taskableBuilding == null)
			{
				return false;
			}
			global::Kampai.Game.ResourceBuilding resourceBuilding = taskableBuilding as global::Kampai.Game.ResourceBuilding;
			if (resourceBuilding != null && resourceBuilding.BonusMinionItems.Count > 0 && !sentFromUI)
			{
				return RewardBonusItems(resourceBuilding);
			}
			if (taskableBuilding.GetNumCompleteMinions() > 0 || (resourceBuilding != null && resourceBuilding.AvailableHarvest > 0))
			{
				int iD = taskableBuilding.ID;
				global::UnityEngine.Vector3 type = new global::UnityEngine.Vector3(taskableBuilding.Location.x, 0f, taskableBuilding.Location.y);
				int transactionID = taskableBuilding.GetTransactionID(definitionService);
				if (playerService.FinishTransaction(transactionID, global::Kampai.Game.TransactionTarget.HARVEST, new global::Kampai.Game.TransactionArg(iD)))
				{
					int harvestItemDefinitionIdFromTransactionId = definitionService.GetHarvestItemDefinitionIdFromTransactionId(transactionID);
					spawnDooberSignal.Dispatch(type, global::Kampai.UI.View.DestinationType.STORAGE, harvestItemDefinitionIdFromTransactionId, true);
					buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerMediator>().LastHarvestedBuildingID = iD;
					buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerMediator>().HarvestTimer = 1f;
					if (resourceBuilding == null)
					{
						if (taskableBuilding.GetMinionsInBuilding() <= 0)
						{
							return false;
						}
						int minionByIndex = taskableBuilding.GetMinionByIndex(0);
						playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionByIndex).AlreadyRushed = false;
					}
					buildingHarvestSignal.Dispatch(iD);
					callback();
					int newHarvestAvailable = GetNewHarvestAvailable(resourceBuilding, taskableBuilding);
					updateResourceIconCountSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(iD, harvestItemDefinitionIdFromTransactionId), newHarvestAvailable);
					buildingObj.Bounce();
					global::Kampai.Game.IQuestService obj = questService;
					int item = harvestItemDefinitionIdFromTransactionId;
					obj.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Harvest, global::Kampai.Game.QuestTaskTransition.Complete, null, 0, item);
				}
				return true;
			}
			return false;
		}

		private void OpenStorage()
		{
			global::Kampai.Game.StorageBuilding firstInstanceByDefintion = playerService.GetFirstInstanceByDefintion<global::Kampai.Game.StorageBuilding, global::Kampai.Game.StorageBuildingDefinition>();
			openStorageBuildingSignal.Dispatch(firstInstanceByDefintion, true);
		}

		private bool TryHarvestVillainLairResource(global::Kampai.Game.Building building)
		{
			global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = building as global::Kampai.Game.VillainLairResourcePlot;
			if (villainLairResourcePlot == null)
			{
				return false;
			}
			if (villainLairResourcePlot.BonusMinionItems.Count > 0 && !sentFromUI)
			{
				return RewardBonusItems(villainLairResourcePlot);
			}
			if (villainLairResourcePlot.MinionIsTaskedToBuilding() || villainLairResourcePlot.UTCLastTaskingTimeStarted == 0)
			{
				return true;
			}
			int resourceItemID = villainLairResourcePlot.parentLair.Definition.ResourceItemID;
			global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(resourceItemID);
			int transactionId = ingredientsItemDefinition.TransactionId;
			if (playerService.FinishTransaction(transactionId, global::Kampai.Game.TransactionTarget.HARVEST, new global::Kampai.Game.TransactionArg(villainLairResourcePlot.ID)))
			{
				global::Kampai.Game.VillainLairEntranceBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(villainLairResourcePlot.parentLair.portalInstanceID);
				global::UnityEngine.Vector3 type = ((!lairModel.isPortalResourceModalOpen) ? new global::UnityEngine.Vector3(building.Location.x, 0f, building.Location.y) : new global::UnityEngine.Vector3(byInstanceId.Location.x, 0f, byInstanceId.Location.y));
				spawnDooberSignal.Dispatch(type, global::Kampai.UI.View.DestinationType.STORAGE, ingredientsItemDefinition.ID, true);
				SetPlotAsHarvested(villainLairResourcePlot, resourceItemID);
				int item = byInstanceId.GetNewHarvestAvailableForPortal(playerService).Item1;
				updateResourceIconCountSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(byInstanceId.ID, resourceItemID), item);
				buildingObj.Bounce();
				callback();
			}
			return true;
		}

		private void SetPlotAsHarvested(global::Kampai.Game.VillainLairResourcePlot plot, int itemID)
		{
			plot.UTCLastTaskingTimeStarted = 0;
			buildingChangeStateSignal.Dispatch(plot.ID, global::Kampai.Game.BuildingState.Idle);
			plot.harvestCount--;
			updateResourceIconCountSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(plot.ID, itemID), 0);
		}

		private bool TryHarvestVillainLairPortal(global::Kampai.Game.Building building)
		{
			global::Kampai.Game.VillainLairEntranceBuilding villainLairEntranceBuilding = building as global::Kampai.Game.VillainLairEntranceBuilding;
			if (villainLairEntranceBuilding == null)
			{
				return false;
			}
			if (villainLairEntranceBuilding.State == global::Kampai.Game.BuildingState.Inaccessible)
			{
				return true;
			}
			global::Kampai.Util.Tuple<int, int> newHarvestAvailableForPortal = villainLairEntranceBuilding.GetNewHarvestAvailableForPortal(playerService);
			int item = newHarvestAvailableForPortal.Item1;
			int item2 = newHarvestAvailableForPortal.Item2;
			if (item2 > 0 && !sentFromUI)
			{
				return RewardBonusItems(villainLairEntranceBuilding);
			}
			if (item < 1)
			{
				return true;
			}
			global::Kampai.Game.VillainLair byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLair>(villainLairEntranceBuilding.VillainLairInstanceID);
			int firstBuildingNumberOfHarvestableResourcePlot = byInstanceId.GetFirstBuildingNumberOfHarvestableResourcePlot(playerService, false);
			if (firstBuildingNumberOfHarvestableResourcePlot != 0)
			{
				global::Kampai.Game.VillainLairResourcePlot byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.VillainLairResourcePlot>(firstBuildingNumberOfHarvestableResourcePlot);
				if (byInstanceId2 != null)
				{
					int resourceItemID = byInstanceId2.parentLair.Definition.ResourceItemID;
					global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(resourceItemID);
					int transactionId = ingredientsItemDefinition.TransactionId;
					if (playerService.FinishTransaction(transactionId, global::Kampai.Game.TransactionTarget.HARVEST, new global::Kampai.Game.TransactionArg(byInstanceId2.ID)))
					{
						global::UnityEngine.Vector3 type = new global::UnityEngine.Vector3(villainLairEntranceBuilding.Location.x, 0f, villainLairEntranceBuilding.Location.y);
						spawnDooberSignal.Dispatch(type, global::Kampai.UI.View.DestinationType.STORAGE, ingredientsItemDefinition.ID, true);
						SetPlotAsHarvested(byInstanceId2, resourceItemID);
						int item3 = villainLairEntranceBuilding.GetNewHarvestAvailableForPortal(playerService).Item1;
						updateResourceIconCountSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(villainLairEntranceBuilding.ID, resourceItemID), item3);
						callback();
					}
				}
			}
			return true;
		}

		private bool RewardBonusItems(global::Kampai.Game.Building building)
		{
			global::UnityEngine.Vector3 buildingPosition = new global::UnityEngine.Vector3(building.Location.x, 0f, building.Location.y);
			int iD = building.ID;
			global::Kampai.Game.ResourceBuilding resourceBuilding = building as global::Kampai.Game.ResourceBuilding;
			global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = building as global::Kampai.Game.VillainLairResourcePlot;
			global::Kampai.Game.VillainLairEntranceBuilding villainLairEntranceBuilding = building as global::Kampai.Game.VillainLairEntranceBuilding;
			int num = 0;
			int num2 = 0;
			global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot2 = null;
			global::Kampai.Game.VillainLairEntranceBuilding villainLairEntranceBuilding2 = null;
			if (resourceBuilding != null)
			{
				SortBonusItemList(resourceBuilding.BonusMinionItems);
				num = resourceBuilding.BonusMinionItems.Count - 1;
				num2 = resourceBuilding.BonusMinionItems[num];
			}
			else if (villainLairResourcePlot != null)
			{
				SortBonusItemList(villainLairResourcePlot.BonusMinionItems);
				num = villainLairResourcePlot.BonusMinionItems.Count - 1;
				num2 = villainLairResourcePlot.BonusMinionItems[num];
				villainLairEntranceBuilding2 = playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(villainLairResourcePlot.parentLair.portalInstanceID);
				if (lairModel.isPortalResourceModalOpen)
				{
					buildingPosition = new global::UnityEngine.Vector3(villainLairEntranceBuilding2.Location.x, 0f, villainLairEntranceBuilding2.Location.y);
				}
			}
			else if (villainLairEntranceBuilding != null)
			{
				global::Kampai.Game.VillainLair byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLair>(villainLairEntranceBuilding.VillainLairInstanceID);
				global::System.Collections.Generic.List<int> allPlotBonusItems = byInstanceId.GetAllPlotBonusItems(playerService);
				SortBonusItemList(allPlotBonusItems);
				int index = allPlotBonusItems.Count - 1;
				int num3 = allPlotBonusItems[index];
				for (int i = 0; i < byInstanceId.resourcePlotInstanceIDs.Count; i++)
				{
					villainLairResourcePlot2 = playerService.GetByInstanceId<global::Kampai.Game.VillainLairResourcePlot>(byInstanceId.resourcePlotInstanceIDs[i]);
					if (villainLairResourcePlot2 != null && villainLairResourcePlot2.BonusMinionItems.Count > 0)
					{
						SortBonusItemList(villainLairResourcePlot2.BonusMinionItems);
						num = villainLairResourcePlot2.BonusMinionItems.Count - 1;
						num2 = villainLairResourcePlot2.BonusMinionItems[num];
						if (num2 == num3)
						{
							break;
						}
					}
				}
			}
			if (LimitStorage(num2))
			{
				return false;
			}
			RunBonusTransactionAndSpawnDoobers(num2, iD, buildingPosition);
			if (resourceBuilding != null)
			{
				buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerMediator>().LastHarvestedBuildingID = iD;
				buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerMediator>().HarvestTimer = 1f;
				resourceBuilding.BonusMinionItems.RemoveAt(num);
				updateResourceIconCountSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(iD, 196), resourceBuilding.BonusMinionItems.Count);
				buildingObj.Bounce();
				questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.MysteryBoxOnboarding, global::Kampai.Game.QuestTaskTransition.Complete, resourceBuilding, resourceBuilding.Definition.ID, num2);
				if (resourceBuilding.GetTotalHarvests() == 0)
				{
					buildingChangeStateSignal.Dispatch(resourceBuilding.ID, global::Kampai.Game.BuildingState.Idle);
				}
			}
			else if (villainLairResourcePlot != null)
			{
				if (villainLairEntranceBuilding2 != null)
				{
					RemoveAndUpdateBonusIconsFromPlotAndPortal(villainLairResourcePlot, num, villainLairEntranceBuilding2);
				}
				buildingObj.Bounce();
			}
			else if (villainLairEntranceBuilding != null && villainLairResourcePlot2 != null)
			{
				RemoveAndUpdateBonusIconsFromPlotAndPortal(villainLairResourcePlot2, num, villainLairEntranceBuilding);
			}
			return true;
		}

		private void SortBonusItemList(global::System.Collections.Generic.List<int> bonusItems)
		{
			bonusItems.Sort(CompareBonusIds);
		}

		private int CompareBonusIds(int item1, int item2)
		{
			int result = item2.CompareTo(item1);
			if (item1 != 1 && item2 != 1)
			{
				global::Kampai.Game.DropItemDefinition definition;
				if (definitionService.TryGet<global::Kampai.Game.DropItemDefinition>(item1, out definition))
				{
					return 1;
				}
				if (definitionService.TryGet<global::Kampai.Game.DropItemDefinition>(item2, out definition))
				{
					return -1;
				}
			}
			return result;
		}

		private bool LimitStorage(int itemID)
		{
			if (itemID == 1)
			{
				return false;
			}
			if (playerService.isStorageFull())
			{
				OpenStorage();
				return true;
			}
			return false;
		}

		private void RunBonusTransactionAndSpawnDoobers(int itemDefID, int buildingId, global::UnityEngine.Vector3 buildingPosition)
		{
			int quantity = 1;
			global::Kampai.UI.View.DestinationType type = global::Kampai.UI.View.DestinationType.MYSTERY_BOX;
			if (itemDefID == 1)
			{
				quantity = definitionService.Get<global::Kampai.Game.MinionBenefitLevelBandDefintion>(89898).BonusPremiumRewardValue;
			}
			playerService.CreateAndRunCustomTransaction(itemDefID, quantity, global::Kampai.Game.TransactionTarget.NO_VISUAL, new global::Kampai.Game.TransactionArg(buildingId));
			spawnDooberSignal.Dispatch(buildingPosition, type, itemDefID, true);
		}

		private void RemoveAndUpdateBonusIconsFromPlotAndPortal(global::Kampai.Game.VillainLairResourcePlot plot, int endIndex, global::Kampai.Game.VillainLairEntranceBuilding portal)
		{
			plot.BonusMinionItems.RemoveAt(endIndex);
			updateResourceIconCountSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(plot.ID, 196), plot.BonusMinionItems.Count);
			int item = portal.GetNewHarvestAvailableForPortal(playerService).Item2;
			updateResourceIconCountSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(plot.parentLair.portalInstanceID, 196), item);
		}

		private int GetNewHarvestAvailable(global::Kampai.Game.ResourceBuilding resourceBuilding, global::Kampai.Game.TaskableBuilding taskBuilding)
		{
			int num = 0;
			if (resourceBuilding != null)
			{
				num = resourceBuilding.AvailableHarvest;
				if (taskBuilding.GetMinionsInBuilding() == 0)
				{
					if (resourceBuilding.GetTotalHarvests() == 0)
					{
						buildingChangeStateSignal.Dispatch(taskBuilding.ID, global::Kampai.Game.BuildingState.Idle);
					}
				}
				else
				{
					buildingChangeStateSignal.Dispatch(taskBuilding.ID, global::Kampai.Game.BuildingState.Working);
				}
			}
			else
			{
				num = taskBuilding.GetNumCompleteMinions();
			}
			return num;
		}

		private void TryHarvestCrafting(global::Kampai.Game.Building building)
		{
			global::Kampai.Game.CraftingBuilding craftingBuilding = building as global::Kampai.Game.CraftingBuilding;
			if (craftingBuilding == null)
			{
				return;
			}
			int count = craftingBuilding.CompletedCrafts.Count;
			if (count <= 0)
			{
				return;
			}
			global::UnityEngine.Vector3 type = new global::UnityEngine.Vector3(craftingBuilding.Location.x, 0f, craftingBuilding.Location.y);
			int num = craftingBuilding.CompletedCrafts[0];
			global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(num);
			global::Kampai.Game.Transaction.TransactionDefinition transaction = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(ingredientsItemDefinition.TransactionId);
			int xPOutputForTransaction = global::Kampai.Game.Transaction.TransactionUtil.GetXPOutputForTransaction(transaction);
			global::Kampai.Game.TransactionArg transactionArg = new global::Kampai.Game.TransactionArg(craftingBuilding.ID);
			transactionArg.CraftableXPEarned = xPOutputForTransaction;
			if (!playerService.FinishTransaction(ingredientsItemDefinition.TransactionId, global::Kampai.Game.TransactionTarget.HARVEST, transactionArg))
			{
				return;
			}
			spawnDooberSignal.Dispatch(type, global::Kampai.UI.View.DestinationType.STORAGE, ingredientsItemDefinition.ID, true);
			global::Kampai.Game.IQuestService obj = questService;
			int item = num;
			obj.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Harvest, global::Kampai.Game.QuestTaskTransition.Complete, null, 0, item);
			int num2 = 0;
			for (int i = 1; i < count; i++)
			{
				if (craftingBuilding.CompletedCrafts[i] == num)
				{
					num2++;
				}
			}
			craftingBuilding.CompletedCrafts.RemoveAt(GetLastIndexOfItem(craftingBuilding));
			count = craftingBuilding.CompletedCrafts.Count;
			int count2 = craftingBuilding.RecipeInQueue.Count;
			global::Kampai.Game.BuildingState param = global::Kampai.Game.BuildingState.Idle;
			if (count > 0 && count2 > 0)
			{
				param = global::Kampai.Game.BuildingState.HarvestableAndWorking;
			}
			else if (count > 0)
			{
				param = global::Kampai.Game.BuildingState.Harvestable;
			}
			else if (count2 > 0)
			{
				param = global::Kampai.Game.BuildingState.Working;
			}
			buildingChangeStateSignal.Dispatch(craftingBuilding.ID, param);
			TryHarvestCraftingDone(craftingBuilding.ID, num2, num);
		}

		private int GetLastIndexOfItem(global::Kampai.Game.CraftingBuilding craftBuilding)
		{
			for (int num = craftBuilding.CompletedCrafts.Count - 1; num >= 0; num--)
			{
				if (craftBuilding.CompletedCrafts[num] == craftBuilding.CompletedCrafts[0])
				{
					return num;
				}
			}
			return 0;
		}

		private void TryHarvestCraftingDone(int buildingId, int newCount, int itemDefId)
		{
			buildingObj.Bounce();
			buildingHarvestSignal.Dispatch(buildingId);
			callback();
			updateResourceIconCountSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(buildingId, itemDefId), newCount);
		}
	}
}
