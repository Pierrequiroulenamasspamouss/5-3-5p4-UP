namespace Kampai.Game
{
	public class GoToService : global::Kampai.Game.IGoToService
	{
		public const float PLACEMENT_ZOOM_LEVEL = 0.4f;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GoToService") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.CameraAutoZoomSignal autoZoomSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IBuildMenuService buildMenuService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ClearNewBuildTabCount clearNewBuildTabCount { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseQuestBookSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeUISignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CraftingModalParams craftingModalParams { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ExitVillainLairSignal exitVillainLairSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenVillainLairResourcePlotBuildingSignal openVillainLairResourcePlotBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.FTUEQuestGoToSignal ftueGoToSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.HighlightTabSignal highlightTabSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToBuildingSignal moveToBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToMignetteSignal moveToMignetteSignal { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera myCamera { get; set; }

		[Inject]
		public global::Kampai.Game.OpenBuildingMenuSignal openBuildingMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenStoreHighlightItemSignal openStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenSellBuildingModalSignal openSellBuildingModalSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PanAndOpenModalSignal panAndOpenModalSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		[Inject]
		public global::Kampai.Game.EnterVillainLairSignal enterVillainLairSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToPositionSignal cameraAutoMoveToPositionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoPanCompleteSignal cameraAutoPanCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		public void GoToClicked(global::Kampai.Game.QuestStep step, global::Kampai.Game.QuestStepDefinition stepDefinition, global::Kampai.Game.IQuestController questController, int stepNumber)
		{
			ftueGoToSignal.Dispatch();
			switch (stepDefinition.Type)
			{
			case global::Kampai.Game.QuestStepType.Construction:
			case global::Kampai.Game.QuestStepType.BridgeRepair:
			case global::Kampai.Game.QuestStepType.CabanaRepair:
			case global::Kampai.Game.QuestStepType.WelcomeHutRepair:
			case global::Kampai.Game.QuestStepType.FountainRepair:
			case global::Kampai.Game.QuestStepType.StorageRepair:
			case global::Kampai.Game.QuestStepType.MinionUpgradeBuildingRepair:
				HandleBridgeAndConstruction(step, stepDefinition, questController.GetStepController(stepNumber));
				break;
			case global::Kampai.Game.QuestStepType.Delivery:
			case global::Kampai.Game.QuestStepType.Harvest:
			case global::Kampai.Game.QuestStepType.Leisure:
			case global::Kampai.Game.QuestStepType.PlayAnyLeisure:
			case global::Kampai.Game.QuestStepType.HarvestAnyLeisure:
				HandleDeliveryAndMinionTask(step, stepDefinition, questController.GetStepController(stepNumber));
				break;
			case global::Kampai.Game.QuestStepType.OrderBoard:
				HandleOrderBoard();
				break;
			case global::Kampai.Game.QuestStepType.Mignette:
				HandleMignette(stepDefinition.ItemDefinitionID);
				break;
			case global::Kampai.Game.QuestStepType.StageRepair:
				HandleStage(step.TrackedID);
				break;
			case global::Kampai.Game.QuestStepType.LairPortalRepair:
				HandleUniqueBuilding(step.TrackedID);
				break;
			case global::Kampai.Game.QuestStepType.MinionUpgrade:
			case global::Kampai.Game.QuestStepType.HaveUpgradedMinions:
				HandleUniqueBuilding(step.TrackedID, true);
				break;
			case global::Kampai.Game.QuestStepType.ThrowParty:
				HandleThrowParty();
				break;
			case global::Kampai.Game.QuestStepType.MinionTask:
				HandleMinionTask(stepDefinition.ItemDefinitionID);
				break;
			case global::Kampai.Game.QuestStepType.MysteryBoxOnboarding:
				HandleMysteryBoxOnboarding();
				break;
			case global::Kampai.Game.QuestStepType.MasterPlanTask:
				GoToClicked(stepDefinition as global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition);
				break;
			case global::Kampai.Game.QuestStepType.MasterPlanComponentBuild:
			case global::Kampai.Game.QuestStepType.MasterPlanBuild:
				HandleMasterPlanConstruction();
				break;
			default:
				logger.Error("Attempting to handle QuestStepDefinition.Type case {0}: invalid enum type. No action taken.", (int)stepDefinition.Type);
				break;
			}
			closeSignal.Dispatch();
		}

		public void GoToClicked(global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition taskDefinition)
		{
			switch (taskDefinition.taskDefinition.Type)
			{
			case global::Kampai.Game.MasterPlanComponentTaskType.Deliver:
			case global::Kampai.Game.MasterPlanComponentTaskType.Collect:
				GoToBuildingFromItem(taskDefinition.ItemDefinitionID);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.CompleteOrders:
				HandleOrderBoard();
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.PlayMiniGame:
			case global::Kampai.Game.MasterPlanComponentTaskType.MiniGameScore:
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnMignettePartyPoints:
				HandleMignette(taskDefinition.ItemDefinitionID);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints:
				HandlePartyPoints(taskDefinition.ItemDefinitionID);
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnLeisurePartyPoints:
				HandleDistractivity();
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars:
				HandleSandDollars(taskDefinition.ItemDefinitionID);
				break;
			default:
				logger.Error("Attempting to handle MasterPlanComponentTaskType case {0}: invalid enum type. No action taken.", (int)taskDefinition.taskDefinition.Type);
				break;
			}
		}

		public void GoToBuildingFromItem(int itemDefID)
		{
			int buildingDefintionIDFromItemDefintionID = definitionService.GetBuildingDefintionIDFromItemDefintionID(itemDefID);
			TransitionToBuildingUIorStore(buildingDefintionIDFromItemDefintionID, itemDefID);
		}

		private void HandleBridgeAndConstruction(global::Kampai.Game.QuestStep step, global::Kampai.Game.QuestStepDefinition stepDefinition, global::Kampai.Game.IQuestStepController questStepController)
		{
			if ((step.TrackedID == 0 || (questStepController.ProgressBarAmount != 0 && questStepController.ProgressBarAmount < questStepController.ProgressBarTotal)) && definitionService.Has<global::Kampai.Game.BuildingDefinition>(stepDefinition.ItemDefinitionID))
			{
				OpenStoreFromAnywhere(stepDefinition.ItemDefinitionID);
				return;
			}
			global::Kampai.Game.Building building = playerService.GetByInstanceId<global::Kampai.Game.Building>(step.TrackedID);
			if (building != null)
			{
				global::Kampai.Game.PanInstructions pi = new global::Kampai.Game.PanInstructions(building);
				pi.ZoomDistance = new global::Kampai.Util.Boxed<float>(0.5f);
				TransitionAway(building.Definition.ID, delegate
				{
					moveToBuildingSignal.Dispatch(building, pi);
				});
			}
		}

		private void HandleDeliveryAndMinionTask(global::Kampai.Game.QuestStep step, global::Kampai.Game.QuestStepDefinition stepDefinition, global::Kampai.Game.IQuestStepController questStepController)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Building> accessibleBuildingList = GetAccessibleBuildingList(step, questStepController);
			if (accessibleBuildingList.Count > 0)
			{
				global::Kampai.Game.Building building = accessibleBuildingList[randomService.NextInt(accessibleBuildingList.Count)];
				TransitionToBuildingLocation(building, global::Kampai.Util.GotoBuildingHelpers.BuildingMenuIsAccessible(building), stepDefinition.ItemDefinitionID);
			}
			else if (step.TrackedID <= 0)
			{
				switch (stepDefinition.Type)
				{
				case global::Kampai.Game.QuestStepType.Leisure:
				case global::Kampai.Game.QuestStepType.PlayAnyLeisure:
				case global::Kampai.Game.QuestStepType.HarvestAnyLeisure:
					TransitionAndHighlightStoreTab(global::Kampai.Game.StoreItemType.Leisure);
					break;
				case global::Kampai.Game.QuestStepType.ThrowParty:
				case global::Kampai.Game.QuestStepType.StorageRepair:
					break;
				}
			}
			else
			{
				TransitionToBuildingUIorStore(step.TrackedID);
			}
		}

		private void HandleOrderBoard()
		{
			TransitionToBuildingUIorStore(3022);
		}

		private void HandleMignette(int buildingDefinitionID)
		{
			if (buildingDefinitionID == 0)
			{
				buildingDefinitionID = global::Kampai.Util.GotoBuildingHelpers.GetSuitableMignette(playerService, definitionService);
			}
			if (playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(buildingDefinitionID) == null)
			{
				global::Kampai.Game.BuildingDefinition definition = null;
				if (!definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(buildingDefinitionID, out definition))
				{
					logger.Fatal(global::Kampai.Util.FatalCode.MIGNETTE_BAD_BUILDING_DEFINITION, buildingDefinitionID);
				}
			}
			TransitionAway(buildingDefinitionID, delegate
			{
				moveToMignetteSignal.Dispatch(buildingDefinitionID, false, new global::Kampai.Game.PanInstructions(null));
			});
		}

		private void HandleStage(int buildingID)
		{
			global::Kampai.Game.Building building = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingID);
			TransitionAway(building.Definition.ID, delegate
			{
				moveToBuildingSignal.Dispatch(building, new global::Kampai.Game.PanInstructions(building));
			});
		}

		private void HandleUniqueBuilding(int buildingID, bool moveToBuildingFirst = false)
		{
			global::Kampai.Game.Building building = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingID);
			global::Kampai.Game.View.BuildingManagerView component = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER).GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject bo = component.GetBuildingObject(building.ID);
			if (bo != null)
			{
				TransitionAway(building.Definition.ID, delegate
				{
					if (moveToBuildingFirst)
					{
						moveToBuildingSignal.Dispatch(building, new global::Kampai.Game.PanInstructions(building));
					}
					openBuildingMenuSignal.Dispatch(bo, building);
				});
			}
			else
			{
				logger.Error("could not open lair portal or minion upgrade asset: building is null");
			}
		}

		private void HandleThrowParty()
		{
			TransitionAway();
		}

		private void HandleMinionTask(int buildingDefID)
		{
			if (buildingDefID == 0)
			{
				buildingDefID = 3002;
			}
			TransitionToBuildingUIorStore(buildingDefID);
		}

		private void HandleMysteryBoxOnboarding()
		{
			global::Kampai.Game.MinionBenefitLevelBandDefintion minionBenefitLevelBandDefintion = definitionService.Get<global::Kampai.Game.MinionBenefitLevelBandDefintion>(global::Kampai.Game.StaticItem.MINION_BENEFITS_DEF_ID);
			global::Kampai.Game.Building firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(minionBenefitLevelBandDefintion.FirstBuildingId);
			global::Kampai.Game.ResourceBuilding resourceBuilding = firstInstanceByDefinitionId as global::Kampai.Game.ResourceBuilding;
			if (resourceBuilding != null)
			{
				bool openUI = true;
				if (resourceBuilding.BonusMinionItems != null && resourceBuilding.BonusMinionItems.Count > 0)
				{
					openUI = false;
				}
				TransitionToBuildingLocation(firstInstanceByDefinitionId, openUI);
			}
		}

		private void HandleMasterPlanConstruction()
		{
			if (villainLairModel.currentActiveLair == null)
			{
				zoomCameraModel.ZoomedIn = false;
				global::Kampai.Game.VillainLair firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.VillainLair>(3137);
				enterVillainLairSignal.Dispatch(firstInstanceByDefinitionId.ID, false);
			}
		}

		private void HandlePartyPoints(int buildingDefinitionID)
		{
			if (buildingDefinitionID != 0)
			{
				global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get<global::Kampai.Game.BuildingDefinition>(buildingDefinitionID);
				global::Kampai.Game.MignetteBuildingDefinition mignetteBuildingDefinition = buildingDefinition as global::Kampai.Game.MignetteBuildingDefinition;
				if (mignetteBuildingDefinition != null)
				{
					TransitionAway(buildingDefinitionID, delegate
					{
						moveToMignetteSignal.Dispatch(buildingDefinitionID, false, new global::Kampai.Game.PanInstructions(null));
					});
				}
				else
				{
					TransitionToBuildingUIorStore(buildingDefinitionID);
				}
			}
			else
			{
				GoToSuitableLeisureBuilding(true);
			}
		}

		private void HandleDistractivity()
		{
			GoToSuitableLeisureBuilding(false);
		}

		private void HandleSandDollars(int itemDefinitionID)
		{
			if (itemDefinitionID == 0)
			{
				global::System.Action onComplete = delegate
				{
					openSellBuildingModalSignal.Dispatch();
				};
				TransitionAway(0, onComplete);
			}
			else
			{
				TransitionToBuildingUIorStore(itemDefinitionID);
			}
		}

		public void OpenStoreFromAnywhere(int buildingDefinitionID)
		{
			if (villainLairModel.currentActiveLair != null)
			{
				exitVillainLairSignal.Dispatch(new global::Kampai.Util.Boxed<global::System.Action>(delegate
				{
					OpenStore(buildingDefinitionID);
				}));
			}
			else if (buildingDefinitionID == 3123 && questService.GetQuestMap().ContainsKey(101120) && questService.GetQuestMap()[101120].State == global::Kampai.Game.QuestState.RunningTasks && playerService.GetInstancesByDefinitionID(3123).Count == 0)
			{
				cameraAutoMoveToPositionSignal.Dispatch(new global::UnityEngine.Vector3(120f, 23f, 167f), 0.4f, true);
				cameraAutoPanCompleteSignal.AddOnce(delegate
				{
					OpenStore(buildingDefinitionID);
				});
			}
			else
			{
				PanOutTikiBar(delegate
				{
					OpenStore(buildingDefinitionID);
				});
			}
		}

		private void OpenStore(int buildingDefinitionID)
		{
			closeUISignal.Dispatch(null);
			uiModel.GoToInEffect = true;
			openStoreSignal.Dispatch(buildingDefinitionID, true);
			ZoomOutToPlacementLevel();
			int storeItemDefinitionIDFromBuildingID = buildMenuService.GetStoreItemDefinitionIDFromBuildingID(buildingDefinitionID);
			global::Kampai.Game.StoreItemDefinition storeItemDefinition = definitionService.Get<global::Kampai.Game.StoreItemDefinition>(storeItemDefinitionIDFromBuildingID);
			clearNewBuildTabCount.Dispatch(storeItemDefinition.Type);
		}

		private void TransitionAndHighlightStoreTab(global::Kampai.Game.StoreItemType itemType)
		{
			global::System.Action onComplete = delegate
			{
				highlightTabSignal.Dispatch(itemType);
			};
			TransitionAway(0, onComplete);
			ZoomOutToPlacementLevel();
		}

		private void TransitionAway(int targetBuildingDefID = 0, global::System.Action onComplete = null, bool forceTargetInLair = false)
		{
			bool flag = villainLairModel.currentActiveLair != null;
			bool flag2 = forceTargetInLair || BuildingBelongsInLair(targetBuildingDefID);
			if (flag)
			{
				if (flag2)
				{
					RunOnComplete(onComplete);
					return;
				}
				exitVillainLairSignal.Dispatch(new global::Kampai.Util.Boxed<global::System.Action>(delegate
				{
					RunOnComplete(onComplete);
				}));
			}
			else
			{
				PanOutTikiBar(delegate
				{
					RunOnComplete(onComplete);
				});
			}
		}

		private void TransitionToBuildingLocation(global::Kampai.Game.Building building, bool openUI = false, int itemDefinitionID = 0)
		{
			if (building != null)
			{
				if (!openUI && building.State != global::Kampai.Game.BuildingState.Inventory)
				{
					TransitionAway(building.Definition.ID, delegate
					{
						moveToBuildingSignal.Dispatch(building, new global::Kampai.Game.PanInstructions(building));
					});
				}
				else
				{
					TransitionToBuildingInstanceUIorStore(building, itemDefinitionID);
				}
			}
			else
			{
				logger.Error("Building instance is null when trying to pan to location");
			}
		}

		private void TransitionToBuildingUIorStore(int buildingDefinitionID, int itemDefinitionID = 0)
		{
			if (buildingDefinitionID == 0)
			{
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.Building> list = new global::System.Collections.Generic.List<global::Kampai.Game.Building>();
			foreach (global::Kampai.Game.Building item in playerService.GetByDefinitionId<global::Kampai.Game.Building>(buildingDefinitionID))
			{
				if (item.State != global::Kampai.Game.BuildingState.Inventory && item.State != global::Kampai.Game.BuildingState.Inaccessible)
				{
					list.Add(item);
				}
			}
			global::Kampai.Game.Building building = null;
			if (list.Count > 0)
			{
				building = list[randomService.NextInt(list.Count)];
			}
			if (building != null)
			{
				TransitionToBuildingInstanceUIorStore(building, itemDefinitionID);
				return;
			}
			int num = UpdateDefinitionToUseForNullBuilding(buildingDefinitionID);
			if (buildingDefinitionID == num)
			{
				OpenStoreFromAnywhere(buildingDefinitionID);
				return;
			}
			building = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(num);
			TransitionToBuildingInstanceUIorStore(building, itemDefinitionID);
		}

		private void TransitionToBuildingInstanceUIorStore(global::Kampai.Game.Building building, int itemDefinitionID = 0)
		{
			if (building != null)
			{
				int iD = building.Definition.ID;
				if (global::Kampai.Util.GotoBuildingHelpers.BuildingLivesInsideLair(building))
				{
					global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = building as global::Kampai.Game.VillainLairResourcePlot;
					if (villainLairResourcePlot != null)
					{
						GoToResourcePlotFromAnywhere(villainLairResourcePlot);
					}
					else
					{
						TransitionAway(iD);
					}
					return;
				}
				if (building.State == global::Kampai.Game.BuildingState.Inventory)
				{
					OpenStoreFromAnywhere(iD);
					return;
				}
				if (itemDefinitionID != 0)
				{
					SetPossibleCraftingParams(building, itemDefinitionID);
				}
				TransitionAway(iD, delegate
				{
					panAndOpenModalSignal.Dispatch(building.ID, false);
				});
			}
			else
			{
				logger.Error("Unable to transition to building instance (or UI): building is null.");
			}
		}

		private void GoToResourcePlotFromAnywhere(global::Kampai.Game.VillainLairResourcePlot plot)
		{
			if (plot == null)
			{
				return;
			}
			if (villainLairModel.currentActiveLair != null)
			{
				openVillainLairResourcePlotBuildingSignal.Dispatch(plot);
				return;
			}
			global::System.Action onComplete = delegate
			{
				panAndOpenModalSignal.Dispatch(plot.parentLair.portalInstanceID, false);
			};
			TransitionAway(0, onComplete);
		}

		private void PanOutTikiBar(global::System.Action onComplete = null)
		{
			if (zoomCameraModel.ZoomedIn)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.OUT, zoomCameraModel.LastZoomBuildingType, onComplete));
			}
			else if (onComplete != null)
			{
				onComplete();
			}
		}

		private void ZoomOutToPlacementLevel()
		{
			float currentPercentage = myCamera.GetComponent<global::Kampai.Game.View.ZoomView>().GetCurrentPercentage();
			if (currentPercentage > 0.4f)
			{
				autoZoomSignal.Dispatch(0.4f);
			}
		}

		private void RunOnComplete(global::System.Action onComplete = null)
		{
			if (onComplete != null)
			{
				onComplete();
			}
		}

		private int UpdateDefinitionToUseForNullBuilding(int buildingDefinitionID)
		{
			global::Kampai.Game.BuildingDefinition definition = null;
			definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(buildingDefinitionID, out definition);
			if (definition == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DS_GOTO_INVALID_DEF);
				return 0;
			}
			if (definition.Type == BuildingType.BuildingTypeIdentifier.LAIR_RESOURCEPLOT)
			{
				return 3132;
			}
			return buildingDefinitionID;
		}

		private void SetPossibleCraftingParams(global::Kampai.Game.Building building, int itemDefinitionID)
		{
			if (building.Definition.Type == BuildingType.BuildingTypeIdentifier.CRAFTING)
			{
				craftingModalParams.itemId = itemDefinitionID;
				craftingModalParams.highlight = true;
			}
		}

		private global::System.Collections.Generic.List<global::Kampai.Game.Building> GetAccessibleBuildingList(global::Kampai.Game.QuestStep step, global::Kampai.Game.IQuestStepController questStepController)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Building> list = new global::System.Collections.Generic.List<global::Kampai.Game.Building>();
			global::Kampai.Game.QuestStepType stepType = questStepController.StepType;
			if (stepType == global::Kampai.Game.QuestStepType.PlayAnyLeisure || stepType == global::Kampai.Game.QuestStepType.HarvestAnyLeisure)
			{
				global::System.Collections.Generic.List<global::Kampai.Game.LeisureBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.LeisureBuilding>();
				foreach (global::Kampai.Game.LeisureBuilding item in instancesByType)
				{
					if (item.State != global::Kampai.Game.BuildingState.Inventory)
					{
						list.Add(item);
					}
				}
			}
			else
			{
				foreach (global::Kampai.Game.Building item2 in playerService.GetByDefinitionId<global::Kampai.Game.Building>(step.TrackedID))
				{
					if (item2.State != global::Kampai.Game.BuildingState.Inventory)
					{
						list.Add(item2);
					}
				}
			}
			return list;
		}

		private void GoToSuitableLeisureBuilding(bool sortByHighestPoints)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> instancesByDefinition = playerService.GetInstancesByDefinition<global::Kampai.Game.LeisureBuildingDefintiion>();
			global::Kampai.Game.LeisureBuilding leisureBuilding = null;
			if (sortByHighestPoints)
			{
				leisureBuilding = GetHighestScoringOwnedLeisureBuilding(instancesByDefinition);
			}
			else
			{
				global::Kampai.Game.Building building = TryGetFirstBuildingNotInState(instancesByDefinition, global::Kampai.Game.BuildingState.Idle);
				if (building != null)
				{
					leisureBuilding = building as global::Kampai.Game.LeisureBuilding;
				}
			}
			if (leisureBuilding == null)
			{
				TransitionAndHighlightStoreTab(global::Kampai.Game.StoreItemType.Leisure);
			}
			else
			{
				TransitionToBuildingInstanceUIorStore(leisureBuilding);
			}
		}

		private global::Kampai.Game.LeisureBuilding GetHighestScoringOwnedLeisureBuilding(global::System.Collections.Generic.IList<global::Kampai.Game.Instance> leisureBuildings)
		{
			global::Kampai.Game.LeisureBuilding result = null;
			int num = 0;
			for (int i = 0; i < leisureBuildings.Count; i++)
			{
				global::Kampai.Game.LeisureBuilding leisureBuilding = leisureBuildings[i] as global::Kampai.Game.LeisureBuilding;
				if (leisureBuilding != null && leisureBuilding.Definition.PartyPointsReward > num)
				{
					num = leisureBuilding.Definition.PartyPointsReward;
					result = leisureBuilding;
				}
			}
			return result;
		}

		private global::Kampai.Game.Building TryGetFirstBuildingNotInState(global::System.Collections.Generic.IList<global::Kampai.Game.Instance> buildings, global::Kampai.Game.BuildingState excludedState)
		{
			global::Kampai.Game.Building building = null;
			for (int i = 0; i < buildings.Count; i++)
			{
				global::Kampai.Game.Instance instance = buildings[i];
				global::Kampai.Game.Building building2 = instance as global::Kampai.Game.Building;
				if (building2 != null && building2.State == excludedState)
				{
					building = building2;
					break;
				}
			}
			if (building == null && buildings.Count > 0)
			{
				building = buildings[0] as global::Kampai.Game.Building;
			}
			return building;
		}

		private bool BuildingBelongsInLair(int targetBuildingID)
		{
			if (targetBuildingID == 0)
			{
				return false;
			}
			global::Kampai.Game.BuildingDefinition definition;
			if (definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(targetBuildingID, out definition))
			{
				return global::Kampai.Util.GotoBuildingHelpers.BuildingLivesInsideLair(definition);
			}
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(targetBuildingID);
			if (byInstanceId != null)
			{
				return global::Kampai.Util.GotoBuildingHelpers.BuildingLivesInsideLair(byInstanceId);
			}
			return false;
		}
	}
}
