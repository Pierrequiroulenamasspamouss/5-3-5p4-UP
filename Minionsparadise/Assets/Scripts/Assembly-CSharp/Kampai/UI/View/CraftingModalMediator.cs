namespace Kampai.UI.View
{
	public class CraftingModalMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.CraftingModalView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CraftingModalMediator") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.List<global::Kampai.Game.CraftingBuilding> validCraftingBuildingList = new global::System.Collections.Generic.List<global::Kampai.Game.CraftingBuilding>();

		private global::Kampai.Game.CameraAutoPanCompleteSignal cameraAutoPanCompleteSignal;

		private global::Kampai.Game.EndPartyBuffTimerSignal endPartyBuffTimerSignal;

		private global::Kampai.Game.ToggleVignetteSignal toggleVignetteSignal;

		private int currentIndex;

		private int craftingBuildingCount;

		private readonly global::Kampai.Game.AdPlacementName adPlacementName = global::Kampai.Game.AdPlacementName.CRAFTING;

		private global::Kampai.Game.AdPlacementInstance adPlacementInstance;

		private global::System.Collections.IEnumerator updateAdButtonCoroutine;

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshQueueSlotSignal refreshSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CraftingModalParams craftingModalParams { get; set; }

		[Inject]
		public global::Kampai.UI.View.CraftingModalClosedSignal closedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CraftingQueuePositionUpdateSignal queuePositionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideHUDAndIconsSignal hideHUDSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal closeItemPopupSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ResetDoubleTapSignal resetDoubleTapSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel model { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllMessageDialogs closeAllMessageDialogs { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateQueueIcon updateQueueSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.GoToResourceButtonClickedSignal gotoSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IGoToService gotoService { get; set; }

		[Inject]
		public global::Kampai.Game.IRewardedAdService rewardedAdService { get; set; }

		[Inject]
		public global::Kampai.Game.RewardedAdRewardSignal rewardedAdRewardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AdPlacementActivityStateChangedSignal adPlacementActivityStateChangedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		public override void OnRegister()
		{
			model.CraftingUIOpen = true;
			if (craftingModalParams.highlight)
			{
				craftingModalParams.highlight = false;
				base.view.highlightItem = true;
				base.view.higlightItemId = craftingModalParams.itemId;
			}
			base.OnRegister();
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.backArrow.ClickedSignal.AddListener(BackArrow);
			base.view.forwardArrow.ClickedSignal.AddListener(ForwardArrow);
			base.view.freeRush.ClickedSignal.AddListener(FreeRush);
			refreshSignal.AddListener(RefreshQueue);
			queuePositionSignal.AddListener(UpdateQueuePosition);
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			cameraAutoPanCompleteSignal = injectionBinder.GetInstance<global::Kampai.Game.CameraAutoPanCompleteSignal>();
			endPartyBuffTimerSignal = injectionBinder.GetInstance<global::Kampai.Game.EndPartyBuffTimerSignal>();
			toggleVignetteSignal = injectionBinder.GetInstance<global::Kampai.Game.ToggleVignetteSignal>();
			cameraAutoPanCompleteSignal.AddListener(PanComplete);
			endPartyBuffTimerSignal.AddListener(MinionPartyEnded);
			toggleVignetteSignal.Dispatch(true, null);
			resetDoubleTapSignal.AddListener(ResetDoubleTap);
			updateQueueSignal.AddListener(OnUpdateQueue);
			rewardedAdRewardSignal.AddListener(OnRewardedAdReward);
			adPlacementActivityStateChangedSignal.AddListener(OnAdPlacementActivityStateChanged);
			buildingChangeStateSignal.AddListener(OnBuildingChangeState);
			hideHUDSignal.Dispatch(false);
			gotoSignal.AddListener(GotoResourceBuilding);
		}

		public override void OnRemove()
		{
			model.CraftingUIOpen = false;
			base.OnRemove();
			if (base.view.buildingObject != null)
			{
				base.view.buildingObject.DisableHighLightBuilding();
			}
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.backArrow.ClickedSignal.RemoveListener(BackArrow);
			base.view.forwardArrow.ClickedSignal.RemoveListener(ForwardArrow);
			base.view.freeRush.ClickedSignal.RemoveListener(FreeRush);
			refreshSignal.RemoveListener(RefreshQueue);
			queuePositionSignal.RemoveListener(UpdateQueuePosition);
			cameraAutoPanCompleteSignal.RemoveListener(PanComplete);
			endPartyBuffTimerSignal.RemoveListener(MinionPartyEnded);
			resetDoubleTapSignal.RemoveListener(ResetDoubleTap);
			updateQueueSignal.RemoveListener(OnUpdateQueue);
			rewardedAdRewardSignal.RemoveListener(OnRewardedAdReward);
			adPlacementActivityStateChangedSignal.RemoveListener(OnAdPlacementActivityStateChanged);
			buildingChangeStateSignal.RemoveListener(OnBuildingChangeState);
			if (updateAdButtonCoroutine != null)
			{
				StopCoroutine(updateAdButtonCoroutine);
				updateAdButtonCoroutine = null;
			}
			gotoSignal.RemoveListener(GotoResourceBuilding);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.Game.CraftingBuilding craftingBuilding = args.Get<global::Kampai.Game.CraftingBuilding>();
			global::UnityEngine.GameObject gameObject = gameContext.injectionBinder.GetInstance(typeof(global::UnityEngine.GameObject), global::Kampai.Game.GameElement.BUILDING_MANAGER) as global::UnityEngine.GameObject;
			global::Kampai.Game.View.BuildingManagerView component = gameObject.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.CraftableBuildingObject buildingObject = component.GetBuildingObject(craftingBuilding.ID) as global::Kampai.Game.View.CraftableBuildingObject;
			if (craftingBuilding != null && craftingBuilding.State == global::Kampai.Game.BuildingState.Construction)
			{
				OnMenuClose();
			}
			else
			{
				int num = args.Get<int>();
				Init(num, buildingObject);
				InitCraftingBuildingList(num);
				CheckPartyState();
			}
			ShowCraftingInstruction();
		}

		private void ShowCraftingInstruction()
		{
			if (!localPersistService.HasKey("didyouknow_Crafting"))
			{
				popupMessageSignal.Dispatch(localService.GetString("CraftingDescription"), global::Kampai.UI.View.PopupMessageType.AUTO_CLOSE_OVERRIDE);
				localPersistService.PutDataInt("didyouknow_Crafting", 1);
			}
		}

		private void CheckPartyState()
		{
			float currentBuffMultiplierForBuffType = guestService.GetCurrentBuffMultiplierForBuffType(global::Kampai.Game.BuffType.PRODUCTION);
			base.view.SetPartyInfo(currentBuffMultiplierForBuffType, localService.GetString("partyBuffMultiplier", currentBuffMultiplierForBuffType), playerService.GetMinionPartyInstance().IsBuffHappening);
		}

		private void MinionPartyEnded(int buffDuration)
		{
			base.view.SetPartyInfo(1f, string.Empty, false);
		}

		protected override void Close()
		{
			closeItemPopupSignal.Dispatch();
			soundFXSignal.Dispatch("Play_menu_disappear_01");
			base.view.Close();
			toggleVignetteSignal.Dispatch(false, null);
			hideHUDSignal.Dispatch(true);
			closeAllMessageDialogs.Dispatch();
		}

		private void RefreshQueue(bool purchasing)
		{
			if (purchasing)
			{
				playerService.BuyCraftingSlot(base.view.building.ID);
			}
			base.view.RefreshQueue();
			base.view.SetChildrenAsPartying();
		}

		private void UpdateQueuePosition()
		{
			base.view.UpdateQueuePosition();
			closeAllMessageDialogs.Dispatch();
		}

		private void BackArrow()
		{
			if (currentIndex <= 0)
			{
				currentIndex = craftingBuildingCount - 1;
			}
			else
			{
				currentIndex--;
			}
			OpenBuildingMenu();
		}

		private void PopulateBuilding(global::Kampai.Game.CraftingBuilding building)
		{
			global::UnityEngine.GameObject gameObject = gameContext.injectionBinder.GetInstance(typeof(global::UnityEngine.GameObject), global::Kampai.Game.GameElement.BUILDING_MANAGER) as global::UnityEngine.GameObject;
			global::Kampai.Game.View.BuildingManagerView component = gameObject.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.CraftableBuildingObject buildingObject = component.GetBuildingObject(building.ID) as global::Kampai.Game.View.CraftableBuildingObject;
			base.view.RePopulateModal(building, buildingObject, global::Kampai.UI.View.HighlightType.DRAG);
			base.view.SetChildrenAsPartying();
			base.view.SetTitle(localService.GetString(building.Definition.LocalizedKey));
		}

		private void OpenBuildingMenu()
		{
			base.view.SetArrowButtonState(false);
			global::Kampai.Game.CraftingBuilding building = validCraftingBuildingList[currentIndex];
			PopulateBuilding(building);
			PanAndShowBuildingMenu(building);
			ScheduleAdButtonUpdate();
		}

		private void PanComplete()
		{
			base.view.SetArrowButtonState(true);
			if (base.view.highlightItem)
			{
				base.view.ShowDragTutorial();
			}
		}

		private void ForwardArrow()
		{
			if (currentIndex >= craftingBuildingCount - 1)
			{
				currentIndex = 0;
			}
			else
			{
				currentIndex++;
			}
			OpenBuildingMenu();
		}

		private void FreeRush()
		{
			if (adPlacementInstance != null)
			{
				rewardedAdService.ShowRewardedVideo(adPlacementInstance);
			}
		}

		private void UpdateAdButton()
		{
			if (base.view.building != null)
			{
				global::Kampai.Game.CraftingBuilding building = base.view.building;
				int iD = building.ID;
				bool flag = rewardedAdService.IsPlacementActive(adPlacementName, iD);
				if (!flag)
				{
					logger.Debug("Ads: placement '{0}' for crafting building {1} is disabled.", adPlacementName, iD);
				}
				global::Kampai.Game.AdPlacementInstance placementInstance = rewardedAdService.GetPlacementInstance(adPlacementName, iD);
				bool enable = flag && !playerService.isStorageFull() && IsBuildingInProduction() && placementInstance != null;
				base.view.EnableRewardedAdRushButton(enable);
				adPlacementInstance = placementInstance;
			}
		}

		private void ScheduleAdButtonUpdate()
		{
			if (updateAdButtonCoroutine == null)
			{
				updateAdButtonCoroutine = UpdateAdButtonOnNextFrame();
				StartCoroutine(updateAdButtonCoroutine);
			}
		}

		private global::System.Collections.IEnumerator UpdateAdButtonOnNextFrame()
		{
			yield return null;
			UpdateAdButton();
			updateAdButtonCoroutine = null;
		}

		private int GetBuildingId()
		{
			int result = -1;
			if (base.view != null && base.view.building != null)
			{
				result = base.view.building.ID;
			}
			return result;
		}

		private void OnRewardedAdReward(global::Kampai.Game.AdPlacementInstance placement)
		{
			if (placement.PlacementInstanceId != GetBuildingId())
			{
				return;
			}
			global::Kampai.UI.View.CraftingQueueView firstQueueItem = base.view.GetFirstQueueItem();
			if (firstQueueItem != null && IsBuildingInProduction())
			{
				global::Kampai.UI.View.CraftingQueueMediator component = firstQueueItem.GetComponent<global::Kampai.UI.View.CraftingQueueMediator>();
				if (component != null)
				{
					component.Rush(0, false, false);
					rewardedAdService.RewardPlayer(null, placement);
					telemetryService.Send_Telemetry_EVT_AD_INTERACTION(placement.Definition.Name, firstQueueItem.itemDef, placement.RewardPerPeriodCount);
				}
			}
			adPlacementInstance = null;
		}

		private bool IsBuildingInProduction()
		{
			global::Kampai.UI.View.CraftingQueueView firstQueueItem = base.view.GetFirstQueueItem();
			if (firstQueueItem != null)
			{
				return firstQueueItem.inProduction;
			}
			return false;
		}

		private void OnAdPlacementActivityStateChanged(global::Kampai.Game.AdPlacementInstance placement, bool enabled)
		{
			ScheduleAdButtonUpdate();
		}

		private void OnBuildingChangeState(int buildingId, global::Kampai.Game.BuildingState buildingState)
		{
			if (GetBuildingId() == buildingId)
			{
				ScheduleAdButtonUpdate();
			}
		}

		private void PanAndShowBuildingMenu(global::Kampai.Game.Building building)
		{
			global::UnityEngine.GameObject instance = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER);
			global::Kampai.Game.View.BuildingManagerView component = instance.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(building.ID);
			global::UnityEngine.Vector3 position = buildingObject.transform.position;
			global::Kampai.Game.ScreenPosition screenPosition = building.Definition.ScreenPosition;
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveSignal>().Dispatch(position, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.KeepUIOpen, building, null), false);
		}

		private void InitCraftingBuildingList(int craftingBuildingID)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.CraftingBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.CraftingBuilding>();
			for (int i = 0; i < instancesByType.Count; i++)
			{
				global::Kampai.Game.CraftingBuilding craftingBuilding = instancesByType[i];
				if (craftingBuilding.State != global::Kampai.Game.BuildingState.Construction && craftingBuilding.State != global::Kampai.Game.BuildingState.Inventory && craftingBuilding.State != global::Kampai.Game.BuildingState.Inactive && craftingBuilding.State != global::Kampai.Game.BuildingState.Cooldown && craftingBuilding.State != global::Kampai.Game.BuildingState.Complete)
				{
					if (craftingBuildingID == craftingBuilding.ID)
					{
						currentIndex = validCraftingBuildingList.Count;
					}
					validCraftingBuildingList.Add(craftingBuilding);
				}
			}
			craftingBuildingCount = validCraftingBuildingList.Count;
			if (craftingBuildingCount <= 1)
			{
				base.view.backArrow.gameObject.SetActive(false);
				base.view.forwardArrow.gameObject.SetActive(false);
			}
		}

		private void OnMenuClose()
		{
			closedSignal.Dispatch();
			hideSignal.Dispatch("CraftingSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_CraftingMenu");
		}

		private void ResetDoubleTap(int id)
		{
			base.view.ResetDoubleTap(id);
		}

		private void Init(int buildingID, global::Kampai.Game.View.CraftableBuildingObject buildingObject)
		{
			global::Kampai.Game.CraftingBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.CraftingBuilding>(buildingID);
			base.view.Init(playerService, definitionService, questService, byInstanceId, buildingObject);
			base.view.SetTitle(localService.GetString(byInstanceId.Definition.LocalizedKey));
			ScheduleAdButtonUpdate();
		}

		private void OnUpdateQueue()
		{
			ScheduleAdButtonUpdate();
			base.view.CleanupTweens();
		}

		private void GotoResourceBuilding(int itemDefinitionId)
		{
			global::Kampai.Game.CraftingBuilding craftingBuilding = validCraftingBuildingList[currentIndex];
			int buildingDefintionIDFromItemDefintionID = definitionService.GetBuildingDefintionIDFromItemDefintionID(itemDefinitionId);
			if (buildingDefintionIDFromItemDefintionID == craftingBuilding.Definition.ID)
			{
				base.view.highlightItem = true;
				base.view.higlightItemId = itemDefinitionId;
				PopulateBuilding(craftingBuilding);
				base.view.ShowDragTutorial();
				return;
			}
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.Building>(buildingDefintionIDFromItemDefintionID);
			if (byDefinitionId.Count > 0 && global::System.Linq.Enumerable.First(byDefinitionId) is global::Kampai.Game.CraftingBuilding)
			{
				for (int i = 0; i < validCraftingBuildingList.Count; i++)
				{
					if (validCraftingBuildingList[i].Definition.ID == buildingDefintionIDFromItemDefintionID)
					{
						currentIndex = i;
						base.view.highlightItem = true;
						base.view.higlightItemId = itemDefinitionId;
						OpenBuildingMenu();
						return;
					}
				}
			}
			Close();
			gotoService.GoToBuildingFromItem(itemDefinitionId);
		}
	}
}
