namespace Kampai.UI.View
{
	public class LeisureBuildingMenuMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.LeisureBuildingMenuView>
	{
		private int index;

		private global::Kampai.Game.LeisureBuilding building;

		private global::System.Collections.Generic.IList<global::Kampai.Game.LeisureBuilding> leisureBuildings;

		private global::Kampai.UI.View.ModalSettings modalSettings = new global::Kampai.UI.View.ModalSettings();

		private global::Kampai.Game.EndPartyBuffTimerSignal endPartyBuffTimerSignal;

		private global::Kampai.Game.View.BuildingManagerView buildingManagerView;

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.SendMinionToLeisureSignal sendMinionToLeisureSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateLeisureMenuViewSignal updateLeisureMenuViewSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playGlobalSFX { get; set; }

		[Inject]
		public global::Kampai.UI.View.TryCollectLeisurePointsSignal tryCollectPoints { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		[Inject]
		public global::Kampai.Game.HarvestReadySignal harvestReadySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisableLeisureRushButtonSignal disableLeisureRushButtonSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			updateLeisureMenuViewSignal.AddListener(UpdateView);
			disableLeisureRushButtonSignal.AddListener(DisableRushButton);
			base.view.CallMinions.ClickedSignal.AddListener(CallMinions);
			base.view.CollectPoints.ClickedSignal.AddListener(CollectPoints);
			base.view.RushMinions.ClickedSignal.AddListener(RushMinions);
			base.view.PrevBuilding.ClickedSignal.AddListener(PreviousBuilding);
			base.view.NextBuilding.ClickedSignal.AddListener(NextBuilding);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			endPartyBuffTimerSignal = injectionBinder.GetInstance<global::Kampai.Game.EndPartyBuffTimerSignal>();
			endPartyBuffTimerSignal.AddListener(MinionPartyEnded);
			injectionBinder.GetInstance<global::Kampai.Game.PostMinionPartyEndSignal>().AddListener(UpdateView);
			injectionBinder.GetInstance<global::Kampai.Game.IdleMinionSignal>().AddListener(base.view.SetIdleMinionCount);
			injectionBinder.GetInstance<global::Kampai.Game.CameraAutoPanCompleteSignal>().AddListener(PanComplete);
			uiModel.LeisureMenuOpen = true;
		}

		public override void OnRemove()
		{
			base.OnRemove();
			updateLeisureMenuViewSignal.RemoveListener(UpdateView);
			disableLeisureRushButtonSignal.RemoveListener(DisableRushButton);
			base.view.CallMinions.ClickedSignal.RemoveListener(CallMinions);
			base.view.CollectPoints.ClickedSignal.RemoveListener(CollectPoints);
			base.view.RushMinions.ClickedSignal.RemoveListener(RushMinions);
			base.view.PrevBuilding.ClickedSignal.RemoveListener(PreviousBuilding);
			base.view.NextBuilding.ClickedSignal.RemoveListener(NextBuilding);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			endPartyBuffTimerSignal.RemoveListener(MinionPartyEnded);
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			injectionBinder.GetInstance<global::Kampai.Game.PostMinionPartyEndSignal>().RemoveListener(UpdateView);
			injectionBinder.GetInstance<global::Kampai.Game.IdleMinionSignal>().RemoveListener(base.view.SetIdleMinionCount);
			injectionBinder.GetInstance<global::Kampai.Game.CameraAutoPanCompleteSignal>().RemoveListener(PanComplete);
			uiModel.LeisureMenuOpen = false;
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			building = args.Get<global::Kampai.Game.LeisureBuilding>();
			leisureBuildings = playerService.GetInstancesByType<global::Kampai.Game.LeisureBuilding>();
			modalSettings.enableCallThrob = args.Contains<global::Kampai.UI.ThrobCallButtons>();
			modalSettings.enableRushThrob = args.Contains<global::Kampai.UI.ThrobRushButtons>();
			global::Kampai.UI.BuildingPopupPositionData positionData = args.Get<global::Kampai.UI.BuildingPopupPositionData>();
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			base.view.Init(localService, definitionService, playerService, timeEventService, injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.MINION_MANAGER), positionData);
			int num = 0;
			for (int i = 0; i < leisureBuildings.Count; i++)
			{
				global::Kampai.Game.LeisureBuilding leisureBuilding = leisureBuildings[i];
				if (leisureBuilding.State == global::Kampai.Game.BuildingState.Inventory)
				{
					leisureBuildings.RemoveAt(i);
					i--;
					continue;
				}
				num++;
				if (building.ID == leisureBuilding.ID)
				{
					index = i;
				}
			}
			if (num <= 1)
			{
				base.view.SetArrowsActive(false);
			}
			global::UnityEngine.GameObject instance = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER);
			buildingManagerView = instance.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			CheckPartyState();
			UpdateView();
		}

		protected override void Close()
		{
			hideSkrimSignal.Dispatch("BuildingSkrim");
			if (playGlobalSFX != null)
			{
				playGlobalSFX.Dispatch("Play_menu_disappear_01");
			}
			if (base.view != null)
			{
				base.view.Close();
			}
		}

		protected override void Update()
		{
			if (building != null && building.GetMinionsInBuilding() > 0)
			{
				base.view.SetClockTIme(building);
				base.view.SetRushCost(building);
				if (base.view.TimeRemaining == 0 && building.State == global::Kampai.Game.BuildingState.Working)
				{
					harvestReadySignal.Dispatch(building.ID);
					timeEventService.RemoveEvent(building.ID);
				}
			}
		}

		private void OnMenuClose()
		{
			base.view.Cleanup();
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_LeisureObject");
		}

		private void UpdateView()
		{
			if (building != null)
			{
				int partyPointsReward = building.Definition.PartyPointsReward;
				string time = UIUtils.FormatTime(building.Definition.LeisureTimeDuration, localService);
				base.view.SetTitle(building.Definition.LocalizedKey);
				if (playerService.IsMinionPartyUnlocked())
				{
					base.view.SetProduction("LeisureProduction", partyPointsReward, time);
				}
				else
				{
					base.view.SetProduction("LeisureProductionXP", partyPointsReward, time);
				}
				base.view.SetMinionsNeeded(building.Definition.WorkStations);
				base.view.SetRushCost(building);
				base.view.EnablePartyPoints(playerService.IsMinionPartyUnlocked());
				if (building.GetMinionsInBuilding() > 0 && building.State == global::Kampai.Game.BuildingState.Working)
				{
					base.view.EnableRush();
				}
				else if (building.UTCLastTaskingTimeStarted != 0 && building.State == global::Kampai.Game.BuildingState.Harvestable)
				{
					base.view.EnableCollect();
				}
				else
				{
					SetupCallButton();
				}
				base.view.SetIdleMinionCount();
				if (base.view.IsCallButtonEnabled())
				{
					base.view.Throb(base.view.CallMinions, modalSettings.enableCallThrob);
					base.view.Throb(base.view.RushMinions, modalSettings.enableRushThrob);
				}
			}
		}

		private void DisableRushButton()
		{
			base.view.DisableRushButton();
		}

		private void SetupCallButton()
		{
			base.view.EnableCallMinion();
			int highestMinionForLeisure = playerService.GetHighestMinionForLeisure(building.Definition.WorkStations);
			base.view.SetCallButtonInfo(highestMinionForLeisure);
		}

		private void CheckPartyState()
		{
			float currentBuffMultiplierForBuffType = guestService.GetCurrentBuffMultiplierForBuffType(global::Kampai.Game.BuffType.PARTY);
			base.view.SetPartyInfo(currentBuffMultiplierForBuffType, localService.GetString("partyBuffMultiplier", currentBuffMultiplierForBuffType), playerService.GetMinionPartyInstance().IsBuffHappening);
		}

		private void MinionPartyEnded(int duration)
		{
			base.view.SetPartyInfo(1f, string.Empty, false);
		}

		private void CollectPoints()
		{
			tryCollectPoints.Dispatch(building);
			UpdateView();
		}

		private void CallMinions()
		{
			modalSettings.enableCallThrob = false;
			sendMinionToLeisureSignal.Dispatch(building.ID);
			UpdateView();
		}

		private void RushMinions()
		{
			if (base.view.RushMinions.isDoubleConfirmed())
			{
				playerService.ProcessRush(base.view.rushCost, true, RushTransactionCallback, building.ID);
			}
			else if (modalSettings.enableRushButtons)
			{
				base.view.Throb(base.view.RushMinions, false);
				base.view.RushMinions.ShowConfirmMessage();
			}
		}

		private void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				playGlobalSFX.Dispatch("Play_button_premium_01");
				setPremiumCurrencySignal.Dispatch();
				modalSettings.enableRushThrob = false;
				if (timeEventService.HasEventID(building.ID))
				{
					timeEventService.RushEvent(building.ID);
				}
				else
				{
					gameContext.injectionBinder.GetInstance<global::Kampai.Game.HarvestReadySignal>().Dispatch(building.ID);
				}
			}
		}

		private void PreviousBuilding()
		{
			if (index <= 0)
			{
				index = leisureBuildings.Count - 1;
			}
			else
			{
				index--;
			}
			BeginPan();
		}

		private void NextBuilding()
		{
			if (index >= leisureBuildings.Count - 1)
			{
				index = 0;
			}
			else
			{
				index++;
			}
			BeginPan();
		}

		private void BeginPan()
		{
			building = leisureBuildings[index];
			base.view.SetArrowsInteractable(false);
			PanToBuilding(building);
			UpdateView();
		}

		private void PanToBuilding(global::Kampai.Game.Building building)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			global::Kampai.Game.View.BuildingObject buildingObject = buildingManagerView.GetBuildingObject(building.ID);
			global::UnityEngine.Vector3 position = buildingObject.transform.position;
			global::Kampai.Game.ScreenPosition screenPosition = building.Definition.ScreenPosition;
			injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveSignal>().Dispatch(position, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.KeepUIOpen, building, null), false);
		}

		private void PanComplete()
		{
			base.view.SetArrowsInteractable(true);
		}
	}
}
