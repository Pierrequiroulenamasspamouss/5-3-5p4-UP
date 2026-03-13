namespace Kampai.Game
{
	public class OpenBuildingMenuCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("OpenBuildingMenuCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.View.BuildingObject BuildingObject { get; set; }

		[Inject]
		public global::Kampai.Game.Building Building { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject BuildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService DefinitionService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveSignal AutoMoveSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowHiddenBuildingsSignal showHiddenBuildingsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowBuildingDetailMenuSignal ShowBuildingDetailmenuSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenStageBuildingSignal openStageBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowBridgeUISignal BridgeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowNeedXMinionsSignal ShowNeedXMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenStorageBuildingSignal OpenStorageBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenVillainLairPortalBuildingSignal openVillainLairPortalSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenMinionUpgradeBuildingSignal openMinionUpgradeBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingZoomSignal BuildingZoomSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.MignetteGameModel MignetteGameModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberModel SpawnDooberModel { get; set; }

		[Inject]
		public global::Kampai.Game.MIBBuildingSelectedSignal MessageInABottleSelectedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.RepairBuildingSignal RepairBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.Game.OpenOrderBoardSignal openOrderBoardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardUpdateTicketOnBoardSignal updateTicketOnBoardSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.DCNShowFeaturedContentSignal dcnShowFeaturedContentSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToBuildingSignal moveToBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenVillainLairResourcePlotBuildingSignal openResourcePlotBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ClickedVillainLairComponentBuildingSignal clickedVillainLairComponentBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Common.TikiBarViewPickSignal tikiBarViewPickSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		public override void Execute()
		{
			TryOpenMenu(BuildingObject, Building);
		}

		private void TryOpenMenu(global::Kampai.Game.View.BuildingObject buildingObject, global::Kampai.Game.Building building)
		{
			global::Kampai.Game.View.BuildingManagerMediator component = BuildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerMediator>();
			if ((component.LastHarvestedBuildingID == building.ID && component.HarvestTimer > 0f) || pickControllerModel.IsInstanceIgnored(buildingObject.ID))
			{
				return;
			}
			if (building.State == global::Kampai.Game.BuildingState.Broken || building.State == global::Kampai.Game.BuildingState.MissingTikiSign)
			{
				RepairBuildingSignal.Dispatch(building);
				if (building is global::Kampai.Game.OrderBoard)
				{
					routineRunner.StartCoroutine(UpdateTickets());
				}
			}
			else
			{
				TryOpenBuildingMenu(buildingObject, building);
			}
		}

		private global::System.Collections.IEnumerator UpdateTickets()
		{
			yield return null;
			updateTicketOnBoardSignal.Dispatch();
		}

		private void TryOpenBuildingMenu(global::Kampai.Game.View.BuildingObject buildingObject, global::Kampai.Game.Building building)
		{
			global::Kampai.Game.BuildingState state = building.State;
			global::Kampai.Game.VillainLairEntranceBuilding villainLairEntranceBuilding = building as global::Kampai.Game.VillainLairEntranceBuilding;
			if (villainLairEntranceBuilding != null)
			{
				openVillainLairPortalSignal.Dispatch(villainLairEntranceBuilding, buildingObject as global::Kampai.Game.View.VillainLairEntranceBuildingObject);
			}
			else
			{
				if (((state != global::Kampai.Game.BuildingState.Idle && state != global::Kampai.Game.BuildingState.Working && state != global::Kampai.Game.BuildingState.Cooldown) || pickControllerModel.SelectedBuilding.HasValue || pickControllerModel.HeldTimer >= 0.75f) && !(building is global::Kampai.Game.CraftingBuilding))
				{
					return;
				}
				global::Kampai.Game.MIBBuilding mIBBuilding = building as global::Kampai.Game.MIBBuilding;
				if (mIBBuilding != null)
				{
					MessageInABottleSelectedSignal.Dispatch();
					return;
				}
				global::Kampai.Game.MignetteBuilding mignetteBuilding = building as global::Kampai.Game.MignetteBuilding;
				if (mignetteBuilding != null)
				{
					if (MignetteGameModel.IsMignetteActive || mignetteBuilding.AreAllMinionSlotsFilled() || SpawnDooberModel.DooberCounter > 0)
					{
						return;
					}
					global::Kampai.Game.MinionParty minionPartyInstance = PlayerService.GetMinionPartyInstance();
					if (minionPartyInstance != null && minionPartyInstance.IsPartyReady)
					{
						return;
					}
					int levelUnlocked = mignetteBuilding.Definition.LevelUnlocked;
					if (PlayerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) < levelUnlocked)
					{
						string aspirationalMessage = mignetteBuilding.Definition.AspirationalMessage;
						globalSFXSignal.Dispatch("Play_action_locked_01");
						popupMessageSignal.Dispatch(localService.GetString(aspirationalMessage, levelUnlocked), global::Kampai.UI.View.PopupMessageType.NORMAL);
						return;
					}
					if (!HasEnoughFreeMinionsToAssignToBuilding(PlayerService, mignetteBuilding))
					{
						ShowNeedXMinionsSignal.Dispatch(mignetteBuilding.GetMinionSlotsOwned());
						return;
					}
				}
				global::Kampai.Game.DebrisBuilding debrisBuilding = building as global::Kampai.Game.DebrisBuilding;
				if (debrisBuilding != null && debrisBuilding.PaidInputCostToClear)
				{
					logger.Warning("Already bought debris building: {0}", building.ID);
					return;
				}
				global::Kampai.Game.TikiBarBuilding tikiBarBuilding = building as global::Kampai.Game.TikiBarBuilding;
				if (tikiBarBuilding != null)
				{
					tikiBarViewPickSignal.Dispatch(pickControllerModel.EndHitObject);
					BuildingZoomSignal.Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.IN, global::Kampai.Game.BuildingZoomType.TIKIBAR));
					return;
				}
				if (debrisBuilding == null && building.State != global::Kampai.Game.BuildingState.Cooldown)
				{
					global::Kampai.Game.TaskableBuilding taskableBuilding = building as global::Kampai.Game.TaskableBuilding;
					if (taskableBuilding != null && pickControllerModel.SelectedMinions.Count > 0 && taskableBuilding.GetMinionsInBuilding() < taskableBuilding.Definition.WorkStations)
					{
						return;
					}
				}
				global::Kampai.Game.StorageBuilding storageBuilding = building as global::Kampai.Game.StorageBuilding;
				if (storageBuilding != null && MignetteGameModel.IsMignetteActive)
				{
					return;
				}
				if (!(building is global::Kampai.Game.DecorationBuilding) && mignetteBuilding == null)
				{
					buildingObject.Bounce();
				}
				if (storageBuilding != null)
				{
					if (marketplaceService.AreThereSoldItems())
					{
						uiContext.injectionBinder.GetInstance<global::Kampai.Game.OpenSellBuildingModalSignal>().Dispatch();
					}
					else
					{
						OpenStorageBuildingSignal.Dispatch(storageBuilding, false);
					}
					return;
				}
				global::Kampai.Game.OrderBoard orderBoard = building as global::Kampai.Game.OrderBoard;
				if (orderBoard != null)
				{
					if (orderBoard.menuEnabled && !uiModel.LevelUpUIOpen)
					{
						openOrderBoardSignal.Dispatch(orderBoard);
					}
					return;
				}
				global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = building as global::Kampai.Game.VillainLairResourcePlot;
				if (villainLairResourcePlot != null)
				{
					openResourcePlotBuildingSignal.Dispatch(villainLairResourcePlot);
					return;
				}
				global::Kampai.Game.MasterPlanComponentBuilding masterPlanComponentBuilding = building as global::Kampai.Game.MasterPlanComponentBuilding;
				if (masterPlanComponentBuilding != null)
				{
					global::Kampai.Game.MasterPlanDefinition definition = masterPlanService.CurrentMasterPlan.Definition;
					for (int i = 0; i < definition.CompBuildingDefinitionIDs.Count; i++)
					{
						if (masterPlanComponentBuilding.Definition.ID == definition.CompBuildingDefinitionIDs[i])
						{
							int id = definition.ComponentDefinitionIDs[i];
							global::Kampai.Game.MasterPlanComponentDefinition type = DefinitionService.Get<global::Kampai.Game.MasterPlanComponentDefinition>(id);
							clickedVillainLairComponentBuildingSignal.Dispatch(type);
							break;
						}
					}
				}
				global::Kampai.Game.BridgeBuilding bridgeBuilding = building as global::Kampai.Game.BridgeBuilding;
				if (bridgeBuilding != null)
				{
					moveToBuildingSignal.Dispatch(building, new global::Kampai.Game.PanInstructions(building));
					BridgeSignal.Dispatch(bridgeBuilding.ID);
					return;
				}
				global::Kampai.Game.StageBuilding stageBuilding = building as global::Kampai.Game.StageBuilding;
				if (stageBuilding != null)
				{
					openStageBuildingSignal.Dispatch(stageBuilding);
					return;
				}
				global::Kampai.Game.MinionUpgradeBuilding minionUpgradeBuilding = building as global::Kampai.Game.MinionUpgradeBuilding;
				if (minionUpgradeBuilding != null)
				{
					openMinionUpgradeBuildingSignal.Dispatch();
					return;
				}
				global::Kampai.Game.DCNBuilding dCNBuilding = building as global::Kampai.Game.DCNBuilding;
				if (dCNBuilding != null)
				{
					dcnShowFeaturedContentSignal.Dispatch();
				}
				ShowBuildingDetailMenu(buildingObject, building);
			}
		}

		private void ShowBuildingDetailMenu(global::Kampai.Game.View.BuildingObject buildingObject, global::Kampai.Game.Building building)
		{
			ShowBuildingDetailmenuSignal.Dispatch(building);
			if (AllowPan(building))
			{
				PanToBuilding(buildingObject, building);
			}
		}

		private void PanToBuilding(global::Kampai.Game.View.BuildingObject buildingObject, global::Kampai.Game.Building building)
		{
			global::UnityEngine.Vector3 zoomCenter = buildingObject.ZoomCenter;
			global::Kampai.Game.CameraMovementSettings cameraMovementSettings = new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.KeepUIOpen, building, null);
			cameraMovementSettings.cameraSpeed = 0.4f;
			global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition> type = new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(building.Definition.ScreenPosition);
			AutoMoveSignal.Dispatch(zoomCenter, type, cameraMovementSettings, false);
			showHiddenBuildingsSignal.Dispatch();
		}

		public bool AllowPan(global::Kampai.Game.Building building)
		{
			global::Kampai.Game.CompositeBuilding compositeBuilding = building as global::Kampai.Game.CompositeBuilding;
			if (compositeBuilding != null)
			{
				return false;
			}
			return true;
		}

		public static bool HasEnoughFreeMinionsToAssignToBuilding(global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.TaskableBuilding building)
		{
			return playerService.HasPurchasedMinigamePack() || playerService.GetIdleMinions().Count >= building.GetMinionSlotsOwned() - building.GetMinionsInBuilding();
		}
	}
}
