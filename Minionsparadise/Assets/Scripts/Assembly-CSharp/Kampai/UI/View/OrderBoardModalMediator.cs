namespace Kampai.UI.View
{
	public class OrderBoardModalMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.OrderBoardModalView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("OrderBoardModalMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.OrderBoard building;

		private int currentSelectedTickedIndex;

		private global::Kampai.Game.Transaction.TransactionDefinition currentTransactionDef;

		private global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> currentMissingItems;

		private int currentFulfilledTicket = -1;

		private global::Kampai.Game.Prestige currentSelectedPrestige;

		private bool waitingDoobersToClose;

		private bool prestigeFull;

		private bool beingPrestiged;

		private global::Kampai.Game.AwardLevelSignal awardLevelSignal;

		private global::Kampai.Game.CloseDownOrderBoardUISignal closeDownOrderBoardUISignal;

		private readonly global::Kampai.Game.AdPlacementName adPlacementName = global::Kampai.Game.AdPlacementName.ORDERBOARD;

		private global::Kampai.Game.AdPlacementInstance adPlacementInstance;

		private bool alreadyClosing;

		private global::Kampai.UI.View.ModalSettings modalSettings = new global::Kampai.UI.View.ModalSettings();

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.OrderBoardTicketClickedSignal ticketClicked { get; set; }

		[Inject]
		public global::Kampai.UI.View.OrderBoardTicketDeletedSignal ticketDeletedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OrderBoardPrestigeSlotFullSignal prestigeSlotFullSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardFillOrderSignal fillOrderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardDeleteOrderSignal deleteOrderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardRefillTicketSignal refillTicketSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RushDialogConfirmationSignal dialogConfirmedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardTransactionFailedSignal transactionFailedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDReminderSignal showHUDReminderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService characterService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OrderBoardStartFillingPrestigeBarSignal startFillingPrestigeBarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardFillOrderCompleteSignal fillOrderCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ResetDoubleTapSignal resetDoubleTapSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetFTUETextSignal setFTUETextSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.DoobersFlownSignal doobersFlownSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal moveAudioListener { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displayPlayerTrainingSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.GetBuffStateSignal getBuffStateSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.GoToResourceButtonClickedSignal gotoResourceBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IGoToService goToService { get; set; }

		[Inject]
		public global::Kampai.Game.IRewardedAdService rewardedAdService { get; set; }

		[Inject]
		public global::Kampai.Game.RewardedAdRewardSignal rewardedAdRewardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AdPlacementActivityStateChangedSignal adPlacementActivityStateChangedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.CloseButton.ClickedSignal.AddListener(Close);
			base.view.FillOrderButton.ClickedSignal.AddListener(FillOrder);
			base.view.DeleteButton.ClickedSignal.AddListener(DeleteTicket);
			base.view.AdVideoButton.ClickedSignal.AddListener(AdVideo);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			ticketClicked.AddListener(TicketClicked);
			dialogConfirmedSignal.AddListener(ConfirmClicked);
			refillTicketSignal.AddListener(RefillTicket);
			transactionFailedSignal.AddListener(TransactionFailed);
			fillOrderCompleteSignal.AddListener(FillOrderComplete);
			resetDoubleTapSignal.AddListener(ResetDoubleTap);
			setFTUETextSignal.AddListener(SetFTUEText);
			doobersFlownSignal.AddListener(DoobersFlown);
			awardLevelSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.AwardLevelSignal>();
			awardLevelSignal.AddListener(AwardLevel);
			closeDownOrderBoardUISignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.CloseDownOrderBoardUISignal>();
			closeDownOrderBoardUISignal.AddListener(SpecialCloseDownForPrePartyPause);
			gotoResourceBuildingSignal.AddListener(GotoResourceBuilding);
			rewardedAdRewardSignal.AddListener(OnRewardedAdReward);
			adPlacementActivityStateChangedSignal.AddListener(OnAdPlacementActivityStateChanged);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.uiRemovedSignal.Dispatch(base.view.gameObject);
			ticketClicked.RemoveListener(TicketClicked);
			refillTicketSignal.RemoveListener(RefillTicket);
			base.view.CloseButton.ClickedSignal.RemoveListener(Close);
			base.view.FillOrderButton.ClickedSignal.RemoveListener(FillOrder);
			base.view.DeleteButton.ClickedSignal.RemoveListener(DeleteTicket);
			base.view.AdVideoButton.ClickedSignal.RemoveListener(AdVideo);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			dialogConfirmedSignal.RemoveListener(ConfirmClicked);
			transactionFailedSignal.RemoveListener(TransactionFailed);
			fillOrderCompleteSignal.RemoveListener(FillOrderComplete);
			resetDoubleTapSignal.RemoveListener(ResetDoubleTap);
			setFTUETextSignal.RemoveListener(SetFTUEText);
			doobersFlownSignal.RemoveListener(DoobersFlown);
			awardLevelSignal.RemoveListener(AwardLevel);
			closeDownOrderBoardUISignal.RemoveListener(SpecialCloseDownForPrePartyPause);
			gotoResourceBuildingSignal.RemoveListener(GotoResourceBuilding);
			rewardedAdRewardSignal.RemoveListener(OnRewardedAdReward);
			adPlacementActivityStateChangedSignal.RemoveListener(OnAdPlacementActivityStateChanged);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			base.view.SetDeleteButtonEnabled(!args.Contains<global::Kampai.UI.DisableDeleteOrderButton>());
			global::Kampai.Game.OrderBoard orderBoard = args.Get<global::Kampai.Game.OrderBoard>();
			modalSettings.enableTicketThrob = args.Contains<global::Kampai.UI.ThrobTicketButton>();
			base.view.modalSettings = modalSettings;
			base.view.Init(args.Get<OrderBoardBuildingTicketsView>(), positionService, guiService, orderBoard.Definition.TicketRepopTime, localService, false);
			soundFXSignal.Dispatch("Play_menu_popUp_01");
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.OrderBoardClearTicketOnBoardSignal>().Dispatch();
			LoadTicketsFromBuilding(orderBoard);
			if (modalSettings.enableTicketThrob)
			{
				setFTUETextSignal.Dispatch("ftue_q6_order");
				HideButtons(true);
			}
			if (playerService.GetMinionPartyInstance().IsPartyReady)
			{
				Close();
			}
			else
			{
				UpdateAdButton();
			}
		}

		internal void AwardLevel(global::Kampai.Game.Transaction.TransactionDefinition td)
		{
			Close();
		}

		internal void DoobersFlown()
		{
			if (waitingDoobersToClose)
			{
				Close();
			}
		}

		internal void FillOrderComplete(int ticketIndex)
		{
			if (currentSelectedPrestige != null && !prestigeFull)
			{
				displayPlayerTrainingSignal.Dispatch(currentSelectedPrestige.Definition.PlayerTrainingNonPrestigeDefinitionId, false, new global::strange.extensions.signal.impl.Signal<bool>());
			}
			currentFulfilledTicket = -1;
			currentSelectedTickedIndex = ticketIndex;
			foreach (global::Kampai.Game.OrderBoardTicket ticket in building.tickets)
			{
				if (ticket.BoardIndex == currentSelectedTickedIndex)
				{
					AddTicket(ticket, false);
				}
				CheckSingleTicketRequirementMatchingState(ticket);
			}
			SetTicketClicks(true);
		}

		internal void ConfirmClicked()
		{
			FillOrder();
		}

		internal void TransactionFailed(global::Kampai.Game.Transaction.TransactionDefinition td)
		{
			if (td == currentTransactionDef)
			{
				SetDeleteOrderButton(true);
				base.view.SetFillOrderButtonState(global::Kampai.UI.View.OrderBoardButtonState.Enable);
				currentFulfilledTicket = -1;
			}
		}

		internal void CheckTicketRequirementMatchingState()
		{
			foreach (global::Kampai.Game.OrderBoardTicket ticket in building.tickets)
			{
				CheckSingleTicketRequirementMatchingState(ticket);
			}
		}

		internal void CheckSingleTicketRequirementMatchingState(global::Kampai.Game.OrderBoardTicket ticket)
		{
			global::Kampai.Game.Transaction.TransactionInstance transactionInst = ticket.TransactionInst;
			bool ticketCheckmark = true;
			int count = transactionInst.Inputs.Count;
			for (int i = 0; i < count; i++)
			{
				global::Kampai.Util.QuantityItem quantityItem = transactionInst.Inputs[i];
				uint quantity = quantityItem.Quantity;
				uint quantityByDefinitionId = playerService.GetQuantityByDefinitionId(quantityItem.ID);
				if (quantity > quantityByDefinitionId)
				{
					ticketCheckmark = false;
				}
			}
			base.view.TicketSlots[ticket.BoardIndex].SetTicketCheckmark(ticketCheckmark);
		}

		internal void DeleteTicket()
		{
			resetDoubleTapSignal.Dispatch(-1);
			SetDeleteOrderButton(false);
			base.view.SetFillOrderButtonState(global::Kampai.UI.View.OrderBoardButtonState.Disable);
			soundFXSignal.Dispatch("Play_delete_ticket_01");
			deleteOrderSignal.Dispatch(currentSelectedTickedIndex, currentTransactionDef, building);
			ticketDeletedSignal.Dispatch();
		}

		internal void AdVideo()
		{
			if (adPlacementInstance != null)
			{
				rewardedAdService.ShowRewardedVideo(adPlacementInstance);
			}
		}

		protected override void Close()
		{
			Close(ZoomOut, null);
		}

		private void ZoomOut(global::System.Action onComplete)
		{
			global::Kampai.Game.BuildingZoomSettings type = new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.OUT, global::Kampai.Game.BuildingZoomType.ORDERBOARD, onComplete);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(type);
		}

		private void Close(global::System.Action<global::System.Action> moveAwayAction, global::System.Action onComplete)
		{
			if (!alreadyClosing)
			{
				alreadyClosing = true;
				if (playerService.GetHighestFtueCompleted() == 999999)
				{
					removeWayFinderSignal.Dispatch(309);
				}
				if (building != null)
				{
					building.MenuOpened = false;
				}
				else
				{
					OnMenuClose();
				}
				moveAudioListener.Dispatch(true, null);
				soundFXSignal.Dispatch("Play_menu_disappear_01");
				base.view.Close();
				moveAwayAction(onComplete);
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.OrderBoardUpdateTicketOnBoardSignal>().Dispatch();
			}
		}

		private void SpecialCloseDownForPrePartyPause()
		{
			soundFXSignal.Dispatch("Play_menu_disappear_01");
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.OrderBoardUpdateTicketOnBoardSignal>().Dispatch();
			if (building != null)
			{
				building.MenuOpened = false;
			}
			base.view.Close();
		}

		private void OnMenuClose()
		{
			showHUDReminderSignal.Dispatch(true);
			moveAudioListener.Dispatch(true, null);
			base.view.DestoryTickets();
			hideSkrim.Dispatch("OrderBoardSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_OrderBoard");
		}

		internal void FillOrder()
		{
			resetDoubleTapSignal.Dispatch(-1);
			if (base.view.FillOrderButton.isDoubleConfirmed())
			{
				if (currentTransactionDef != null)
				{
					currentFulfilledTicket = currentSelectedTickedIndex;
					SetDeleteOrderButton(false);
					if (base.view.FillOrderButton.GetLastFillOrderButtonState() == global::Kampai.UI.View.OrderBoardButtonState.Rush)
					{
						int lastRushCost = base.view.FillOrderButton.GetLastRushCost();
						base.view.SetFillOrderButtonState(global::Kampai.UI.View.OrderBoardButtonState.Disable);
						playerService.ProcessOrderFill(lastRushCost, currentMissingItems, true, RushTransactionCallback);
					}
					else
					{
						base.view.SetFillOrderButtonState(global::Kampai.UI.View.OrderBoardButtonState.Disable);
						CheckIfItIsPrestigeTicketBeforeFillingOrder();
					}
				}
				else
				{
					logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Trying to start an empty black market transaction");
				}
			}
			else
			{
				soundFXSignal.Dispatch("Play_button_click_01");
			}
		}

		private void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				setPremiumCurrencySignal.Dispatch();
				CheckIfItIsPrestigeTicketBeforeFillingOrder();
			}
		}

		private void CheckIfItIsPrestigeTicketBeforeFillingOrder()
		{
			SetTicketClicks(false);
			if (currentSelectedPrestige != null)
			{
				int currentPrestigePoints = currentSelectedPrestige.CurrentPrestigePoints;
				int num = currentPrestigePoints + global::Kampai.Game.Transaction.TransactionUtil.ExtractQuantityFromTransaction(currentTransactionDef, 2);
				int neededPrestigePoints = currentSelectedPrestige.NeededPrestigePoints;
				if (num >= neededPrestigePoints)
				{
					base.view.SetFillOrderButtonState(global::Kampai.UI.View.OrderBoardButtonState.Disable);
					prestigeFull = true;
					num = neededPrestigePoints;
				}
				beingPrestiged = true;
				startFillingPrestigeBarSignal.Dispatch(num, FillOrderAfterBarIsFilled);
			}
			else
			{
				fillOrderSignal.Dispatch(currentSelectedTickedIndex, currentTransactionDef, building);
			}
		}

		private void SetTicketClicks(bool enabled)
		{
			base.view.SetTicketClicks(enabled);
		}

		private void FillOrderAfterBarIsFilled()
		{
			if (prestigeFull)
			{
				waitingDoobersToClose = true;
			}
			beingPrestiged = false;
			fillOrderSignal.Dispatch(currentSelectedTickedIndex, currentTransactionDef, building);
		}

		internal void GetToNextAvailableTicket(bool mute)
		{
			if (building.tickets.Count != 0 && !modalSettings.enableTicketThrob)
			{
				global::Kampai.UI.View.OrderBoardTicketView firstClickableTicketIndex = base.view.GetFirstClickableTicketIndex();
				if (firstClickableTicketIndex.IsCounting())
				{
					SetDeleteOrderButton(false);
					base.view.SetFillOrderButtonState(global::Kampai.UI.View.OrderBoardButtonState.Disable);
					ticketDeletedSignal.Dispatch();
				}
				else
				{
					ticketClicked.Dispatch(firstClickableTicketIndex.ticketInstance, firstClickableTicketIndex.Title, mute);
				}
			}
		}

		internal void RefillTicket(int negativeIndex)
		{
			StartCoroutine(GetNewTicket(-negativeIndex));
		}

		private global::System.Collections.IEnumerator GetNewTicket(int index)
		{
			yield return null;
			if (!(base.view != null))
			{
				yield break;
			}
			foreach (global::Kampai.Game.OrderBoardTicket ticket in building.tickets)
			{
				if (ticket.BoardIndex == index)
				{
					AddTicket(ticket, false);
					CheckSingleTicketRequirementMatchingState(ticket);
				}
			}
		}

		internal void TicketClicked(global::Kampai.Game.OrderBoardTicket ticketInstance, string title, bool mute)
		{
			resetDoubleTapSignal.Dispatch(-1);
			global::Kampai.Game.Transaction.TransactionInstance transactionInst = ticketInstance.TransactionInst;
			if (!mute)
			{
				soundFXSignal.Dispatch("Play_button_click_01");
			}
			base.view.TicketSlots[currentSelectedTickedIndex].SetTicketSelected(false);
			currentSelectedTickedIndex = ticketInstance.BoardIndex;
			base.view.TicketSlots[currentSelectedTickedIndex].SetTicketSelected(true);
			currentTransactionDef = transactionInst.ToDefinition();
			SetDeleteOrderButton(true);
			bool flag = false;
			int characterDefinitionId = ticketInstance.CharacterDefinitionId;
			if (characterDefinitionId != 0)
			{
				currentSelectedPrestige = characterService.GetPrestige(characterDefinitionId);
				if (currentSelectedPrestige == null)
				{
					logger.Error("You have a prestige ticket that doesn't have a prestige instance: {0}", characterDefinitionId);
					return;
				}
				global::Kampai.Game.PrestigeType type = currentSelectedPrestige.Definition.Type;
				if ((type == global::Kampai.Game.PrestigeType.Minion && characterService.IsTikiBarFull()) || (type == global::Kampai.Game.PrestigeType.Villain && characterService.GetEmptyCabana() == null))
				{
					flag = true;
					prestigeSlotFullSignal.Dispatch((type != global::Kampai.Game.PrestigeType.Minion) ? "VillainSlotFull" : "MinionSlotFull");
				}
			}
			else
			{
				currentSelectedPrestige = null;
			}
			if (currentFulfilledTicket == -1)
			{
				global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
				if (minionPartyInstance.IsPartyReady)
				{
					base.view.SetFillOrderButtonState(global::Kampai.UI.View.OrderBoardButtonState.Disable);
				}
				else
				{
					currentMissingItems = playerService.GetMissingItemListFromTransaction(currentTransactionDef);
					if (currentMissingItems.Count == 0)
					{
						base.view.SetFillOrderButtonState(waitingDoobersToClose ? global::Kampai.UI.View.OrderBoardButtonState.Disable : global::Kampai.UI.View.OrderBoardButtonState.MeetRequirement);
					}
					else
					{
						int rushCost = playerService.CalculateRushCost(currentMissingItems);
						base.view.SetFillOrderButtonState(waitingDoobersToClose ? global::Kampai.UI.View.OrderBoardButtonState.Disable : global::Kampai.UI.View.OrderBoardButtonState.Rush, rushCost);
					}
				}
			}
			if (!base.view.DeleteButton.gameObject.activeSelf)
			{
				base.view.DeleteButton.gameObject.SetActive(true);
			}
			if (flag)
			{
				base.view.SetFillOrderButtonState(global::Kampai.UI.View.OrderBoardButtonState.Hide);
			}
			UpdateAdButton();
		}

		internal void SetDeleteOrderButton(bool active)
		{
			base.view.SetupDeleteOrderButton(active);
		}

		internal void LoadTicketsFromBuilding(global::Kampai.Game.OrderBoard building)
		{
			this.building = building;
			foreach (global::Kampai.Game.OrderBoardTicket ticket in building.tickets)
			{
				AddTicket(ticket, ticket.StartTime >= 0, true);
			}
			CheckTicketRequirementMatchingState();
			GetToNextAvailableTicket(true);
		}

		internal void AddTicket(global::Kampai.Game.OrderBoardTicket ticket, bool isInProgress, bool isInit = false)
		{
			string empty = string.Empty;
			if (ticket.CharacterDefinitionId == 0)
			{
				empty = building.Definition.OrderNames[ticket.OrderNameTableIndex];
			}
			else
			{
				global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(ticket.CharacterDefinitionId);
				empty = prestigeDefinition.LocalizedKey;
			}
			string locText = localService.GetString(empty);
			int eventDuration = timeEventService.GetEventDuration(-ticket.BoardIndex);
			base.view.AddTicket(ticket, isInProgress, eventDuration, locText, characterService, isInit, getBuffStateSignal, (!beingPrestiged) ? ticketClicked : null, playerService);
		}

		private void HideButtons(bool hide)
		{
			if (hide)
			{
				base.view.FillOrderButton.gameObject.SetActive(false);
				base.view.DeleteButton.gameObject.SetActive(false);
			}
			else
			{
				base.view.FillOrderButton.gameObject.SetActive(true);
				base.view.DeleteButton.gameObject.SetActive(true);
			}
		}

		private void ResetDoubleTap(int id)
		{
			base.view.ResetDoubleTap(id);
		}

		private void SetFTUEText(string title)
		{
			base.view.CloseButton.gameObject.SetActive(false);
		}

		private void GotoResourceBuilding(int itemDefinitionId)
		{
			int buildingDefintionIDFromItemDefintionID = definitionService.GetBuildingDefintionIDFromItemDefintionID(itemDefinitionId);
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.Building>(buildingDefintionIDFromItemDefintionID);
			if (byDefinitionId.Count > 0)
			{
				global::Kampai.Game.Building suitableBuilding = global::Kampai.Util.GotoBuildingHelpers.GetSuitableBuilding(byDefinitionId);
				if (suitableBuilding.State != global::Kampai.Game.BuildingState.Inventory)
				{
					Close(MoveToBuildingAction(itemDefinitionId), null);
					return;
				}
			}
			goToService.GoToBuildingFromItem(itemDefinitionId);
		}

		private global::System.Action<global::System.Action> MoveToBuildingAction(int itemDefID)
		{
			return delegate
			{
				goToService.GoToBuildingFromItem(itemDefID);
			};
		}

		private void OnRewardedAdReward(global::Kampai.Game.AdPlacementInstance placement)
		{
			adPlacementInstance = null;
			playerService.ProcessOrderFill(0, currentMissingItems, true, RushTransactionCallback);
			rewardedAdService.RewardPlayer(null, placement);
			telemetryService.Send_Telemetry_EVT_AD_INTERACTION(placement.Definition.Name, currentMissingItems, placement.RewardPerPeriodCount);
		}

		private void OnAdPlacementActivityStateChanged(global::Kampai.Game.AdPlacementInstance placement, bool enabled)
		{
			UpdateAdButton();
		}

		private void UpdateAdButton()
		{
			bool flag = rewardedAdService.IsPlacementActive(adPlacementName);
			if (!flag)
			{
				logger.Debug("Ads: placement '{0}' for the order board is disabled.", adPlacementName);
			}
			global::Kampai.Game.AdPlacementInstance placementInstance = rewardedAdService.GetPlacementInstance(adPlacementName);
			bool enable = flag && placementInstance != null;
			base.view.EnableRewardedAdRushButton(enable);
			adPlacementInstance = placementInstance;
		}
	}
}
