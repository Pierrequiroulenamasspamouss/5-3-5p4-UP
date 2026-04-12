namespace Kampai.Game
{
	public class OrderBoardService : global::Kampai.Game.IOrderBoardService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("OrderBoardService") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.OrderBoard board;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService characterService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDialogSignal showDialogSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardUpdateTicketOnBoardSignal updateTicketOnBoardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displaySignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockCharacterModel characterModel { get; set; }

		[PostConstruct]
		public void Initialize()
		{
			board = playerService.GetByInstanceId<global::Kampai.Game.OrderBoard>(309);
		}

		public global::Kampai.Game.OrderBoard GetBoard()
		{
			return board;
		}

		public void ReplaceCharacterTickets(int characterDefinitionID)
		{
			foreach (global::Kampai.Game.OrderBoardTicket ticket in board.tickets)
			{
				if (ticket.CharacterDefinitionId == characterDefinitionID)
				{
					GetNewTicket(ticket.BoardIndex);
				}
			}
			updateTicketOnBoardSignal.Dispatch();
		}

		public int GetLongestIdleOrderDuration()
		{
			int longestIdleTime = 0;
			GetLongestIdleOrder(out longestIdleTime);
			return longestIdleTime;
		}

		public global::Kampai.Game.Transaction.TransactionDefinition GetLongestIdleOrderTransaction()
		{
			int longestIdleTime = 0;
			global::Kampai.Game.OrderBoardTicket longestIdleOrder = GetLongestIdleOrder(out longestIdleTime);
			if (longestIdleOrder != null)
			{
				return longestIdleOrder.TransactionInst.ToDefinition();
			}
			return null;
		}

		private global::Kampai.Game.OrderBoardTicket GetLongestIdleOrder(out int longestIdleTime)
		{
			if (board == null)
			{
				logger.Warning("OrderBoardService is not instantiated yet. Calling Initialize.");
				Initialize();
			}
			longestIdleTime = 0;
			global::Kampai.Game.OrderBoardTicket result = null;
			foreach (global::Kampai.Game.OrderBoardTicket ticket in board.tickets)
			{
				if (ticket.StartTime <= 0)
				{
					int gameTimeDuration = playerDurationService.GetGameTimeDuration(ticket);
					if (gameTimeDuration > longestIdleTime)
					{
						longestIdleTime = gameTimeDuration;
						result = ticket;
					}
				}
			}
			return result;
		}

		public void GetNewTicket(int orderBoardIndex)
		{
			if (board == null)
			{
				return;
			}
			bool flag = false;
			global::System.Collections.Generic.IList<string> list = new global::System.Collections.Generic.List<string>(board.Definition.OrderNames);
			global::System.Collections.Generic.IList<string> list2 = new global::System.Collections.Generic.List<string>();
			global::Kampai.Game.OrderBoardTicket orderBoardTicket = null;
			foreach (global::Kampai.Game.OrderBoardTicket ticket in board.tickets)
			{
				if (ticket.BoardIndex != orderBoardIndex)
				{
					list2.Add(list[ticket.OrderNameTableIndex]);
					continue;
				}
				orderBoardTicket = ticket;
				flag = true;
			}
			foreach (string item2 in list2)
			{
				list.Remove(item2);
			}
			if (!flag)
			{
				orderBoardTicket = new global::Kampai.Game.OrderBoardTicket();
			}
			orderBoardTicket.StartTime = -1;
			orderBoardTicket.BoardIndex = orderBoardIndex;
			orderBoardTicket.TransactionInst = CreateNewOrder(orderBoardIndex);
			orderBoardTicket.StartGameTime = playerDurationService.TotalGamePlaySeconds;
			CheckIfTransactionContainsPartyFavor(orderBoardTicket.TransactionInst);
			if (orderBoardTicket.TransactionInst.Inputs.Count != 0)
			{
				if (!flag)
				{
					board.tickets.Add(orderBoardTicket);
				}
				if (!GetIsCharacterOrder(orderBoardTicket))
				{
					int index = RollDice(0, list.Count);
					string item = list[index];
					orderBoardTicket.OrderNameTableIndex = board.Definition.OrderNames.IndexOf(item);
					orderBoardTicket.CharacterDefinitionId = 0;
				}
			}
		}

		private bool GetIsCharacterOrder(global::Kampai.Game.OrderBoardTicket ticket)
		{
			bool result = false;
			int currentTicketXP = GetCurrentTicketXP(ticket);
			global::Kampai.Game.Prestige prestige = characterService.GetPrestige(ticket.CharacterDefinitionId);
			if (prestige != null && prestige.state != global::Kampai.Game.PrestigeState.InQueue && !characterService.IsPrestigeFulfilled(prestige))
			{
				for (int num = board.PriorityPrestigeDefinitionIDs.Count - 1; num >= 0; num--)
				{
					if (board.PriorityPrestigeDefinitionIDs[num] == ticket.CharacterDefinitionId)
					{
						board.PriorityPrestigeDefinitionIDs.RemoveAt(num);
					}
				}
				result = true;
			}
			else if (board.PriorityPrestigeDefinitionIDs.Count > 0)
			{
				ticket.CharacterDefinitionId = board.PriorityPrestigeDefinitionIDs[0];
				board.PriorityPrestigeDefinitionIDs.RemoveAt(0);
				CheckAndUpdatePriorityPrestigeCharacterXP(ticket, currentTicketXP);
				result = true;
			}
			else if (ShouldBeCharacterOrder())
			{
				int num2 = PickCharacterDefinitionId(currentTicketXP);
				if (num2 != 0)
				{
					ticket.CharacterDefinitionId = num2;
					result = true;
				}
			}
			return result;
		}

		private void CheckIfTransactionContainsPartyFavor(global::Kampai.Game.Transaction.TransactionInstance trasaction)
		{
			if (trasaction == null)
			{
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.PartyFavorAnimationDefinition> all = definitionService.GetAll<global::Kampai.Game.PartyFavorAnimationDefinition>();
			if (all == null || all.Count == 0)
			{
				return;
			}
			foreach (global::Kampai.Util.QuantityItem input in trasaction.Inputs)
			{
				foreach (global::Kampai.Game.PartyFavorAnimationDefinition item in all)
				{
					if (input.ID == item.ItemID)
					{
						trasaction.Outputs.Add(new global::Kampai.Util.QuantityItem(item.UnlockId, 1u));
					}
				}
			}
		}

		public void AddPriorityPrestigeCharacter(int prestigeDefinitionID)
		{
			if (board.PriorityPrestigeDefinitionIDs.Contains(prestigeDefinitionID))
			{
				return;
			}
			global::Kampai.Game.Prestige prestige = characterService.GetPrestige(prestigeDefinitionID);
			if (prestigeDefinitionID != 40003 || prestige.CurrentPrestigeLevel >= 1)
			{
				board.PriorityPrestigeDefinitionIDs.Add(prestigeDefinitionID);
				global::Kampai.Game.PrestigeDefinition definition = prestige.Definition;
				global::Kampai.Game.QuestDialogSetting questDialogSetting = new global::Kampai.Game.QuestDialogSetting();
				questDialogSetting.type = global::Kampai.UI.View.QuestDialogType.NEWPRESTIGE;
				questDialogSetting.additionalStringParameter = definition.LocalizedKey;
				if (prestige.CurrentPrestigeLevel >= 1)
				{
					displaySignal.Dispatch(19000022, false, new global::strange.extensions.signal.impl.Signal<bool>());
				}
				global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
				if (minionPartyInstance.IsPartyReady || minionPartyInstance.IsPartyHappening)
				{
					characterModel.dialogQueue.Add(questDialogSetting);
				}
				else
				{
					showDialogSignal.Dispatch("AlertPrePrestige", questDialogSetting, new global::Kampai.Util.Tuple<int, int>(0, 0));
				}
			}
		}

		public void SetEnabled(bool b)
		{
			GetBoard().menuEnabled = b;
		}

		private void CheckAndUpdatePriorityPrestigeCharacterXP(global::Kampai.Game.OrderBoardTicket ticket, int currentTicketXP)
		{
			global::Kampai.Game.Prestige prestige = characterService.GetPrestige(ticket.CharacterDefinitionId);
			if (prestige == null)
			{
				return;
			}
			int neededPrestigePoints = prestige.NeededPrestigePoints;
			if (currentTicketXP < neededPrestigePoints || prestige.state != global::Kampai.Game.PrestigeState.PreUnlocked)
			{
				return;
			}
			int num = RollDice(1, neededPrestigePoints);
			float num2 = (float)num / (float)currentTicketXP;
			global::Kampai.Game.Transaction.TransactionInstance transactionInst = ticket.TransactionInst;
			uint num3 = 0u;
			foreach (global::Kampai.Util.QuantityItem input in transactionInst.Inputs)
			{
				num3 = (uint)((float)input.Quantity * num2);
				num3 = ((num3 == 0) ? 1u : num3);
				input.Quantity = num3;
			}
			foreach (global::Kampai.Util.QuantityItem output in transactionInst.Outputs)
			{
				num3 = (uint)((float)output.Quantity * num2);
				num3 = ((num3 == 0) ? 1u : num3);
				output.Quantity = num3;
			}
		}

		private bool ShouldBeCharacterOrder()
		{
			int num = RollDice(0, 100);
			if (num < board.Definition.CharacterOrderChance)
			{
				return true;
			}
			return false;
		}

		private int PickCharacterDefinitionId(int currentTicketXP)
		{
			global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.CharacterTicketData> dictionary = UpdateCharacterTicketDataOnOrderBoard();
			global::Kampai.Game.Transaction.WeightedDefinition weightedDefinition = new global::Kampai.Game.Transaction.WeightedDefinition();
			weightedDefinition.Entities = new global::System.Collections.Generic.List<global::Kampai.Game.Transaction.WeightedQuantityItem>();
			global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.Prestige> allUnlockedPrestiges = characterService.GetAllUnlockedPrestiges();
			bool flag = characterService.IsTikiBarFull();
			bool flag2 = characterService.GetEmptyCabana() == null;
			if (flag && flag2)
			{
				return 0;
			}
			foreach (global::System.Collections.Generic.KeyValuePair<int, global::Kampai.Game.Prestige> item in allUnlockedPrestiges)
			{
				global::Kampai.Game.Prestige value = item.Value;
				global::Kampai.Game.PrestigeDefinition definition = value.Definition;
				if ((!flag || definition.Type != global::Kampai.Game.PrestigeType.Minion) && (!flag2 || definition.Type != global::Kampai.Game.PrestigeType.Villain))
				{
					int num = 0;
					int num2 = 0;
					int iD = definition.ID;
					if (dictionary.ContainsKey(iD))
					{
						num = dictionary[iD].OnBoardCount;
						num2 = dictionary[iD].XPAmount;
					}
					if (num < definition.MaxedBadgedOrder && (value.state != global::Kampai.Game.PrestigeState.PreUnlocked || num < 1) && (value.state == global::Kampai.Game.PrestigeState.PreUnlocked || value.state == global::Kampai.Game.PrestigeState.Prestige) && num2 + value.CurrentPrestigePoints <= value.NeededPrestigePoints && (value.state != global::Kampai.Game.PrestigeState.PreUnlocked || value.CurrentPrestigePoints + currentTicketXP + num2 <= value.NeededPrestigePoints))
					{
						weightedDefinition.Entities.Add(new global::Kampai.Game.Transaction.WeightedQuantityItem(iD, 0u, definition.OrderBoardWeight));
					}
				}
			}
			if (weightedDefinition.Entities.Count == 0)
			{
				return 0;
			}
			global::Kampai.Game.Transaction.WeightedInstance weightedInstance = new global::Kampai.Game.Transaction.WeightedInstance(weightedDefinition);
			return weightedInstance.NextPick(randomService).ID;
		}

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.CharacterTicketData> UpdateCharacterTicketDataOnOrderBoard()
		{
			global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.CharacterTicketData> dictionary = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.CharacterTicketData>();
			foreach (global::Kampai.Game.OrderBoardTicket ticket in board.tickets)
			{
				int characterDefinitionId = ticket.CharacterDefinitionId;
				if (characterDefinitionId != 0)
				{
					if (!dictionary.ContainsKey(characterDefinitionId))
					{
						global::Kampai.Game.CharacterTicketData characterTicketData = new global::Kampai.Game.CharacterTicketData();
						characterTicketData.OnBoardCount = 1;
						characterTicketData.XPAmount = GetCurrentTicketXP(ticket);
						dictionary.Add(characterDefinitionId, characterTicketData);
					}
					else
					{
						dictionary[characterDefinitionId].OnBoardCount++;
						dictionary[characterDefinitionId].XPAmount += global::Kampai.Game.Transaction.TransactionUtil.ExtractQuantityFromTransaction(ticket.TransactionInst, 2);
					}
				}
			}
			return dictionary;
		}

		public void UpdateOrderNumber()
		{
			if (playerService.GetUnlockedDefsByType<global::Kampai.Game.IngredientsItemDefinition>().Count == 0)
			{
				return;
			}
			int num = 0;
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			foreach (global::Kampai.Game.BlackMarketBoardUnlockedOrderSlotDefinition unlockTicketSlot in board.Definition.UnlockTicketSlots)
			{
				if (playerService.GetHighestFtueCompleted() < 999999)
				{
					num = 1;
				}
				else if (quantity >= unlockTicketSlot.UnlockLevel)
				{
					num = unlockTicketSlot.OrderSlots;
				}
			}
			global::Kampai.Game.StuartCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StuartCharacter>(70001);
			if (firstInstanceByDefinitionId == null)
			{
				num = 1;
			}
			int count = board.tickets.Count;
			num = ((count <= num) ? num : count);
			for (int i = count; i < num; i++)
			{
				GetNewTicket(i);
			}
		}

		private global::Kampai.Game.Transaction.TransactionInstance CreateNewOrder(int orderBoardIndex)
		{
			global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem> uniqueIngredients = GetUniqueIngredients(orderBoardIndex);
			global::Kampai.Game.BlackMarketBoardSlotDefinition currentSlotValues = new global::Kampai.Game.BlackMarketBoardSlotDefinition();
			foreach (global::Kampai.Game.BlackMarketBoardSlotDefinition minMaxIngredient in board.Definition.MinMaxIngredients)
			{
				if (minMaxIngredient.SlotIndex == orderBoardIndex + 1)
				{
					currentSlotValues = minMaxIngredient;
				}
			}
			SetIngredientsQty(uniqueIngredients, currentSlotValues);
			return CreateOrderBoardTransactionBasedOnQuantityList(uniqueIngredients);
		}

		private int GetCurrentTicketXP(global::Kampai.Game.OrderBoardTicket targetTicket)
		{
			return global::Kampai.Game.Transaction.TransactionUtil.ExtractQuantityFromTransaction(targetTicket.TransactionInst, 2);
		}

		private global::Kampai.Game.Transaction.TransactionInstance CreateOrderBoardTransactionBasedOnQuantityList(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> qList)
		{
			global::Kampai.Game.Transaction.TransactionInstance transactionInstance = new global::Kampai.Game.Transaction.TransactionInstance();
			transactionInstance.Inputs = qList;
			int num = 0;
			float num2 = 0f;
			float num3 = 1f;
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			foreach (global::Kampai.Game.BlackMarketBoardMultiplierDefinition item in board.Definition.LevelBandXP)
			{
				if (quantity >= item.Level)
				{
					num3 = item.Multiplier;
				}
			}
			foreach (global::Kampai.Util.QuantityItem q in qList)
			{
				global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(q.ID);
				num += ingredientsItemDefinition.BaseGrindCost * (int)q.Quantity;
				num2 += (float)(ingredientsItemDefinition.BaseXP * (int)q.Quantity) * num3;
			}
			uint quantity2 = (uint)global::System.Math.Round(num2);
			transactionInstance.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			transactionInstance.Outputs.Add(new global::Kampai.Util.QuantityItem(0, (uint)num));
			transactionInstance.Outputs.Add(new global::Kampai.Util.QuantityItem(2, quantity2));
			playerService.AssignNextInstanceId(transactionInstance);
			return transactionInstance;
		}

		private void SetIngredientsQty(global::System.Collections.Generic.IEnumerable<global::Kampai.Util.QuantityItem> qList, global::Kampai.Game.BlackMarketBoardSlotDefinition currentSlotValues)
		{
			foreach (global::Kampai.Util.QuantityItem q in qList)
			{
				int quantity = RollDice(currentSlotValues.MinQuantity, currentSlotValues.MaxQuantity + 1);
				q.Quantity = (uint)quantity;
			}
		}

		private global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem> GetUniqueIngredients(int orderBoardIndex)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> unlockedDefsByType = playerService.GetUnlockedDefsByType<global::Kampai.Game.IngredientsItemDefinition>();
			global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> availableIngredients = AvailableItems(unlockedDefsByType);
			return PickTicketIngredient(availableIngredients, orderBoardIndex);
		}

		private global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> AvailableItems(global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> unlockedItems)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.IngredientsItemDefinition> list = new global::System.Collections.Generic.List<global::Kampai.Game.IngredientsItemDefinition>();
			foreach (global::Kampai.Game.IngredientsItemDefinition unlockedItem in unlockedItems)
			{
				if (!playerService.HasPurchasedBuildingAssociatedWithItem(unlockedItem))
				{
					list.Add(unlockedItem);
				}
			}
			foreach (global::Kampai.Game.IngredientsItemDefinition item in list)
			{
				unlockedItems.Remove(item);
			}
			return unlockedItems;
		}

		private global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem> PickTicketIngredient(global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> availableIngredients, int orderBoardIndex)
		{
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem> list2 = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			foreach (global::Kampai.Game.IngredientsItemDefinition availableIngredient in availableIngredients)
			{
				int tier = availableIngredient.Tier;
				int iD = availableIngredient.ID;
				if (orderBoardIndex < 3 && tier == 0)
				{
					list.Add(iD);
				}
				else if (orderBoardIndex < 6 && orderBoardIndex >= 3 && tier <= 1)
				{
					list.Add(iD);
				}
				else if (orderBoardIndex >= 6 && tier >= 1)
				{
					list.Add(iD);
				}
			}
			int id = ((list.Count >= 1) ? list[RollDice(0, list.Count)] : availableIngredients[RollDice(0, availableIngredients.Count)].ID);
			list2.Add(new global::Kampai.Util.QuantityItem(id, 1u));
			return list2;
		}

		private int RollDice(int minValue, int maxValue)
		{
			return randomService.NextInt(minValue, maxValue);
		}
	}
}
