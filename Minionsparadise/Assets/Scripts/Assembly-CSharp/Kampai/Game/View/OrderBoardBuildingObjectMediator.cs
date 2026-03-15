namespace Kampai.Game.View
{
	public class OrderBoardBuildingObjectMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("OrderBoardBuildingObjectMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.View.OrderBoardBuildingObjectView view { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardRefillTicketSignal refillTicketSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardStartRefillTicketSignal startRefillTicketSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardSetNewTicketSignal setNewTicketSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PostTransactionSignal postTransactionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AwardLevelSignal awardLevelSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IOrderBoardService orderBoardService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardUpdateTicketOnBoardSignal updateTicketOnBoardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardClearTicketOnBoardSignal clearTicketOnBoardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ToggleHitboxSignal toggleHitboxSignal { get; set; }

		public override void OnRegister()
		{
			startRefillTicketSignal.AddListener(StartRefillTicket);
			postTransactionSignal.AddListener(PostTransaction);
			awardLevelSignal.AddListener(AwardLevel);
			refillTicketSignal.AddListener(RefillTicket);
			updateTicketOnBoardSignal.AddListener(UpdateTicketState);
			clearTicketOnBoardSignal.AddListener(ClearTickets);
			toggleHitboxSignal.AddListener(ToggleHitbox);
			StartCoroutine(RefreshTickets(true));
		}

		public override void OnRemove()
		{
			refillTicketSignal.RemoveListener(RefillTicket);
			startRefillTicketSignal.RemoveListener(StartRefillTicket);
			awardLevelSignal.RemoveListener(AwardLevel);
			updateTicketOnBoardSignal.RemoveListener(UpdateTicketState);
			clearTicketOnBoardSignal.RemoveListener(ClearTickets);
			postTransactionSignal.RemoveListener(PostTransaction);
			toggleHitboxSignal.RemoveListener(ToggleHitbox);
		}

		private global::System.Collections.IEnumerator RefreshTickets(bool clearBoard = false)
		{
			yield return null;
			if (clearBoard)
			{
				view.ClearBoard();
			}
			if (!view.orderBoard.MenuOpened || clearBoard)
			{
				UpdateTicketState();
			}
		}

		private void AwardLevel(global::Kampai.Game.Transaction.TransactionDefinition td)
		{
			logger.Debug("Award Level: {0}", td.ID);
			StartCoroutine(RefreshTickets());
		}

		private void ToggleHitbox(global::Kampai.Game.BuildingZoomType zoomBuildingType, bool enable)
		{
			if (zoomBuildingType == global::Kampai.Game.BuildingZoomType.ORDERBOARD)
			{
				view.ToggleHitbox(enable);
			}
		}

		private void PostTransaction(global::Kampai.Game.Transaction.TransactionUpdateData update)
		{
			StartCoroutine(RefreshTickets());
		}

		private void UpdateTicketState()
		{
			orderBoardService.UpdateOrderNumber();
			foreach (global::Kampai.Game.OrderBoardTicket ticket in view.orderBoard.tickets)
			{
				if (ticket.StartTime != -1)
				{
					view.SetTicketState(ticket.BoardIndex, global::Kampai.Game.OrderBoardTicketState.TIMER);
					continue;
				}
				bool flag = false;
				global::Kampai.Game.Transaction.TransactionInstance transactionInst = ticket.TransactionInst;
				int count = transactionInst.Inputs.Count;
				for (int i = 0; i < count; i++)
				{
					global::Kampai.Util.QuantityItem quantityItem = transactionInst.Inputs[i];
					uint quantity = quantityItem.Quantity;
					uint quantityByDefinitionId = playerService.GetQuantityByDefinitionId(quantityItem.ID);
					if (quantity > quantityByDefinitionId)
					{
						flag = true;
					}
				}
				global::Kampai.Game.OrderBoardTicketState orderBoardTicketState = global::Kampai.Game.OrderBoardTicketState.NOT_AVAILABLE;
				if (ticket.CharacterDefinitionId != 0)
				{
					global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(ticket.CharacterDefinitionId);
					orderBoardTicketState = ((prestige == null || prestige.Definition.Type != global::Kampai.Game.PrestigeType.Villain) ? ((!flag) ? global::Kampai.Game.OrderBoardTicketState.PRESTIGE_CHECKED : global::Kampai.Game.OrderBoardTicketState.PRESTIGE_UNCHECKED) : ((!flag) ? global::Kampai.Game.OrderBoardTicketState.VILLAIN_CHECKED : global::Kampai.Game.OrderBoardTicketState.VILLAIN_UNCHECKED));
				}
				else
				{
					orderBoardTicketState = (flag ? global::Kampai.Game.OrderBoardTicketState.UNCHECKED : global::Kampai.Game.OrderBoardTicketState.CHECKED);
				}
				view.SetTicketState(ticket.BoardIndex, orderBoardTicketState);
			}
		}

		private void ClearTickets()
		{
			view.ClearBoard();
			view.orderBoard.MenuOpened = true;
		}

		private void StartRefillTicket(global::Kampai.Util.Tuple<int, int, float> tuple)
		{
			StartCoroutine(RefreshTickets());
		}

		private void RefillTicket(int index)
		{
			setNewTicketSignal.Dispatch(index, false);
			StartCoroutine(RefreshTickets());
		}
	}
}
