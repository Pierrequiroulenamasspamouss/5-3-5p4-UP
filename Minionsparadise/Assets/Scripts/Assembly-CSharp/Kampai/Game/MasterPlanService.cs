namespace Kampai.Game
{
    public class MasterPlanService : global::Kampai.Game.IMasterPlanService
    {
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MasterPlanService") as global::Kampai.Util.IKampaiLogger;

		private int _forceDefinitionID;

		private global::Kampai.Game.MasterPlan _currentMasterPlan;

		private global::Kampai.Game.MasterPlanComponent activeComponent;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanTaskCompleteSignal taskCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanComponentTaskUpdatedSignal taskUpdatedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ResetLairWayfinderIconSignal resetIconSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetMasterPlanWayfinderIconToCompleteSignal setCompleteIconSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateAllVillainLairCollidersSignal updateLairCollidersSignal { get; set; }

		public global::Kampai.Game.MasterPlan CurrentMasterPlan
		{
			get
			{
				if (_currentMasterPlan == null)
				{
					_currentMasterPlan = playerService.GetFirstInstanceByDefintion<global::Kampai.Game.MasterPlan, global::Kampai.Game.MasterPlanDefinition>();
				}
				return _currentMasterPlan;
			}
			private set
			{
				_currentMasterPlan = value;
			}
		}

		public void AddMasterPlanObject(global::Kampai.Game.View.MasterPlanObject obj)
		{
			if (!global::Kampai.Game.View.ActionableObjectManagerView.allObjects.ContainsKey(obj.ID))
			{
				global::Kampai.Game.View.ActionableObjectManagerView.allObjects.Add(obj.ID, obj);
			}
		}

		public void Initialize()
		{
			activeComponent = GetActiveComponent();
		}

		private global::Kampai.Game.MasterPlan CreateMasterPlan(global::Kampai.Game.MasterPlanDefinition masterPlanDefinition)
		{
			global::Kampai.Game.MasterPlan masterPlan = masterPlanDefinition.Build() as global::Kampai.Game.MasterPlan;
			masterPlan.StartGameTime = playerDurationService.TotalGamePlaySeconds;
			playerService.Add(masterPlan);
			CurrentMasterPlan = masterPlan;
			return masterPlan;
		}

		public void CreateMasterPlanComponents(global::Kampai.Game.MasterPlan masterPlan)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponentDefinition> componentDefinitions = GetComponentDefinitions(masterPlan.Definition);
			global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(componentDefinitions[0].ID);
			if (firstInstanceByDefinitionId != null)
			{
				logger.Warning("Trying to create components that already exist");
				return;
			}
			global::System.Collections.Generic.IList<int> compBuildingDefinitionIDs = masterPlan.Definition.CompBuildingDefinitionIDs;
			if (componentDefinitions.Count != compBuildingDefinitionIDs.Count)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DS_COMPONENT_COUNT_MISMATCH, masterPlan.Definition.ID);
				return;
			}
			global::Kampai.Game.VillainLairDefinition villainLairDefinition = definitionService.Get<global::Kampai.Game.VillainLairDefinition>(global::Kampai.Game.StaticItem.VILLAIN_LAIR_DEFINITION_ID);
			global::System.Collections.Generic.List<global::Kampai.Game.PlatformDefinition> platforms = villainLairDefinition.Platforms;
			for (int i = 0; i < componentDefinitions.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponentDefinition masterPlanComponentDefinition = componentDefinitions[i];
				global::Kampai.Game.MasterPlanComponent masterPlanComponent = masterPlanComponentDefinition.Build() as global::Kampai.Game.MasterPlanComponent;
				masterPlanComponent.planTrackingInstance = masterPlan.ID;
				masterPlanComponent.reward = masterPlanComponentDefinition.Reward.Build();
				masterPlanComponent.buildingDefID = compBuildingDefinitionIDs[i];
				masterPlanComponent.buildingLocation = platforms[i].placementLocation;
				for (int j = 0; j < masterPlanComponentDefinition.Tasks.Count; j++)
				{
					masterPlanComponent.tasks.Add(masterPlanComponentDefinition.Tasks[j].Build());
				}
				playerService.Add(masterPlanComponent);
			}
			updateLairCollidersSignal.Dispatch();
		}

		public global::Kampai.Game.MasterPlan CreateNewMasterPlan()
		{
			if (_forceDefinitionID != 0)
			{
				return ForceCreateMasterPlan();
			}
			global::Kampai.Game.MasterPlanDefinition masterPlanDefinition = null;
			global::Kampai.Game.Transaction.WeightedInstance weightedInstance = playerService.GetWeightedInstance(4037);
			global::System.Collections.Generic.IList<global::Kampai.Game.Transaction.WeightedQuantityItem> entities = weightedInstance.Definition.Entities;
			if (CurrentMasterPlan == null)
			{
				masterPlanDefinition = definitionService.Get<global::Kampai.Game.MasterPlanDefinition>(entities[0].ID);
				global::Kampai.Game.MasterPlan masterPlan = CreateMasterPlan(masterPlanDefinition);
				CreateMasterPlanComponents(masterPlan);
				return masterPlan;
			}
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.MASTER_PLAN_PLAYLIST_INDEX);
			if (quantity < entities.Count - 1)
			{
				playerService.AlterQuantity(global::Kampai.Game.StaticItem.MASTER_PLAN_PLAYLIST_INDEX, 1);
				quantity++;
				masterPlanDefinition = definitionService.Get<global::Kampai.Game.MasterPlanDefinition>(entities[quantity].ID);
			}
			else
			{
				masterPlanDefinition = GenerateNextRandomPlanDefinition(CurrentMasterPlan.Definition, weightedInstance);
			}
			playerService.Remove(CurrentMasterPlan);
			global::Kampai.Game.MasterPlan result = CreateMasterPlan(masterPlanDefinition);
			GenerateDynamicMasterPlan(masterPlanDefinition);
			return result;
		}

		public bool ForceNextMPDefinition(int defID)
		{
			_forceDefinitionID = defID;
			global::Kampai.Game.MasterPlanDefinition definition = null;
			if (definitionService.TryGet<global::Kampai.Game.MasterPlanDefinition>(defID, out definition))
			{
				return true;
			}
			_forceDefinitionID = 0;
			return false;
		}

		private global::Kampai.Game.MasterPlan ForceCreateMasterPlan()
		{
			if (CurrentMasterPlan != null)
			{
				playerService.Remove(CurrentMasterPlan);
			}
			global::Kampai.Game.MasterPlanDefinition masterPlanDefinition = definitionService.Get<global::Kampai.Game.MasterPlanDefinition>(_forceDefinitionID);
			_forceDefinitionID = 0;
			global::Kampai.Game.MasterPlan result = CreateMasterPlan(masterPlanDefinition);
			GenerateDynamicMasterPlan(masterPlanDefinition);
			return result;
		}

		private global::Kampai.Game.MasterPlanDefinition GenerateNextRandomPlanDefinition(global::Kampai.Game.MasterPlanDefinition currentPlanDef, global::Kampai.Game.Transaction.WeightedInstance wi)
		{
			if (wi != null && wi.Definition.Entities.Count > 1)
			{
				global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
				for (int i = 0; i < wi.Definition.Entities.Count; i++)
				{
					int iD = wi.Definition.Entities[i].ID;
					if (iD != currentPlanDef.ID)
					{
						list.AddRange(global::System.Linq.Enumerable.Repeat(iD, (int)wi.Definition.Entities[i].Weight));
					}
				}
				if (list.Count > 0)
				{
					for (int j = 0; j < list.Count; j++)
					{
						int index = randomService.NextInt(j, list.Count);
						int value = list[j];
						list[j] = list[index];
						list[index] = value;
					}
					global::Kampai.Game.MasterPlanDefinition definition;
					if (definitionService.TryGet<global::Kampai.Game.MasterPlanDefinition>(list[list.Count - 1], out definition))
					{
						return definition;
					}
				}
			}
			logger.Error("Was unable to randomly create a new master plan.  Returning the old random plan: {0}", currentPlanDef.ID);
			return currentPlanDef;
		}

		private void GenerateDynamicMasterPlan(global::Kampai.Game.MasterPlanDefinition masterPlanDefinition)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponentDefinition> componentDefinitions = GetComponentDefinitions(masterPlanDefinition);
			global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(componentDefinitions[0].ID);
			if (firstInstanceByDefinitionId != null)
			{
				logger.Warning("Trying to create components that already exist");
				return;
			}
			global::System.Collections.Generic.IList<int> compBuildingDefinitionIDs = masterPlanDefinition.CompBuildingDefinitionIDs;
			if (componentDefinitions.Count != compBuildingDefinitionIDs.Count)
			{
				logger.Error("The number of components and buildings do not match for plan {0}", masterPlanDefinition.ID);
				logger.Fatal(global::Kampai.Util.FatalCode.DS_COMPONENT_COUNT_MISMATCH, masterPlanDefinition.ID);
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.PlatformDefinition> platforms = definitionService.Get<global::Kampai.Game.VillainLairDefinition>(global::Kampai.Game.StaticItem.VILLAIN_LAIR_DEFINITION_ID).Platforms;
			global::Kampai.Game.DynamicMasterPlanDefinition dynamicMasterPlanDefinition = definitionService.Get<global::Kampai.Game.DynamicMasterPlanDefinition>();
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list = new global::System.Collections.Generic.List<global::Kampai.Game.IngredientsItemDefinition>();
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list2 = new global::System.Collections.Generic.List<global::Kampai.Game.IngredientsItemDefinition>();
			GenerateItems(dynamicMasterPlanDefinition.ItemCategoryCount, list, list2);
			for (int i = 0; i < dynamicMasterPlanDefinition.DynamicComponents.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent masterPlanComponent = componentDefinitions[i].Build() as global::Kampai.Game.MasterPlanComponent;
				masterPlanComponent.planTrackingInstance = CurrentMasterPlan.ID;
				masterPlanComponent.buildingDefID = compBuildingDefinitionIDs[i];
				masterPlanComponent.buildingLocation = platforms[i].placementLocation;
				playerService.Add(masterPlanComponent);
				global::Kampai.Game.MasterPlanComponentDefinition masterPlanComponentDefinition = dynamicMasterPlanDefinition.DynamicComponents[i];
				for (int j = 0; j < masterPlanComponentDefinition.Tasks.Count; j++)
				{
					int requiredItemId = masterPlanComponentDefinition.Tasks[j].requiredItemId;
					global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list4;
					if (requiredItemId % 2 == 0)
					{
						global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list3 = list2;
						list4 = list3;
					}
					else
					{
						list4 = list;
					}
					global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list5 = list4;
					global::Kampai.Game.IngredientsItemDefinition item = list5[requiredItemId / 2];
					global::Kampai.Game.MasterPlanComponentTask item2 = GenerateDynamicTask(masterPlanComponentDefinition.Tasks[j], item);
					masterPlanComponent.tasks.Add(item2);
				}
				int rewardItemId = masterPlanComponentDefinition.Reward.rewardItemId;
				global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list6;
				if (rewardItemId % 2 == 0)
				{
					global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list3 = list2;
					list6 = list3;
				}
				else
				{
					list6 = list;
				}
				global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list7 = list6;
				global::Kampai.Game.IngredientsItemDefinition item3 = list7[rewardItemId / 2];
				masterPlanComponent.reward = GenerateDynamicReward(masterPlanComponent, item3);
			}
			updateLairCollidersSignal.Dispatch();
		}

		public bool HasReceivedInitialRewardFromCurrentPlan()
		{
			if (CurrentMasterPlan == null)
			{
				return false;
			}
			return HasReceivedInitialRewardFromPlanDefinition(CurrentMasterPlan.Definition);
		}

		public bool HasReceivedInitialRewardFromPlanDefinition(global::Kampai.Game.MasterPlanDefinition planDefinition)
		{
			if (planDefinition == null)
			{
				return false;
			}
			return playerService.GetInstancesByDefinitionID(planDefinition.LeavebehindBuildingDefID).Count > 0;
		}

		public void SelectMasterPlanComponent(global::Kampai.Game.MasterPlanComponent component)
		{
			activeComponent = component;
			if (activeComponent != null)
			{
				if (activeComponent.State == global::Kampai.Game.MasterPlanComponentState.NotStarted)
				{
					activeComponent.State = global::Kampai.Game.MasterPlanComponentState.InProgress;
				}
				ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.Deliver, 0u);
				ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.Collect, 0u);
			}
		}

		public void ProcessTransactionData(global::Kampai.Game.Transaction.TransactionUpdateData data)
		{
			if (activeComponent == null)
			{
				return;
			}
			ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.Deliver, 0u);
			if (data.Target == global::Kampai.Game.TransactionTarget.BLACKMARKETBOARD && data.Type == global::Kampai.Game.Transaction.UpdateType.TRANSACTION_FINISH)
			{
				ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.CompleteOrders, 1u);
			}
			if (data.Outputs == null)
			{
				return;
			}
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> outputs = data.Outputs;
			foreach (global::Kampai.Util.QuantityItem item in outputs)
			{
				if (item.Quantity != 0)
				{
					if (item.ID == 2)
					{
						ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints, item.Quantity, item.ID);
					}
					if (data.Target == global::Kampai.Game.TransactionTarget.HARVEST || data.Target == global::Kampai.Game.TransactionTarget.MARKETPLACE)
					{
						ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.Collect, item.Quantity, item.ID);
					}
					if (data.Target == global::Kampai.Game.TransactionTarget.BLACKMARKETBOARD && item.ID == 0)
					{
						ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars, item.Quantity, 3022);
					}
					else if (data.Target == global::Kampai.Game.TransactionTarget.MARKETPLACE && item.ID == 0)
					{
						ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars, item.Quantity, 3117);
					}
					else if (item.ID == 0)
					{
						ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars, item.Quantity);
					}
				}
			}
		}

		public void ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType type, uint progress, int source = 0)
		{
			if (activeComponent == null)
			{
				return;
			}
			for (int i = 0; i < activeComponent.tasks.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponentTask masterPlanComponentTask = activeComponent.tasks[i];
				if (activeComponent.tasks[i].Definition.Type == type)
				{
					ProcessTask(masterPlanComponentTask, progress, source);
				}
				if (masterPlanComponentTask.isHarvestable && masterPlanComponentTask.Definition.Type != global::Kampai.Game.MasterPlanComponentTaskType.Deliver)
				{
					taskCompleteSignal.Dispatch(activeComponent, i);
				}
			}
			taskUpdatedSignal.Dispatch(activeComponent);
		}

		private global::Kampai.Game.MasterPlanComponentTask GenerateDynamicTask(global::Kampai.Game.MasterPlanComponentTaskDefinition taskDefinition, global::Kampai.Game.IngredientsItemDefinition item)
		{
			global::Kampai.Game.MasterPlanComponentTask masterPlanComponentTask = new global::Kampai.Game.MasterPlanComponentTask();
			masterPlanComponentTask.Definition = new global::Kampai.Game.MasterPlanComponentTaskDefinition();
			masterPlanComponentTask.Definition.Type = taskDefinition.Type;
			global::Kampai.Game.DynamicMasterPlanDefinition dynamicMasterPlanDefinition = definitionService.Get<global::Kampai.Game.DynamicMasterPlanDefinition>();
			switch (taskDefinition.Type)
			{
			case global::Kampai.Game.MasterPlanComponentTaskType.Deliver:
			case global::Kampai.Game.MasterPlanComponentTaskType.Collect:
				masterPlanComponentTask.Definition.requiredItemId = item.ID;
				masterPlanComponentTask.Definition.requiredQuantity = (uint)GenerateItemQuantity(item);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.CompleteOrders:
				masterPlanComponentTask.Definition.requiredQuantity = (uint)randomService.NextInt((int)dynamicMasterPlanDefinition.FillOrderRangeMin, (int)dynamicMasterPlanDefinition.FillOrderRangeMax);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.PlayMiniGame:
				if (randomService.NextBoolean())
				{
					global::System.Collections.Generic.IList<global::Kampai.Game.MignetteBuilding> instancesByType2 = playerService.GetInstancesByType<global::Kampai.Game.MignetteBuilding>();
					int index2 = randomService.NextInt(instancesByType2.Count);
					masterPlanComponentTask.Definition.requiredItemId = instancesByType2[index2].Definition.ID;
				}
				masterPlanComponentTask.Definition.ShowWayfinder = taskDefinition.ShowWayfinder;
				masterPlanComponentTask.Definition.requiredQuantity = (uint)randomService.NextInt((int)dynamicMasterPlanDefinition.PlayMiniGameRangeMin, (int)dynamicMasterPlanDefinition.PlayMiniGameRangeMax);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.MiniGameScore:
			{
				global::System.Collections.Generic.IList<global::Kampai.Game.MignetteBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MignetteBuilding>();
				int index = randomService.NextInt(instancesByType.Count);
				global::Kampai.Game.MignetteBuildingDefinition definition = instancesByType[index].Definition;
				masterPlanComponentTask.Definition.requiredItemId = definition.ID;
				global::Kampai.Game.MiniGameScoreRange miniGameScoreRange = GetMiniGameScoreRange(definition.ID);
				masterPlanComponentTask.Definition.requiredQuantity = GetRoundedQuantityWithinRange(miniGameScoreRange.ScoreRangeMin, miniGameScoreRange.ScoreRangeMax);
				masterPlanComponentTask.Definition.ShowWayfinder = taskDefinition.ShowWayfinder;
				break;
			}
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnLeisurePartyPoints:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnMignettePartyPoints:
				masterPlanComponentTask.Definition.requiredQuantity = GetRoundedQuantityWithinRange((int)dynamicMasterPlanDefinition.PartyPointsRangeMin, (int)dynamicMasterPlanDefinition.PartyPointsRangeMax);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars:
				masterPlanComponentTask.Definition.requiredQuantity = GetRoundedQuantityWithinRange((int)dynamicMasterPlanDefinition.EarnSandDollarMin, (int)dynamicMasterPlanDefinition.EarnSandDollarMax);
				break;
			default:
				logger.Error("Undefined component task type: {0}", taskDefinition.Type);
				break;
			}
			return masterPlanComponentTask;
		}

		private global::Kampai.Game.MasterPlanComponentReward GenerateDynamicReward(global::Kampai.Game.MasterPlanComponent component, global::Kampai.Game.IngredientsItemDefinition item)
		{
			global::Kampai.Game.MasterPlanComponentReward masterPlanComponentReward = new global::Kampai.Game.MasterPlanComponentReward();
			masterPlanComponentReward.Definition = new global::Kampai.Game.MasterPlanComponentRewardDefinition();
			masterPlanComponentReward.Definition.rewardItemId = item.ID;
			masterPlanComponentReward.Definition.rewardQuantity = (uint)global::UnityEngine.Mathf.CeilToInt((float)GenerateItemQuantity(item) / 2f);
			uint num = 0u;
			uint num2 = 0u;
			global::Kampai.Game.DynamicMasterPlanDefinition dynamicMasterPlanDefinition = definitionService.Get<global::Kampai.Game.DynamicMasterPlanDefinition>();
			for (int i = 0; i < component.tasks.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponentTaskDefinition definition = component.tasks[i].Definition;
				switch (definition.Type)
				{
				case global::Kampai.Game.MasterPlanComponentTaskType.Deliver:
				case global::Kampai.Game.MasterPlanComponentTaskType.Collect:
				{
					global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(definition.requiredItemId);
					num += (uint)(itemDefinition.BaseGrindCost * (int)definition.requiredQuantity);
					break;
				}
				case global::Kampai.Game.MasterPlanComponentTaskType.CompleteOrders:
					num2 += GetPremiumReward(dynamicMasterPlanDefinition.RewardTableCompleteOrders, definition.requiredQuantity);
					break;
				case global::Kampai.Game.MasterPlanComponentTaskType.PlayMiniGame:
					num2 += GetPremiumReward(dynamicMasterPlanDefinition.RewardTablePlayMiniGame, definition.requiredQuantity);
					break;
				case global::Kampai.Game.MasterPlanComponentTaskType.MiniGameScore:
				{
					global::Kampai.Game.MiniGameScoreReward miniGameReward = GetMiniGameReward(definition.requiredItemId);
					num2 += GetPremiumReward(miniGameReward.rewardTable, definition.requiredQuantity);
					break;
				}
				case global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints:
				case global::Kampai.Game.MasterPlanComponentTaskType.EarnLeisurePartyPoints:
				case global::Kampai.Game.MasterPlanComponentTaskType.EarnMignettePartyPoints:
					num2 += GetPremiumReward(dynamicMasterPlanDefinition.RewardTableEarnPartyPoints, definition.requiredQuantity);
					break;
				case global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars:
					num2 += GetPremiumReward(dynamicMasterPlanDefinition.RewardTableEarnSandDollars, definition.requiredQuantity);
					break;
				}
			}
			masterPlanComponentReward.Definition.grindReward = global::System.Math.Max(dynamicMasterPlanDefinition.MinGrindReward, num);
			masterPlanComponentReward.Definition.premiumReward = global::System.Math.Max(dynamicMasterPlanDefinition.MinPremiumReward, num2);
			return masterPlanComponentReward;
		}

		private uint GetPremiumReward(global::System.Collections.Generic.IList<global::Kampai.Game.Reward> rewardTable, uint requiredQuantity)
		{
			for (int num = rewardTable.Count - 1; num >= 0; num--)
			{
				if (rewardTable[num].requiredQuantity <= requiredQuantity)
				{
					return rewardTable[num].premiumReward;
				}
			}
			return 0u;
		}

		private uint GetRoundedQuantityWithinRange(int rangeMin, int rangeMax)
		{
			int num = randomService.NextInt(rangeMin, rangeMax);
			int b = ((num != 0) ? ((int)global::System.Math.Round((double)num / 100.0, 0) * 100) : 0);
			return (uint)global::UnityEngine.Mathf.Max(rangeMin, b);
		}

		private global::Kampai.Game.MiniGameScoreRange GetMiniGameScoreRange(int miniGameDefinitionId)
		{
			global::Kampai.Game.DynamicMasterPlanDefinition dynamicMasterPlanDefinition = definitionService.Get<global::Kampai.Game.DynamicMasterPlanDefinition>();
			foreach (global::Kampai.Game.MiniGameScoreRange item in dynamicMasterPlanDefinition.RequirementTableMiniGameScore)
			{
				if (item.MiniGameId == miniGameDefinitionId)
				{
					return item;
				}
			}
			return null;
		}

		private global::Kampai.Game.MiniGameScoreReward GetMiniGameReward(int miniGameDefinitionId)
		{
			global::Kampai.Game.DynamicMasterPlanDefinition dynamicMasterPlanDefinition = definitionService.Get<global::Kampai.Game.DynamicMasterPlanDefinition>();
			foreach (global::Kampai.Game.MiniGameScoreReward item in dynamicMasterPlanDefinition.RewardTableMiniGameScore)
			{
				if (item.MiniGameId == miniGameDefinitionId)
				{
					return item;
				}
			}
			return null;
		}

		private void GenerateItems(int itemCount, global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> craftItems, global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> resourceItems)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list = GenerateCategory("Craftable");
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list2 = GenerateCategory("Base Resource");
			while (craftItems.Count < itemCount)
			{
				global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = list[randomService.NextInt(list.Count)];
				if (craftItems.Contains(ingredientsItemDefinition))
				{
					continue;
				}
				global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list3 = new global::System.Collections.Generic.List<global::Kampai.Game.IngredientsItemDefinition>();
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(ingredientsItemDefinition.TransactionId);
				for (int i = 0; i < transactionDefinition.Inputs.Count; i++)
				{
					global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition2 = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(transactionDefinition.Inputs[i].ID);
					if (ingredientsItemDefinition2 != null && !(ingredientsItemDefinition2 is global::Kampai.Game.DynamicIngredientsDefinition) && list2.Contains(ingredientsItemDefinition2) && !resourceItems.Contains(ingredientsItemDefinition2))
					{
						list3.Add(ingredientsItemDefinition2);
					}
				}
				if (list3.Count != 0)
				{
					craftItems.Add(ingredientsItemDefinition);
					int index = randomService.NextInt(list3.Count);
					resourceItems.Add(list3[index]);
				}
			}
		}

		private int GenerateItemQuantity(global::Kampai.Game.IngredientsItemDefinition item)
		{
			global::Kampai.Game.DynamicMasterPlanDefinition dynamicMasterPlanDefinition = definitionService.Get<global::Kampai.Game.DynamicMasterPlanDefinition>();
			int a = (int)(dynamicMasterPlanDefinition.MaxProductionTime / IngredientsItemUtil.GetHarvestTimeFromIngredientDefinition(item, definitionService));
			int b = (int)(dynamicMasterPlanDefinition.MaxStorageCapactiy * (float)playerService.GetCurrentStorageCapacity());
			int a2 = global::UnityEngine.Mathf.Min(a, b);
			int num = global::UnityEngine.Mathf.Min(a2, (int)dynamicMasterPlanDefinition.MaxProductionCount);
			return (num != 0) ? num : ((int)dynamicMasterPlanDefinition.MinProductionCount);
		}

		private global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> GenerateCategory(string category)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> list = new global::System.Collections.Generic.List<global::Kampai.Game.IngredientsItemDefinition>();
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> unlockedDefsByType = playerService.GetUnlockedDefsByType<global::Kampai.Game.IngredientsItemDefinition>();
			for (int i = 0; i < unlockedDefsByType.Count; i++)
			{
				if (category.Equals(unlockedDefsByType[i].TaxonomySpecific))
				{
					list.Add(unlockedDefsByType[i]);
				}
			}
			return list;
		}

		private void ProcessTask(global::Kampai.Game.MasterPlanComponentTask task, uint progress, int source = 0)
		{
			if (task.isComplete)
			{
				return;
			}
			int requiredItemId = task.Definition.requiredItemId;
			switch (task.Definition.Type)
			{
			case global::Kampai.Game.MasterPlanComponentTaskType.Deliver:
			case global::Kampai.Game.MasterPlanComponentTaskType.Collect:
				task.earnedQuantity = playerService.GetQuantity((global::Kampai.Game.StaticItem)requiredItemId);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.CompleteOrders:
				task.earnedQuantity += progress;
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.PlayMiniGame:
			case global::Kampai.Game.MasterPlanComponentTaskType.MiniGameScore:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnLeisurePartyPoints:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnMignettePartyPoints:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars:
				if (requiredItemId == 0 || requiredItemId == source)
				{
					task.earnedQuantity += progress;
				}
				break;
			default:
				logger.Error("Undefined component task type: {0}", task.Definition.Type);
				break;
			}
		}

		private global::Kampai.Game.MasterPlanComponent GetActiveComponent()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MasterPlanComponent>();
			foreach (global::Kampai.Game.MasterPlanComponent item in instancesByType)
			{
				if (item.State == global::Kampai.Game.MasterPlanComponentState.NotStarted || item.State == global::Kampai.Game.MasterPlanComponentState.Complete)
				{
					continue;
				}
				return item;
			}
			return null;
		}

		public global::Kampai.Game.MasterPlanComponent GetActiveComponentFromPlanDefinition(int masterPlanDefinitionID)
		{
			global::Kampai.Game.MasterPlanDefinition masterPlanDefinition = definitionService.Get<global::Kampai.Game.MasterPlanDefinition>(masterPlanDefinitionID);
			for (int i = 0; i < masterPlanDefinition.ComponentDefinitionIDs.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(masterPlanDefinition.ComponentDefinitionIDs[i]);
				if (firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.State != global::Kampai.Game.MasterPlanComponentState.NotStarted && firstInstanceByDefinitionId.State != global::Kampai.Game.MasterPlanComponentState.Complete)
				{
					return firstInstanceByDefinitionId;
				}
			}
			return null;
		}

		public bool AllComponentsAreComplete(int masterPlanDefinitionID)
		{
			global::Kampai.Game.MasterPlanDefinition masterPlanDefinition = definitionService.Get<global::Kampai.Game.MasterPlanDefinition>(masterPlanDefinitionID);
			for (int i = 0; i < masterPlanDefinition.ComponentDefinitionIDs.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(masterPlanDefinition.ComponentDefinitionIDs[i]);
				if (firstInstanceByDefinitionId == null || firstInstanceByDefinitionId.State != global::Kampai.Game.MasterPlanComponentState.Complete)
				{
					return false;
				}
			}
			return true;
		}

		public int GetComponentCompleteCount(global::Kampai.Game.MasterPlanDefinition definition)
		{
			int num = 0;
			foreach (int componentDefinitionID in definition.ComponentDefinitionIDs)
			{
				global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(componentDefinitionID);
				if (firstInstanceByDefinitionId.State == global::Kampai.Game.MasterPlanComponentState.Complete)
				{
					num++;
				}
			}
			return num;
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> GetInactiveComponents(global::Kampai.Game.MasterPlanDefinition masterPlanDefinition)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> list = new global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponent>();
			foreach (int componentDefinitionID in masterPlanDefinition.ComponentDefinitionIDs)
			{
				global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(componentDefinitionID);
				if (firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.State == global::Kampai.Game.MasterPlanComponentState.NotStarted && masterPlanDefinition.ComponentDefinitionIDs.Contains(firstInstanceByDefinitionId.Definition.ID))
				{
					list.Add(firstInstanceByDefinitionId);
				}
			}
			return list;
		}

		public void SetWayfinderState()
		{
			global::Kampai.Game.MasterPlan currentMasterPlan = CurrentMasterPlan;
			if (currentMasterPlan == null)
			{
				resetIconSignal.Dispatch();
				return;
			}
			int iD = currentMasterPlan.Definition.ID;
			global::Kampai.Game.MasterPlanComponent activeComponentFromPlanDefinition = GetActiveComponentFromPlanDefinition(iD);
			if (activeComponentFromPlanDefinition != null)
			{
				taskUpdatedSignal.Dispatch(activeComponentFromPlanDefinition);
				return;
			}
			global::Kampai.Game.MasterPlanDefinition masterPlanDefinition = definitionService.Get<global::Kampai.Game.MasterPlanDefinition>(iD);
			global::Kampai.Game.MasterPlanComponentBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponentBuilding>(masterPlanDefinition.BuildingDefID);
			if (AllComponentsAreComplete(iD) && firstInstanceByDefinitionId != null)
			{
				setCompleteIconSignal.Dispatch();
			}
			else
			{
				resetIconSignal.Dispatch();
			}
		}

		private global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponentDefinition> GetComponentDefinitions(global::Kampai.Game.MasterPlanDefinition masterPlanDefinition)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentDefinition> list = new global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentDefinition>();
			foreach (int componentDefinitionID in masterPlanDefinition.ComponentDefinitionIDs)
			{
				global::Kampai.Game.MasterPlanComponentDefinition item = definitionService.Get<global::Kampai.Game.MasterPlanComponentDefinition>(componentDefinitionID);
				list.Add(item);
			}
			return list;
		}

		public global::UnityEngine.Vector3 GetComponentBuildingOffset(int buildingID)
		{
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			global::Kampai.Game.VillainLairDefinition villainLairDefinition = definitionService.Get<global::Kampai.Game.VillainLairDefinition>(global::Kampai.Game.StaticItem.VILLAIN_LAIR_DEFINITION_ID);
			global::System.Collections.Generic.List<int> compBuildingDefinitionIDs = CurrentMasterPlan.Definition.CompBuildingDefinitionIDs;
			global::System.Collections.Generic.List<global::Kampai.Game.PlatformDefinition> platforms = villainLairDefinition.Platforms;
			int index = 0;
			if (platforms == null || platforms.Count < 1)
			{
				return zero;
			}
			if (CurrentMasterPlan.Definition.BuildingDefID == buildingID)
			{
				index = platforms.Count - 1;
			}
			else
			{
				for (int i = 0; i < compBuildingDefinitionIDs.Count; i++)
				{
					if (compBuildingDefinitionIDs[i] == buildingID)
					{
						index = i;
						break;
					}
				}
			}
			return platforms[index].offset;
		}

		public global::UnityEngine.Vector3 GetComponentBuildingPosition(int buildingID)
		{
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			global::Kampai.Game.Location location = null;
			global::Kampai.Game.VillainLairDefinition villainLairDefinition = definitionService.Get<global::Kampai.Game.VillainLairDefinition>(global::Kampai.Game.StaticItem.VILLAIN_LAIR_DEFINITION_ID);
			global::System.Collections.Generic.List<int> compBuildingDefinitionIDs = CurrentMasterPlan.Definition.CompBuildingDefinitionIDs;
			global::System.Collections.Generic.List<global::Kampai.Game.PlatformDefinition> platforms = villainLairDefinition.Platforms;
			int index = 0;
			if (platforms == null || platforms.Count < 1)
			{
				return zero;
			}
			if (CurrentMasterPlan.Definition.BuildingDefID == buildingID)
			{
				index = platforms.Count - 1;
			}
			else
			{
				for (int i = 0; i < compBuildingDefinitionIDs.Count; i++)
				{
					if (compBuildingDefinitionIDs[i] == buildingID)
					{
						index = i;
						break;
					}
				}
			}
			zero = platforms[index].offset;
			location = platforms[index].placementLocation;
			return zero + (global::UnityEngine.Vector3)location;
		}
	}
}
