namespace Kampai.Tools.AnimationToolKit
{
	public class AnimationToolKitPlayerService : global::Kampai.Game.IPlayerService
	{
		private int nextId = 1000;

		private global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.Instance> byInstanceId;

		protected global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Kampai.Game.Instance>> byDefinitionId;

		public int NextId
		{
			get
			{
				return nextId;
			}
		}

		public long ID
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public global::Kampai.Game.Player LastSave
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public bool PlayerDataIsLoaded
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public int LevelUpUTC
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public int LastGameStartUTC
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public int FirstGameStartUTC
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public int LastPlayedUTC
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public int TimeZoneOffset
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public string SWRVEGroup { get; set; }

		public int GameplaySecondsSinceLevelUp
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public int AccumulatedGameplayDuration
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
			set
			{
				throw new global::System.NotImplementedException();
			}
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.Player.HelpTipTrackingItem> helpTipsTrackingData
		{
			get
			{
				throw new global::System.NotImplementedException();
			}
		}

		public AnimationToolKitPlayerService()
		{
			byInstanceId = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.Instance>();
			byDefinitionId = new global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.List<global::Kampai.Game.Instance>>();
		}

		public uint GetQuantity(global::Kampai.Game.StaticItem def)
		{
			return 0u;
		}

		public uint GetStorageCount()
		{
			throw new global::System.NotImplementedException();
		}

		public uint GetQuantityByDefinitionId(int defId)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessSlotPurchase(int slotCost, bool showStoreOnFail, int slotNumber, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, int instanceId)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessSaleCancel(int cost, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessRefreshMarket(int cost, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessItemPurchase(int itemCost, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, bool byPassStorageCheck = false)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessOrderFill(int slotCost, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessRush(int rushCost, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, int instanceId)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetCountByDefinitionId(int defId)
		{
			throw new global::System.NotImplementedException();
		}

		public uint GetQuantityByInstanceId(int instanceId)
		{
			throw new global::System.NotImplementedException();
		}

		public bool isStorageFull()
		{
			throw new global::System.NotImplementedException();
		}

		public uint GetAvailableStorageCapacity()
		{
			throw new global::System.NotImplementedException();
		}

		public void AlterQuantity(global::Kampai.Game.StaticItem def, int amount)
		{
			throw new global::System.NotImplementedException();
		}

		public void SetQuantity(global::Kampai.Game.StaticItem def, int amount)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetPartyBuffRemainingTime(global::Kampai.Game.MinionParty minionParty)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetPartyBuffDurationTime(global::Kampai.Game.MinionParty minionParty)
		{
			throw new global::System.NotImplementedException();
		}

		public bool IsMinionPartyUnlocked()
		{
			throw new global::System.NotImplementedException();
		}

		public bool IsMinigamePackAccessible()
		{
			throw new global::System.NotImplementedException();
		}

		public bool HasPurchasedMinigamePack()
		{
			throw new global::System.NotImplementedException();
		}

		public void UpdateMinionPartyPointValues()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> GetBuildings()
		{
			global::System.Collections.Generic.LinkedList<global::Kampai.Game.Building> linkedList = new global::System.Collections.Generic.LinkedList<global::Kampai.Game.Building>();
			foreach (global::Kampai.Game.Instance value in byInstanceId.Values)
			{
				if (value.Definition is global::Kampai.Game.BuildingDefinition)
				{
					linkedList.AddLast((global::Kampai.Game.Building)value);
				}
			}
			return linkedList;
		}

		public global::System.Collections.Generic.ICollection<int> GetAnimatingBuildingIDs()
		{
			global::System.Collections.Generic.LinkedList<int> linkedList = new global::System.Collections.Generic.LinkedList<int>();
			foreach (global::Kampai.Game.Instance value in byInstanceId.Values)
			{
				if (value.Definition is global::Kampai.Game.AnimatingBuildingDefinition)
				{
					linkedList.AddLast(((global::Kampai.Game.Building)value).ID);
				}
			}
			return linkedList;
		}

		public global::Kampai.Game.MinionParty GetMinionPartyInstance()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.IngredientsItemDefinition> GetAllUnLockedIngredients()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<T> GetUnlockedDefsByType<T>() where T : global::Kampai.Game.Definition
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.ICollection<global::Kampai.Game.DestructibleBuilding> GetDestructibles()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.ICollection<global::Kampai.Game.Minion> GetMinions()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.ICollection<global::Kampai.Game.Quest> GetQuests()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.Item> GetSellableItems()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.Dictionary<int, int> GetBuildingOnBoardCountMap()
		{
			throw new global::System.NotImplementedException();
		}

		public bool IsMissingItemFromTransaction(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> GetMissingItemListFromTransaction(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.ICollection<global::Kampai.Game.Item> GetStorableItems(out uint totalQuantity)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetUnlockedQuantityOfID(int defId)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetPurchasedQuanityByUpsellID(int defId)
		{
			throw new global::System.NotImplementedException();
		}

		public void AddUpsellToPurchased(int defID)
		{
			throw new global::System.NotImplementedException();
		}

		public void ClearPurchasedUpsells(int defID)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.Player LoadPlayerData(string serialized)
		{
			throw new global::System.NotImplementedException();
		}

		public void Deserialize(string serialized, bool isRetry = false)
		{
			throw new global::System.NotImplementedException();
		}

		public byte[] SavePlayerData(global::Kampai.Game.Player PlayerDataLoaded)
		{
			throw new global::System.NotImplementedException();
		}

		public byte[] Serialize()
		{
			throw new global::System.NotImplementedException();
		}

		public bool IsPlayerInitialized()
		{
			return byInstanceId != null;
		}

		public void Add(global::Kampai.Game.Instance i)
		{
			if (i != null && i.Definition != null)
			{
				AssignNextInstanceId(i);
				byInstanceId.Add(i.ID, i);
				int iD = i.Definition.ID;
				if (!byDefinitionId.ContainsKey(iD))
				{
					byDefinitionId.Add(iD, new global::System.Collections.Generic.List<global::Kampai.Game.Instance>());
				}
				byDefinitionId[iD].Add(i);
			}
		}

		public void Remove(global::Kampai.Game.Instance i)
		{
			int iD = i.ID;
			if (i != null && iD != 0 && byInstanceId.ContainsKey(iD))
			{
				byInstanceId.Remove(iD);
				int iD2 = i.Definition.ID;
				byDefinitionId[iD2].Remove(i);
				if (byDefinitionId[iD2].Count == 0)
				{
					byDefinitionId.Remove(iD2);
				}
			}
		}

		public T GetByInstanceId<T>(int id) where T : class, global::Kampai.Game.Instance
		{
			if (byInstanceId.ContainsKey(id))
			{
				return byInstanceId[id] as T;
			}
			return (T)null;
		}

		public T GetFirstInstanceByDefinitionId<T>(int definitionId) where T : class, global::Kampai.Game.Instance
		{
			if (byDefinitionId.ContainsKey(definitionId) && byDefinitionId[definitionId].Count > 0)
			{
				return (T)byDefinitionId[definitionId][0];
			}
			return (T)null;
		}

		public I GetFirstInstanceByDefintion<I, D>() where I : class, global::Kampai.Game.Instance where D : global::Kampai.Game.Definition
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.ICollection<T> GetByDefinitionId<T>(int id) where T : global::Kampai.Game.Instance
		{
			if (byDefinitionId.ContainsKey(id))
			{
				global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>();
				{
					foreach (global::Kampai.Game.Instance item in byDefinitionId[id])
					{
						if (item is T)
						{
							list.Add((T)item);
						}
					}
					return list;
				}
			}
			return new global::System.Collections.Generic.List<T>();
		}

		public global::Kampai.Game.Transaction.WeightedInstance GetWeightedInstance(int defId, global::Kampai.Game.Transaction.WeightedDefinition wd = null)
		{
			throw new global::System.NotImplementedException();
		}

		public int CalculateRushCost(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items)
		{
			throw new global::System.NotImplementedException();
		}

		public bool FinishTransaction(int transactionId, global::Kampai.Game.TransactionTarget target, global::Kampai.Game.TransactionArg arg = null)
		{
			throw new global::System.NotImplementedException();
		}

		public bool FinishTransaction(int transactionId, global::Kampai.Game.TransactionTarget target, global::Kampai.Game.TransactionArg arg, float bonusDropRate = 0f, float premiumAwardRate = 0f)
		{
			throw new global::System.NotImplementedException();
		}

		public bool FinishTransaction(int transactionId, global::Kampai.Game.TransactionTarget target, out global::System.Collections.Generic.IList<global::Kampai.Game.Instance> outputs, global::Kampai.Game.TransactionArg arg = null)
		{
			throw new global::System.NotImplementedException();
		}

		public bool FinishTransaction(global::Kampai.Game.Transaction.TransactionDefinition td, global::Kampai.Game.TransactionTarget target, global::Kampai.Game.TransactionArg arg = null)
		{
			throw new global::System.NotImplementedException();
		}

		public bool FinishTransaction(global::Kampai.Game.Transaction.TransactionDefinition td, global::Kampai.Game.TransactionTarget target, out global::System.Collections.Generic.IList<global::Kampai.Game.Instance> outputs, global::Kampai.Game.TransactionArg arg = null)
		{
			throw new global::System.NotImplementedException();
		}

		public bool VerifyTransaction(global::Kampai.Game.Transaction.TransactionDefinition transactiondef)
		{
			throw new global::System.NotImplementedException();
		}

		public bool VerifyTransaction(int transactionId)
		{
			throw new global::System.NotImplementedException();
		}

		public void StopTask(int minionId)
		{
			throw new global::System.NotImplementedException();
		}

		public void BuyCraftingSlot(int buildingID)
		{
			throw new global::System.NotImplementedException();
		}

		public void UpdateCraftingQueue(int buildingID, int itemDefId)
		{
			throw new global::System.NotImplementedException();
		}

		public bool VerifyPlayerHasRequiredInputs(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs)
		{
			throw new global::System.NotImplementedException();
		}

		public void PurchaseSlotForBuilding(int buildingID, int level)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetMinionCount()
		{
			throw new global::System.NotImplementedException();
		}

		public void CreateAndRunCustomTransaction(int defID, int quantity, global::Kampai.Game.TransactionTarget target, global::Kampai.Game.TransactionArg arg = null)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetInvestmentTimeForTransaction(int transactionID)
		{
			throw new global::System.NotImplementedException();
		}

		public void AssignNextInstanceId(global::Kampai.Util.Identifiable i)
		{
			if (i != null)
			{
				i.ID = nextId++;
			}
		}

		public global::Kampai.Game.KampaiPendingTransaction GetPendingTransaction(string externalIdentifier)
		{
			throw new global::System.NotImplementedException();
		}

		public bool PlayerAlreadyHasPlatformStoreTransactionID(string identifier)
		{
			throw new global::System.NotImplementedException();
		}

		public void AddPlatformStoreTransactionID(string identifier)
		{
			throw new global::System.NotImplementedException();
		}

		public void QueuePendingTransaction(global::Kampai.Game.KampaiPendingTransaction pendingTransaction)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.KampaiPendingTransaction ProcessPendingTransaction(string externalIdentifier, bool isFromPremium, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback = null)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.KampaiPendingTransaction CancelPendingTransaction(string externalIdentifier)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.KampaiPendingTransaction> GetPendingTransactions()
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessRush(int rushCost, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessRush(int rushCost, bool showStoreOnFail, string source, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessRush(int rushCost, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, bool byPassStorageCheck = false)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessRush(int rushCost, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, string source, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, bool byPassStorageCheck = false)
		{
			throw new global::System.NotImplementedException();
		}

		public void StartTransaction(global::Kampai.Game.Transaction.TransactionDefinition td, global::Kampai.Game.TransactionTarget target, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, global::Kampai.Game.TransactionArg arg = null, int startTime = 0, int index = 0)
		{
			throw new global::System.NotImplementedException();
		}

		public void StartTransaction(int transactionId, global::Kampai.Game.TransactionTarget target, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, global::Kampai.Game.TransactionArg arg = null, int startTime = 0, int index = 0)
		{
			throw new global::System.NotImplementedException();
		}

		public void RunEntireTransaction(int transactionId, global::Kampai.Game.TransactionTarget target, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, global::Kampai.Game.TransactionArg arg = null)
		{
			throw new global::System.NotImplementedException();
		}

		public void RunEntireTransaction(global::Kampai.Game.Transaction.TransactionDefinition td, global::Kampai.Game.TransactionTarget target, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, global::Kampai.Game.TransactionArg arg = null)
		{
			throw new global::System.NotImplementedException();
		}

		public void ExchangePremiumForGrind(int grindNeeded, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback)
		{
			throw new global::System.NotImplementedException();
		}

		public int PremiumCostForGrind(int grindNeeded)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.QuestDefinition GetProcedurallyGeneratedQuestDefinition(int id)
		{
			throw new global::System.NotImplementedException();
		}

		public bool CanAffordExchange(int grindNeeded)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.Instance> GetInstancesByDefinition<T>() where T : global::Kampai.Game.Definition
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.Instance> GetInstancesByDefinitionID(int defId)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.List<T> GetInstancesByType<T>() where T : class, global::Kampai.Game.Instance
		{
			global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>();
			foreach (global::Kampai.Game.Instance value in byInstanceId.Values)
			{
				T val = value as T;
				if (val != null)
				{
					list.Add(val);
				}
			}
			return list;
		}

		public void GetInstancesByType<T>(ref global::System.Collections.Generic.List<T> list) where T : class, global::Kampai.Game.Instance
		{
			list.AddRange(GetInstancesByType<T>());
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.Item> GetItemsByDefinition<T>() where T : global::Kampai.Game.Definition
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.LandExpansionBuilding> GetBuildingsWithExpansionID(int expansionID)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.Building> GetBuildingsWithoutState(global::Kampai.Game.BuildingState state)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.Building> GetBuildingsWithState(global::Kampai.Game.BuildingState state)
		{
			throw new global::System.NotImplementedException();
		}

		public void AddLandExpansion(global::Kampai.Game.LandExpansionConfig expansionConfig)
		{
			throw new global::System.NotImplementedException();
		}

		public bool IsExpansionPurchased(int expansionId)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetPurchasedExpansionCount()
		{
			throw new global::System.NotImplementedException();
		}

		public void QueueVillain(global::Kampai.Game.Prestige villainPrestige)
		{
			throw new global::System.NotImplementedException();
		}

		public int PopVillain()
		{
			throw new global::System.NotImplementedException();
		}

		public void SetTargetExpansion(int id)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetTargetExpansion()
		{
			throw new global::System.NotImplementedException();
		}

		public void ClearTargetExpansion()
		{
			throw new global::System.NotImplementedException();
		}

		public bool HasTargetExpansion()
		{
			throw new global::System.NotImplementedException();
		}

		public int GetFreezeTime()
		{
			throw new global::System.NotImplementedException();
		}

		public bool HasStorageBuilding()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.Minion> GetMinions(bool has, params global::Kampai.Game.MinionState[] states)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.Minion> GetIdleMinions()
		{
			throw new global::System.NotImplementedException();
		}

		public int GetHighestFtueCompleted()
		{
			throw new global::System.NotImplementedException();
		}

		public void SetHighestFtueCompleted(int newLevel)
		{
			throw new global::System.NotImplementedException();
		}

		public void IncreaseCompletedOrders()
		{
			throw new global::System.NotImplementedException();
		}

		public void IncreaseCompletedQuests()
		{
			throw new global::System.NotImplementedException();
		}

		public int GetInventoryCountByDefinitionID(int defId)
		{
			throw new global::System.NotImplementedException();
		}

		public bool CheckIfBuildingIsCapped(int defID)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessRush(global::System.Collections.Generic.IList<int> itemCostList, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, bool byPassStorageCheck = false)
		{
			throw new global::System.NotImplementedException();
		}

		public void ProcessItemPurchase(global::System.Collections.Generic.IList<int> itemCosts, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, bool byPassStorageCheck = false)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.SocialClaimRewardItem.ClaimState GetSocialClaimReward(int eventID)
		{
			throw new global::System.NotImplementedException();
		}

		public void AddSocialClaimReward(int eventID, global::Kampai.Game.SocialClaimRewardItem.ClaimState claimState)
		{
			throw new global::System.NotImplementedException();
		}

		public void CleanupSocialClaimReward(global::System.Collections.Generic.List<int> recentEventIDs)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.Player.SanityCheckFailureReason DeepScan(global::Kampai.Game.Player prevSave)
		{
			return global::Kampai.Game.Player.SanityCheckFailureReason.Passed;
		}

		public void TrackMTXPurchase(string SKU)
		{
			throw new global::System.NotImplementedException();
		}

		public int MTXPurchaseCount(string sku)
		{
			throw new global::System.NotImplementedException();
		}

		public void AddXP(int partyPoints)
		{
			throw new global::System.NotImplementedException();
		}

		public void addPendingRemption(global::Kampai.Game.Mtx.ReceiptValidationResult validationResult)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.Mtx.ReceiptValidationResult popPendingRedemption()
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.Mtx.ReceiptValidationResult topPendingRedemption()
		{
			throw new global::System.NotImplementedException();
		}

		public void SetPartyFavorQuantityToZero(int partyFavorID)
		{
		}

		public bool AllSubIngredientBRBAvailable(int itemID)
		{
			throw new global::System.NotImplementedException();
		}

		public bool ItemUsesBuildingsInList(global::Kampai.Game.IngredientsItemDefinition item, global::System.Collections.Generic.List<int> buildingIDs)
		{
			throw new global::System.NotImplementedException();
		}

		public bool ItemUsesBuildingsInList(int itemID, global::System.Collections.Generic.List<int> buildingIDs)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.List<int> GetUnavailableBuildingIDSForItem(int itemID)
		{
			throw new global::System.NotImplementedException();
		}

		public void NotifyBuildMenuNewBuilding(int buildingID)
		{
			throw new global::System.NotImplementedException();
		}

		public void AddSocialInvitationSeen(long invitationId)
		{
			throw new global::System.NotImplementedException();
		}

		public bool SeenSocialInvitation(long invitationId)
		{
			throw new global::System.NotImplementedException();
		}

		public int getAndIncrementRequestCounter()
		{
			throw new global::System.NotImplementedException();
		}

		public void RemoveTrigger(global::Kampai.Game.Trigger.TriggerInstance triggerInstance)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerInstance> GetTriggers()
		{
			throw new global::System.NotImplementedException();
		}

		public int GetMinionCountByLevel(int level)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.Trigger.TriggerInstance AddTrigger(global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.Minion> GetMinionsSortedByLevel()
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.Minion> GetMinionsByLevel(int level)
		{
			throw new global::System.NotImplementedException();
		}

		public bool HasTrigger(int triggerId)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.Trigger.TriggerInstance GetTriggerByDefinitionId(int defId)
		{
			throw new global::System.NotImplementedException();
		}

		public void LevelupMinion(int instanceID)
		{
			throw new global::System.NotImplementedException();
		}

		public uint GetCurrentStorageCapacity()
		{
			throw new global::System.NotImplementedException();
		}

		public void AwardMasterPlanComponents(global::Kampai.Game.MasterPlanDefinition definition)
		{
			throw new global::System.NotImplementedException();
		}

		public void GrantInputs(global::Kampai.Game.Transaction.TransactionDefinition transaction)
		{
			throw new global::System.NotImplementedException();
		}

		public int GetHighestUntaskedMinionLevel()
		{
			throw new global::System.NotImplementedException();
		}

		public void IngestPlayerMeta(PlayerMetaData meta)
		{
			throw new global::System.NotImplementedException();
		}

		public global::Kampai.Game.Minion GetUntaskedMinionWithHighestLevel()
		{
			throw new global::System.NotImplementedException();
		}

		public int GetHighestMinionForLeisure(int requiredMinionCount)
		{
			throw new global::System.NotImplementedException();
		}

		public bool IsInSegment(string segmentName)
		{
			throw new global::System.NotImplementedException();
		}

		public void TrackHelpTipShown(int tipDefinitionId, int time)
		{
			throw new global::System.NotImplementedException();
		}

		public float Churn()
		{
			throw new global::System.NotImplementedException();
		}

		public int GetMinionCountAtOrAboveLevel(int level)
		{
			throw new global::System.NotImplementedException();
		}

		public bool HasPurchasedBuildingAssociatedWithItem(global::Kampai.Game.IngredientsItemDefinition item)
		{
			throw new global::System.NotImplementedException();
		}

		public uint GetTotalCountByDefinitionId(int id)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.Item> GetSellableItems(out uint totalStorableQuantity, out uint totalSellableQuantity)
		{
			throw new global::System.NotImplementedException();
		}

		public global::System.Collections.Generic.IList<string> GetMTXPurchaseTracking()
		{
			throw new global::System.NotImplementedException();
		}
	}
}
