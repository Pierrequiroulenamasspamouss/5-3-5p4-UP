namespace Kampai.Game
{
	public class ScheduleJobCompleteNotificationsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ScheduleNotificationSignal notificationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IDevicePrefsService notifPrefs { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		public override void Execute()
		{
			if (!global::Kampai.Util.Native.AreNotificationsEnabled())
			{
				return;
			}
			ScheduleMinionNotifications();
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Building>();
			int num = int.MaxValue;
			int num2 = int.MinValue;
			foreach (global::Kampai.Game.Building item in instancesByType)
			{
				int num3 = timeEventService.GetTimeRemaining(item.ID);
				if (num3 <= 0)
				{
					continue;
				}
				if (item.State == global::Kampai.Game.BuildingState.Construction)
				{
					if (num3 < num)
					{
						num = num3;
					}
					continue;
				}
				global::Kampai.Game.CraftingBuilding craftingBuilding = item as global::Kampai.Game.CraftingBuilding;
				if (craftingBuilding == null)
				{
					continue;
				}
				global::System.Collections.Generic.IList<int> recipeInQueue = craftingBuilding.RecipeInQueue;
				if (recipeInQueue.Count > 1)
				{
					for (int i = 1; i < recipeInQueue.Count; i++)
					{
						global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(recipeInQueue[i]);
						if (ingredientsItemDefinition != null)
						{
							num3 += IngredientsItemUtil.GetHarvestTimeFromIngredientDefinition(ingredientsItemDefinition, definitionService);
						}
					}
				}
				if (num3 > num2)
				{
					num2 = num3;
				}
			}
			if (num2 > int.MinValue)
			{
				ScheduleCraftingNotification(num2);
			}
			if (num < int.MaxValue)
			{
				ScheduleConstructionNotification(num);
			}
			ScheduleBlackMarketNotifications();
			ScheduleMarketPlaceNotifications();
		}

		private int CalculateFirstNewFulfillableOrder()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Transaction.TransactionDefinition> unfillableOrders = new global::System.Collections.Generic.List<global::Kampai.Game.Transaction.TransactionDefinition>();
			global::System.Collections.Generic.Dictionary<int, uint> possibleInventory = new global::System.Collections.Generic.Dictionary<int, uint>();
			global::System.Collections.Generic.List<global::Kampai.Util.Tuple<int, int>> list = new global::System.Collections.Generic.List<global::Kampai.Util.Tuple<int, int>>();
			GetUnfillableOrders(unfillableOrders, possibleInventory);
			GetAllHarvestingBuildings(list);
			AddItemsFromCraftingBuildings(list);
			list.Sort((global::Kampai.Util.Tuple<int, int> a, global::Kampai.Util.Tuple<int, int> b) => a.Item1.CompareTo(b.Item1));
			return FindFulfillableOrder(unfillableOrders, possibleInventory, list);
		}

		private void ScheduleMinionNotifications()
		{
			if (!notifPrefs.GetDevicePrefs().BaseResourceNotif)
			{
				return;
			}
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Minion> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Minion>();
			int num = -1;
			foreach (global::Kampai.Game.Minion item in instancesByType)
			{
				int timeRemaining = timeEventService.GetTimeRemaining(item.ID);
				if (timeRemaining > num)
				{
					num = timeRemaining;
				}
			}
			if (num > 0)
			{
				global::Kampai.Game.NotificationDefinition notificationDefinition = definitionService.Get(10007) as global::Kampai.Game.NotificationDefinition;
				if (notificationDefinition != null)
				{
					notificationDefinition.Seconds = num;
					notificationSignal.Dispatch(notificationDefinition);
				}
			}
		}

		private void ScheduleCraftingNotification(int maxCraftingTime)
		{
			if (notifPrefs.GetDevicePrefs().CraftingNotif && maxCraftingTime > 0)
			{
				global::Kampai.Game.NotificationDefinition notificationDefinition = definitionService.Get(10011) as global::Kampai.Game.NotificationDefinition;
				if (notificationDefinition != null)
				{
					notificationDefinition.Seconds = maxCraftingTime;
					notificationSignal.Dispatch(notificationDefinition);
				}
			}
		}

		private void ScheduleConstructionNotification(int maxBuildingTime)
		{
			if (notifPrefs.GetDevicePrefs().ConstructionNotif && maxBuildingTime > 0)
			{
				global::Kampai.Game.NotificationDefinition notificationDefinition = definitionService.Get(10008) as global::Kampai.Game.NotificationDefinition;
				if (notificationDefinition != null)
				{
					notificationDefinition.Seconds = maxBuildingTime;
					notificationSignal.Dispatch(notificationDefinition);
				}
			}
		}

		private void ScheduleBlackMarketNotifications()
		{
			if (!notifPrefs.GetDevicePrefs().BlackMarketNotif)
			{
				return;
			}
			int num = -1;
			for (int i = 1; i <= 20; i++)
			{
				int timeRemaining = timeEventService.GetTimeRemaining(-i);
				if (timeRemaining > 0 && (num < 0 || timeRemaining < num))
				{
					num = timeRemaining;
				}
			}
			if (num > 0)
			{
				global::Kampai.Game.NotificationDefinition notificationDefinition = definitionService.Get(10009) as global::Kampai.Game.NotificationDefinition;
				if (notificationDefinition != null)
				{
					notificationDefinition.Seconds = num;
					notificationSignal.Dispatch(notificationDefinition);
				}
			}
			int num2 = CalculateFirstNewFulfillableOrder();
			if (num2 > 0)
			{
				global::Kampai.Game.NotificationDefinition notificationDefinition2 = definitionService.Get(10010) as global::Kampai.Game.NotificationDefinition;
				if (notificationDefinition2 != null)
				{
					notificationDefinition2.Seconds = num2;
					notificationSignal.Dispatch(notificationDefinition2);
				}
			}
		}

		private void ScheduleMarketPlaceNotifications()
		{
			if (!notifPrefs.GetDevicePrefs().MarketPlaceNotif)
			{
				return;
			}
			global::System.Collections.Generic.ICollection<global::Kampai.Game.MarketplaceSaleItem> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MarketplaceSaleItem>();
			if (instancesByType == null)
			{
				return;
			}
			int num = -1;
			int track = -1;
			foreach (global::Kampai.Game.MarketplaceSaleItem item in instancesByType)
			{
				int timeRemaining = timeEventService.GetTimeRemaining(item.ID);
				if (timeRemaining > 0 && (num < 0 || timeRemaining < num))
				{
					num = timeRemaining;
					track = marketplaceService.GetSlotIndex(marketplaceService.GetSlotByItem(item));
				}
			}
			if (num > 0)
			{
				global::Kampai.Game.NotificationDefinition notificationDefinition = definitionService.Get(10013) as global::Kampai.Game.NotificationDefinition;
				if (notificationDefinition != null)
				{
					notificationDefinition.Seconds = num;
					notificationDefinition.Track = track;
					notificationSignal.Dispatch(notificationDefinition);
				}
			}
		}

		private void GetUnfillableOrders(global::System.Collections.Generic.ICollection<global::Kampai.Game.Transaction.TransactionDefinition> UnfillableOrders, global::System.Collections.Generic.IDictionary<int, uint> PossibleInventory)
		{
			foreach (global::Kampai.Game.Building item in playerService.GetInstancesByType<global::Kampai.Game.Building>())
			{
				global::Kampai.Game.OrderBoard orderBoard = item as global::Kampai.Game.OrderBoard;
				if (orderBoard == null || orderBoard.tickets == null)
				{
					continue;
				}
				foreach (global::Kampai.Game.OrderBoardTicket ticket in orderBoard.tickets)
				{
					if (playerService.VerifyTransaction(ticket.TransactionInst.ToDefinition()))
					{
						continue;
					}
					UnfillableOrders.Add(ticket.TransactionInst.ToDefinition());
					foreach (global::Kampai.Util.QuantityItem input in ticket.TransactionInst.Inputs)
					{
						if (!PossibleInventory.ContainsKey(input.ID))
						{
							PossibleInventory.Add(input.ID, playerService.GetQuantityByDefinitionId(input.ID));
						}
					}
				}
			}
		}

		private void GetAllHarvestingBuildings(global::System.Collections.Generic.ICollection<global::Kampai.Util.Tuple<int, int>> FutureTransactions)
		{
			foreach (global::Kampai.Game.Minion item in playerService.GetInstancesByType<global::Kampai.Game.Minion>())
			{
				if (item.State != global::Kampai.Game.MinionState.Tasking || item.BuildingID <= 0)
				{
					continue;
				}
				int timeRemaining = timeEventService.GetTimeRemaining(item.ID);
				if (timeRemaining > 0)
				{
					global::Kampai.Game.TaskableBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TaskableBuilding>(item.BuildingID);
					if (byInstanceId != null)
					{
						FutureTransactions.Add(new global::Kampai.Util.Tuple<int, int>(timeRemaining, byInstanceId.GetTransactionID(definitionService)));
					}
				}
			}
		}

		private void AddItemsFromCraftingBuildings(global::System.Collections.Generic.ICollection<global::Kampai.Util.Tuple<int, int>> FutureTransactions)
		{
			foreach (global::Kampai.Game.Building item in playerService.GetInstancesByType<global::Kampai.Game.Building>())
			{
				global::Kampai.Game.CraftingBuilding craftingBuilding = item as global::Kampai.Game.CraftingBuilding;
				if (craftingBuilding == null)
				{
					continue;
				}
				int num = timeEventService.GetTimeRemaining(craftingBuilding.ID);
				if (num <= 0)
				{
					continue;
				}
				bool flag = true;
				foreach (int item2 in craftingBuilding.RecipeInQueue)
				{
					global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(item2);
					if (!flag)
					{
						num += IngredientsItemUtil.GetHarvestTimeFromIngredientDefinition(ingredientsItemDefinition, definitionService);
					}
					FutureTransactions.Add(new global::Kampai.Util.Tuple<int, int>(num, ingredientsItemDefinition.TransactionId));
					flag = false;
				}
			}
		}

		private int FindFulfillableOrder(global::System.Collections.Generic.ICollection<global::Kampai.Game.Transaction.TransactionDefinition> UnfillableOrders, global::System.Collections.Generic.IDictionary<int, uint> PossibleInventory, global::System.Collections.Generic.ICollection<global::Kampai.Util.Tuple<int, int>> FutureTransactions)
		{
			foreach (global::Kampai.Util.Tuple<int, int> FutureTransaction in FutureTransactions)
			{
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(FutureTransaction.Item2);
				foreach (global::Kampai.Util.QuantityItem output in transactionDefinition.Outputs)
				{
					uint value = 0u;
					PossibleInventory.TryGetValue(output.ID, out value);
					value += output.Quantity;
					PossibleInventory[output.ID] = value;
				}
				foreach (global::Kampai.Game.Transaction.TransactionDefinition UnfillableOrder in UnfillableOrders)
				{
					bool flag = true;
					foreach (global::Kampai.Util.QuantityItem input in UnfillableOrder.Inputs)
					{
						if (PossibleInventory[input.ID] < input.Quantity)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return FutureTransaction.Item1;
					}
				}
			}
			return -1;
		}
	}
}
