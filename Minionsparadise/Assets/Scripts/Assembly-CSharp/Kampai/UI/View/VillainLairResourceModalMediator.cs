namespace Kampai.UI.View
{
	public class VillainLairResourceModalMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.VillainLairResourceModalView>
	{
		private global::Kampai.Game.VillainLair lair;

		private global::System.Collections.Generic.List<global::Kampai.Game.VillainLairResourcePlot> unlockedPlots;

		private global::System.Collections.Generic.List<global::Kampai.Game.VillainLairResourcePlot> lockedPlots;

		private int currentIndex;

		private global::Kampai.Game.VillainLairResourcePlot currentPlot;

		private bool movingToLockedPlot;

		private global::Kampai.Game.IdleMinionSignal idleMinionSignal;

		private global::Kampai.Game.PostMinionPartyEndSignal postMinionPartyEndSignal;

		private global::Kampai.Game.EndPartyBuffTimerSignal endPartyBuffTimerSignal;

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playGlobalSFX { get; set; }

		[Inject]
		public global::Kampai.Game.HighlightBuildingSignal highlightBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		[Inject]
		public global::Kampai.Game.SendMinionToLairResourcePlotSignal callMinionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UITryHarvestSignal tryHarvestSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AwardLairBonusDropsThenSetHarvestReadySignal awardDropsThenHarvestReadySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateVillainLairMenuViewSignal updateVillainLairMenuViewSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraMoveToCustomLairPlotSignal cameraMoveToCustomLairPlotSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraMoveToCustomPositionSignal cameraMoveToCustomPositionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenVillainLairResourcePlotBuildingSignal openResourcePlotBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel model { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.Init();
			base.view.Setup();
			base.view.prevButton.ClickedSignal.AddListener(PreviousBuilding);
			base.view.nextButton.ClickedSignal.AddListener(NextBuilding);
			base.view.callMinionButton.ClickedSignal.AddListener(CallMinionClicked);
			base.view.collectButton.ClickedSignal.AddListener(CollectClicked);
			base.view.rushButton.ClickedSignal.AddListener(RushClicked);
			updateVillainLairMenuViewSignal.AddListener(UpdateView);
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			idleMinionSignal = injectionBinder.GetInstance<global::Kampai.Game.IdleMinionSignal>();
			idleMinionSignal.AddListener(CheckIdleMinionCount);
			postMinionPartyEndSignal = injectionBinder.GetInstance<global::Kampai.Game.PostMinionPartyEndSignal>();
			postMinionPartyEndSignal.AddListener(UpdateView);
			endPartyBuffTimerSignal = injectionBinder.GetInstance<global::Kampai.Game.EndPartyBuffTimerSignal>();
			endPartyBuffTimerSignal.AddListener(MinionPartyEnded);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.Open();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.prevButton.ClickedSignal.RemoveListener(PreviousBuilding);
			base.view.nextButton.ClickedSignal.RemoveListener(NextBuilding);
			base.view.callMinionButton.ClickedSignal.RemoveListener(CallMinionClicked);
			base.view.collectButton.ClickedSignal.RemoveListener(CollectClicked);
			base.view.rushButton.ClickedSignal.RemoveListener(RushClicked);
			updateVillainLairMenuViewSignal.RemoveListener(UpdateView);
			idleMinionSignal.RemoveListener(CheckIdleMinionCount);
			postMinionPartyEndSignal.RemoveListener(UpdateView);
			endPartyBuffTimerSignal.RemoveListener(MinionPartyEnded);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			currentPlot = args.Get<global::Kampai.Game.VillainLairResourcePlot>();
			lair = currentPlot.parentLair;
			unlockedPlots = new global::System.Collections.Generic.List<global::Kampai.Game.VillainLairResourcePlot>();
			lockedPlots = new global::System.Collections.Generic.List<global::Kampai.Game.VillainLairResourcePlot>();
			int num = 0;
			foreach (int resourcePlotInstanceID in lair.resourcePlotInstanceIDs)
			{
				global::Kampai.Game.VillainLairResourcePlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairResourcePlot>(resourcePlotInstanceID);
				if (byInstanceId.State != global::Kampai.Game.BuildingState.Inaccessible)
				{
					if (resourcePlotInstanceID == currentPlot.ID)
					{
						currentIndex = num;
					}
					unlockedPlots.Add(byInstanceId);
					num++;
				}
				else
				{
					lockedPlots.Add(byInstanceId);
				}
			}
			base.view.EnableArrows(true);
			currentPlot = unlockedPlots[currentIndex];
			base.view.SetResourcePlotTitle(localizationService.GetString(lair.Definition.ResourcePlots[currentPlot.indexInLairResourcePlots].descriptionKey));
			EnablePlotHighlight(true);
			SetLairResourceDescription();
			CheckPartyState();
			UpdateView();
		}

		protected override void Update()
		{
			if (currentPlot != null && currentPlot.MinionIsTaskedToBuilding() && SetClockTimeAndRushCost() == 0 && currentPlot.State == global::Kampai.Game.BuildingState.Working)
			{
				awardDropsThenHarvestReadySignal.Dispatch(currentPlot.ID);
				timeEventService.RemoveEvent(currentPlot.ID);
			}
		}

		protected override void Close()
		{
			if (playGlobalSFX != null)
			{
				playGlobalSFX.Dispatch("Play_menu_disappear_01");
			}
			if (!movingToLockedPlot)
			{
				cameraMoveToCustomPositionSignal.Dispatch(60017, new global::Kampai.Util.Boxed<global::System.Action>(null));
			}
			if (base.view != null)
			{
				base.view.Close();
			}
		}

		private void OnMenuClose()
		{
			EnablePlotHighlight(false);
			if (!movingToLockedPlot)
			{
				hideSkrimSignal.Dispatch("VillainLairResourceSkrim");
			}
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_Resource_Lair_Unlocked");
		}

		private void CheckPartyState()
		{
			float currentBuffMultiplierForBuffType = guestService.GetCurrentBuffMultiplierForBuffType(global::Kampai.Game.BuffType.PRODUCTION);
			base.view.SetPartyInfo(currentBuffMultiplierForBuffType, localizationService.GetString("partyBuffMultiplier", currentBuffMultiplierForBuffType), playerService.GetMinionPartyInstance().IsBuffHappening);
		}

		private void MinionPartyEnded(int duration)
		{
			base.view.SetPartyInfo(1f, string.Empty, false);
		}

		private void SetLairResourceDescription()
		{
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(lair.Definition.ResourceItemID);
			string desc = localizationService.GetString("ResourceProd", localizationService.GetString(itemDefinition.LocalizedKey, 1), UIUtils.FormatTime(lair.Definition.SecondsToHarvest, localizationService));
			SetResourceItemAmount();
			base.view.SetResourceDescription(itemDefinition, desc);
		}

		private void SetResourceItemAmount()
		{
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(lair.Definition.ResourceItemID);
			int quantityByDefinitionId = (int)playerService.GetQuantityByDefinitionId(itemDefinition.ID);
			base.view.SetResourceItemAmount(quantityByDefinitionId);
		}

		private void SelectResourcePlotAndUpdate()
		{
			currentPlot = unlockedPlots[currentIndex];
			base.view.SetResourcePlotTitle(localizationService.GetString(lair.Definition.ResourcePlots[currentPlot.indexInLairResourcePlots].descriptionKey));
			EnablePlotHighlight(true);
			UpdateView();
		}

		private void EnablePlotHighlight(bool enable)
		{
			if (currentPlot != null)
			{
				highlightBuildingSignal.Dispatch(currentPlot.ID, enable);
			}
		}

		private void PreviousBuilding()
		{
			ChangeBuilding(false);
		}

		private void NextBuilding()
		{
			ChangeBuilding(true);
		}

		private void ChangeBuilding(bool next)
		{
			if (model.cameraFlow != null && model.cameraFlow.state != GoTweenState.Complete)
			{
				return;
			}
			EnablePlotHighlight(false);
			if (next)
			{
				if (currentIndex >= unlockedPlots.Count - 1)
				{
					if (OpenLockedPlotInstead(0))
					{
						return;
					}
					currentIndex = 0;
				}
				else
				{
					currentIndex++;
				}
			}
			else if (currentIndex <= 0)
			{
				if (OpenLockedPlotInstead(lockedPlots.Count - 1))
				{
					return;
				}
				currentIndex = unlockedPlots.Count - 1;
			}
			else
			{
				currentIndex--;
			}
			SelectResourcePlotAndUpdate();
		}

		private bool OpenLockedPlotInstead(int index)
		{
			if (lockedPlots.Count > 0)
			{
				global::Kampai.Game.VillainLairResourcePlot type = lockedPlots[index];
				openResourcePlotBuildingSignal.Dispatch(type);
				movingToLockedPlot = true;
				Close();
				return true;
			}
			return false;
		}

		private void PanToCurrentBuilding()
		{
			global::UnityEngine.Vector3 type = (global::UnityEngine.Vector3)currentPlot.Location + global::Kampai.Util.GameConstants.LairResourcePlotCustomUIOffsets.position;
			cameraMoveToCustomLairPlotSignal.Dispatch(type);
		}

		private void CallMinionClicked()
		{
			if (currentPlot != null)
			{
				callMinionSignal.Dispatch(currentPlot.ID);
				UpdateView();
			}
		}

		private void CollectClicked()
		{
			tryHarvestSignal.Dispatch(currentPlot.ID, UpdateView, true);
		}

		private void RushClicked()
		{
			if (currentPlot != null)
			{
				if (base.view.rushButton.isDoubleConfirmed())
				{
					playerService.ProcessRush(base.view.rushPrice, true, RushTransactionCallBack, currentPlot.ID);
				}
				else
				{
					base.view.rushButton.ShowConfirmMessage();
				}
			}
		}

		private void RushTransactionCallBack(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success && currentPlot != null)
			{
				playGlobalSFX.Dispatch("Play_button_premium_01");
				setPremiumCurrencySignal.Dispatch();
				if (timeEventService.HasEventID(currentPlot.ID))
				{
					timeEventService.RushEvent(currentPlot.ID);
				}
				else
				{
					awardDropsThenHarvestReadySignal.Dispatch(currentPlot.ID);
				}
			}
		}

		private int SetClockTimeAndRushCost()
		{
			if (currentPlot == null)
			{
				return -1;
			}
			int num = 0;
			int secondsToHarvest = lair.Definition.SecondsToHarvest;
			num = ((!timeEventService.HasEventID(currentPlot.ID)) ? secondsToHarvest : timeEventService.GetTimeRemaining(currentPlot.ID));
			num = global::UnityEngine.Mathf.Min(num, secondsToHarvest);
			base.view.rushPrice = timeEventService.CalculateRushCostForTimer(num, global::Kampai.Game.RushActionType.HARVESTING);
			if (base.view.rushPrice == 0)
			{
				base.view.SetStateFreeRush();
			}
			base.view.SetClockTimeAndRushCost(UIUtils.FormatTime(num, localizationService), base.view.rushPrice.ToString());
			return num;
		}

		private void UpdateView()
		{
			if (currentPlot != null)
			{
				SetResourceItemAmount();
				PanToCurrentBuilding();
				if (currentPlot.State == global::Kampai.Game.BuildingState.Working && currentPlot.UTCLastTaskingTimeStarted != 0)
				{
					base.view.SetStateRush();
					return;
				}
				if (currentPlot.State == global::Kampai.Game.BuildingState.Harvestable && currentPlot.UTCLastTaskingTimeStarted != 0)
				{
					base.view.SetStateCollect();
					return;
				}
				base.view.SetStateCallMinion();
				base.view.SetMinionLevel(playerService);
				CheckIdleMinionCount();
			}
		}

		private void CheckIdleMinionCount()
		{
			int count = playerService.GetIdleMinions().Count;
			base.view.SetAvailableMinionInformation(count);
		}
	}
}
