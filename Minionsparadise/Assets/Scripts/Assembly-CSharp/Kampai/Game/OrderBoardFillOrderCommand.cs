namespace Kampai.Game
{
	public class OrderBoardFillOrderCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.OrderBoard building { get; set; }

		[Inject]
		public int TicketIndex { get; set; }

		[Inject]
		public global::Kampai.Game.Transaction.TransactionDefinition def { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardSetNewTicketSignal setNewTicketSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardTransactionFailedSignal transactionFailedSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal setStorageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displayPlayerTrainingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.GetBuffStateSignal getBuffStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ValidateCurrentTriggerSignal validateCurrentTriggerSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IPartyFavorAnimationService partyFavorAnimationService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService defService { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		public override void Execute()
		{
			int startTime = timeService.CurrentTime();
			getBuffStateSignal.Dispatch(global::Kampai.Game.BuffType.CURRENCY, UpdateTransactionCallback);
			playerService.StartTransaction(def, global::Kampai.Game.TransactionTarget.BLACKMARKETBOARD, TransactionCallback, new global::Kampai.Game.TransactionArg(building.ID), startTime, TicketIndex);
		}

		private void UpdateTransactionCallback(float multiplier)
		{
			foreach (global::Kampai.Util.QuantityItem output in def.Outputs)
			{
				if (output.ID == 0)
				{
					output.Quantity = (uint)global::UnityEngine.Mathf.CeilToInt((float)output.Quantity * multiplier);
					break;
				}
			}
		}

		private void TransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				UpdatePartyFavors();
				soundFXSignal.Dispatch("Play_fill_order_01");
				global::Kampai.Game.OrderBoardTicket orderBoardTicket = building.tickets[TicketIndex];
				int characterDefinitionId = orderBoardTicket.CharacterDefinitionId;
				int partyPointsEarned = 0;
				if (orderBoardTicket.TransactionInst != null && orderBoardTicket.TransactionInst.Outputs != null)
				{
					partyPointsEarned = global::Kampai.Game.Transaction.TransactionUtil.GetXPOutputForTransaction(orderBoardTicket.TransactionInst.ToDefinition());
				}
				telemetryService.Send_Telemetry_EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL(TicketIndex.ToString(), global::Kampai.Common.Service.Telemetry.TelemetryAchievementType.Order, partyPointsEarned, string.Empty);
				telemetryService.Send_TelemetryOrderBoard(true, def, characterDefinitionId);
				GetReward(characterDefinitionId);
				playerService.IncreaseCompletedOrders();
				setNewTicketSignal.Dispatch(-TicketIndex, true);
				validateCurrentTriggerSignal.Dispatch();
			}
			else
			{
				building.HarvestableCharacterDefinitionId = 0;
				transactionFailedSignal.Dispatch(def);
			}
		}

		private void UpdatePartyFavors()
		{
			foreach (global::Kampai.Util.QuantityItem input in def.Inputs)
			{
				int partyFavorDefinitionIDByItemID = defService.GetPartyFavorDefinitionIDByItemID(input.ID);
				if (partyFavorDefinitionIDByItemID != -1)
				{
					partyFavorAnimationService.AddAvailablePartyFavorItem(partyFavorDefinitionIDByItemID);
				}
			}
		}

		private void GetReward(int prestigeDefID)
		{
			global::Kampai.Game.TransactionArg transactionArg = new global::Kampai.Game.TransactionArg(building.ID);
			if (prestigeDefID != 0)
			{
				building.HarvestableCharacterDefinitionId = prestigeDefID;
				global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(prestigeDefID);
				if (prestige != null)
				{
					prestige.CurrentOrdersCompleted++;
					transactionArg.AddAccumulator(prestige);
				}
			}
			if (playerService.FinishTransaction(def, global::Kampai.Game.TransactionTarget.BLACKMARKETBOARD, transactionArg))
			{
				questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.OrderBoard);
				if (prestigeDefID == 0)
				{
					displayPlayerTrainingSignal.Dispatch(19000006, false, new global::strange.extensions.signal.impl.Signal<bool>());
				}
				else
				{
					global::Kampai.Game.Prestige prestige2 = prestigeService.GetPrestige(prestigeDefID);
					if (prestigeService.IsPrestigeFulfilled(prestige2))
					{
						pickControllerModel.SetIgnoreInstance(313, true);
					}
				}
			}
			setStorageSignal.Dispatch();
		}
	}
}
