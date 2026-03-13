namespace Kampai.UI.View
{
	public class ResourceModalMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.ResourceModalView>
	{
		private global::Kampai.UI.View.ModalSettings modalSettings = new global::Kampai.UI.View.ModalSettings();

		private global::System.Collections.Generic.List<global::Kampai.Game.ResourceBuilding> validResourceBuildingList = new global::System.Collections.Generic.List<global::Kampai.Game.ResourceBuilding>();

		private int currentIndex;

		private int resourceBuildingCount;

		private bool initialized;

		private global::Kampai.Game.CameraAutoPanCompleteSignal cameraAutoPanCompleteSignal;

		private global::Kampai.Game.EndPartyBuffTimerSignal endPartyBuffTimerSignal;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateSliderSignal updateSliderSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFX { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateUIButtonsSignal updateLevelSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.FTUEProgressSignal ftueSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeOtherMenusSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			ftueSignal.Dispatch();
			base.view.LeftArrow.ClickedSignal.AddListener(MoveToPreviousBuilding);
			base.view.RightArrow.ClickedSignal.AddListener(MoveToNextBuilding);
			updateSliderSignal.AddListener(UpdateDisplay);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			updateLevelSignal.AddListener(LevelUnlockItems);
			endPartyBuffTimerSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.EndPartyBuffTimerSignal>();
			endPartyBuffTimerSignal.AddListener(MinionPartyEnded);
			cameraAutoPanCompleteSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraAutoPanCompleteSignal>();
			cameraAutoPanCompleteSignal.AddListener(PanComplete);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.HideAllResourceIconsSignal>().Dispatch();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			updateSliderSignal.RemoveListener(UpdateDisplay);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			updateLevelSignal.RemoveListener(LevelUnlockItems);
			endPartyBuffTimerSignal.RemoveListener(MinionPartyEnded);
			cameraAutoPanCompleteSignal.RemoveListener(PanComplete);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShowAllResourceIconsSignal>().Dispatch();
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			if (!initialized)
			{
				initialized = true;
				global::Kampai.Game.ResourceBuilding resourceBuilding = args.Get<global::Kampai.Game.ResourceBuilding>();
				modalSettings.enableRushButtons = !args.Contains<global::Kampai.UI.DisableRushButtons>();
				modalSettings.enableHarvestButtons = playerService.HasStorageBuilding();
				modalSettings.enableCallButtons = !args.Contains<global::Kampai.UI.DisableCallButtons>();
				modalSettings.enableLockedButtons = !args.Contains<global::Kampai.UI.DisableLockedButton>();
				modalSettings.enableRushThrob = args.Contains<global::Kampai.UI.ThrobRushButtons>();
				modalSettings.enableCallThrob = args.Contains<global::Kampai.UI.ThrobCallButtons>();
				modalSettings.enableLockedThrob = args.Contains<global::Kampai.UI.ThrobLockedButtons>();
				global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData = args.Get<global::Kampai.UI.BuildingPopupPositionData>();
				closeOtherMenusSignal.Dispatch(base.gameObject);
				Init(resourceBuilding, buildingPopupPositionData);
				InitResourceBuildingList(resourceBuilding);
				CheckPartyState();
			}
		}

		private void CheckPartyState()
		{
			float currentBuffMultiplierForBuffType = guestService.GetCurrentBuffMultiplierForBuffType(global::Kampai.Game.BuffType.PRODUCTION);
			base.view.SetPartyInfo(currentBuffMultiplierForBuffType, localService.GetString("partyBuffMultiplier", currentBuffMultiplierForBuffType), playerService.GetMinionPartyInstance().IsBuffHappening);
		}

		private void MinionPartyEnded(int duration)
		{
			if (initialized)
			{
				base.view.SetPartyInfo(1f, string.Empty, false);
			}
		}

		private void MoveToPreviousBuilding()
		{
			if (currentIndex <= 0)
			{
				currentIndex = resourceBuildingCount - 1;
			}
			else
			{
				currentIndex--;
			}
			OpenBuildingMenu();
		}

		private void OpenBuildingMenu()
		{
			base.view.ResetRushButtonsState();
			base.view.SetArrowButtonState(false);
			global::Kampai.Game.ResourceBuilding building = validResourceBuildingList[currentIndex];
			RecreateModal(building);
			PanAndShowBuildingMenu(building);
		}

		private void PanComplete()
		{
			base.view.SetArrowButtonState(true);
		}

		private void MoveToNextBuilding()
		{
			if (currentIndex >= resourceBuildingCount - 1)
			{
				currentIndex = 0;
			}
			else
			{
				currentIndex++;
			}
			OpenBuildingMenu();
		}

		private void PanAndShowBuildingMenu(global::Kampai.Game.Building building)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			global::UnityEngine.GameObject instance = injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER);
			global::Kampai.Game.View.BuildingManagerView component = instance.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(building.ID);
			global::UnityEngine.Vector3 position = buildingObject.transform.position;
			global::Kampai.Game.ScreenPosition screenPosition = building.Definition.ScreenPosition;
			injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveSignal>().Dispatch(position, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.KeepUIOpen, building, null), false);
			injectionBinder.GetInstance<global::Kampai.Game.ShowHiddenBuildingsSignal>().Dispatch();
		}

		private void InitResourceBuildingList(global::Kampai.Game.ResourceBuilding currentResourceBuilding)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.ResourceBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.ResourceBuilding>();
			for (int i = 0; i < instancesByType.Count; i++)
			{
				global::Kampai.Game.ResourceBuilding resourceBuilding = instancesByType[i];
				if (resourceBuilding.State != global::Kampai.Game.BuildingState.Construction && resourceBuilding.State != global::Kampai.Game.BuildingState.Inventory && resourceBuilding.State != global::Kampai.Game.BuildingState.Inactive && resourceBuilding.State != global::Kampai.Game.BuildingState.Cooldown && resourceBuilding.State != global::Kampai.Game.BuildingState.Complete)
				{
					if (currentResourceBuilding.ID == resourceBuilding.ID)
					{
						currentIndex = validResourceBuildingList.Count;
					}
					validResourceBuildingList.Add(resourceBuilding);
				}
			}
			resourceBuildingCount = validResourceBuildingList.Count;
			if (resourceBuildingCount <= 1)
			{
				base.view.LeftArrow.gameObject.SetActive(false);
				base.view.RightArrow.gameObject.SetActive(false);
			}
		}

		private void Init(global::Kampai.Game.ResourceBuilding building, global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData)
		{
			if (building == null)
			{
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.Minion> list = new global::System.Collections.Generic.List<global::Kampai.Game.Minion>();
			foreach (int minion in building.MinionList)
			{
				if (!building.CompleteMinionQueue.Contains(minion))
				{
					list.Add(playerService.GetByInstanceId<global::Kampai.Game.Minion>(minion));
				}
			}
			base.view.Init(building, list, localService, definitionService, playerService, modalSettings, buildingPopupPositionData);
		}

		private void RecreateModal(global::Kampai.Game.ResourceBuilding building)
		{
			if (building == null)
			{
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.Minion> list = new global::System.Collections.Generic.List<global::Kampai.Game.Minion>();
			foreach (int minion in building.MinionList)
			{
				if (!building.CompleteMinionQueue.Contains(minion))
				{
					list.Add(playerService.GetByInstanceId<global::Kampai.Game.Minion>(minion));
				}
			}
			modalSettings.enableRushThrob = false;
			modalSettings.enableCallThrob = false;
			modalSettings.enableLockedThrob = false;
			base.view.RecreateModal(building, list, modalSettings);
		}

		protected override void Close()
		{
			globalSFX.Dispatch("Play_menu_disappear_01");
			base.view.Close();
		}

		private void OnMenuClose()
		{
			hideSignal.Dispatch("BuildingSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_BaseResource");
		}

		private void UpdateDisplay()
		{
			base.view.UpdateDisplay();
		}

		private void LevelUnlockItems(bool clearUnlock)
		{
			uint quantity = playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			base.view.LevelUpUnlock(quantity);
		}
	}
}
