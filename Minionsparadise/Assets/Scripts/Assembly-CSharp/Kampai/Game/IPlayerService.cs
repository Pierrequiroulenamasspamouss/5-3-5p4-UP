namespace Kampai.Game
{
	public interface IPlayerService
	{
		long ID { get; set; }

		global::Kampai.Game.Player LastSave { get; set; }

		int LevelUpUTC { get; set; }

		int FirstGameStartUTC { get; set; }

		int LastGameStartUTC { get; set; }

		int LastPlayedUTC { get; set; }

		string SWRVEGroup { get; set; }

		int GameplaySecondsSinceLevelUp { get; set; }

		int AccumulatedGameplayDuration { get; set; }

		global::System.Collections.Generic.List<global::Kampai.Game.Player.HelpTipTrackingItem> helpTipsTrackingData { get; }

		uint GetQuantity(global::Kampai.Game.StaticItem def);

		int GetCountByDefinitionId(int defId);

		uint GetQuantityByDefinitionId(int defId);

		uint GetTotalCountByDefinitionId(int defId);

		uint GetQuantityByInstanceId(int instanceId);

		global::System.Collections.Generic.ICollection<int> GetAnimatingBuildingIDs();

		void AlterQuantity(global::Kampai.Game.StaticItem def, int amount);

		void SetQuantity(global::Kampai.Game.StaticItem def, int amount);

		global::System.Collections.Generic.ICollection<global::Kampai.Game.Item> GetStorableItems(out uint totalQuantity);

		global::System.Collections.Generic.List<global::Kampai.Game.Item> GetSellableItems();

		global::System.Collections.Generic.List<global::Kampai.Game.Item> GetSellableItems(out uint totalStorableQuantity, out uint totalSellableQuantity);

		global::System.Collections.Generic.Dictionary<int, int> GetBuildingOnBoardCountMap();

		int GetUnlockedQuantityOfID(int defId);

		int GetPurchasedQuanityByUpsellID(int defId);

		void AddUpsellToPurchased(int defID);

		void ClearPurchasedUpsells(int defID);

		global::System.Collections.Generic.IList<T> GetUnlockedDefsByType<T>() where T : global::Kampai.Game.Definition;

		global::Kampai.Game.Player LoadPlayerData(string serialized);

		void Deserialize(string serialized, bool isRetry = false);

		byte[] SavePlayerData(global::Kampai.Game.Player playerData);

		byte[] Serialize();

		bool IsPlayerInitialized();

		void Add(global::Kampai.Game.Instance i);

		void AssignNextInstanceId(global::Kampai.Util.Identifiable i);

		void Remove(global::Kampai.Game.Instance i);

		T GetByInstanceId<T>(int id) where T : class, global::Kampai.Game.Instance;

		T GetFirstInstanceByDefinitionId<T>(int definitionId) where T : class, global::Kampai.Game.Instance;

		global::System.Collections.Generic.ICollection<T> GetByDefinitionId<T>(int id) where T : global::Kampai.Game.Instance;

		global::Kampai.Game.Transaction.WeightedInstance GetWeightedInstance(int defId, global::Kampai.Game.Transaction.WeightedDefinition wd = null);

		global::Kampai.Game.MinionParty GetMinionPartyInstance();

		void AddXP(int xp);

		void UpdateMinionPartyPointValues();

		int CalculateRushCost(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items);

		void ProcessSlotPurchase(int slotCost, bool showStoreOnFail, int slotNumber, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, int instanceId);

		void ProcessSaleCancel(int cost, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback);

		void ProcessRefreshMarket(int cost, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback);

		void ProcessItemPurchase(int itemCost, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, bool byPassStorageCheck = false);

		void ProcessItemPurchase(global::System.Collections.Generic.IList<int> itemCosts, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, bool byPassStorageCheck = false);

		void ProcessOrderFill(int slotCost, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback);

		void ProcessRush(int rushCost, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, int instanceId);

		void ProcessRush(int rushCost, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback);

		void ProcessRush(int rushCost, bool showStoreOnFail, string source, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback);

		void ProcessRush(int rushCost, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, bool byPassStorageCheck = false);

		void ProcessRush(int rushCost, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items, bool showStoreOnFail, string source, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, bool byPassStorageCheck = false);

		bool IsMissingItemFromTransaction(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition);

		global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> GetMissingItemListFromTransaction(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition);

		void StartTransaction(int transactionId, global::Kampai.Game.TransactionTarget target, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, global::Kampai.Game.TransactionArg arg = null, int startTime = 0, int index = 0);

		void StartTransaction(global::Kampai.Game.Transaction.TransactionDefinition td, global::Kampai.Game.TransactionTarget target, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, global::Kampai.Game.TransactionArg arg = null, int startTime = 0, int index = 0);

		bool FinishTransaction(int transactionId, global::Kampai.Game.TransactionTarget target, global::Kampai.Game.TransactionArg arg = null);

		bool FinishTransaction(global::Kampai.Game.Transaction.TransactionDefinition td, global::Kampai.Game.TransactionTarget target, global::Kampai.Game.TransactionArg arg = null);

		bool FinishTransaction(int transactionId, global::Kampai.Game.TransactionTarget target, out global::System.Collections.Generic.IList<global::Kampai.Game.Instance> outputs, global::Kampai.Game.TransactionArg arg = null);

		void RunEntireTransaction(int transactionId, global::Kampai.Game.TransactionTarget target, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, global::Kampai.Game.TransactionArg arg = null);

		void RunEntireTransaction(global::Kampai.Game.Transaction.TransactionDefinition def, global::Kampai.Game.TransactionTarget target, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback, global::Kampai.Game.TransactionArg arg = null);

		bool VerifyTransaction(global::Kampai.Game.Transaction.TransactionDefinition transactiondef);

		bool VerifyTransaction(int transactionId);

		void StopTask(int minionId);

		void BuyCraftingSlot(int buildingID);

		void UpdateCraftingQueue(int buildingID, int itemDefId);

		bool VerifyPlayerHasRequiredInputs(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs);

		void PurchaseSlotForBuilding(int buildingID, int level);

		int GetMinionCount();

		void CreateAndRunCustomTransaction(int defID, int quantity, global::Kampai.Game.TransactionTarget target, global::Kampai.Game.TransactionArg args = null);

		global::Kampai.Game.KampaiPendingTransaction GetPendingTransaction(string externalIdentifier);

		bool PlayerAlreadyHasPlatformStoreTransactionID(string identifier);

		void AddPlatformStoreTransactionID(string identifier);

		global::System.Collections.Generic.IList<global::Kampai.Game.KampaiPendingTransaction> GetPendingTransactions();

		void QueuePendingTransaction(global::Kampai.Game.KampaiPendingTransaction pendingTransaction);

		global::Kampai.Game.KampaiPendingTransaction ProcessPendingTransaction(string externalIdentifier, bool isFromPremium, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback = null);

		global::Kampai.Game.KampaiPendingTransaction CancelPendingTransaction(string externalIdentifier);

		void ExchangePremiumForGrind(int grindNeeded, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback);

		int PremiumCostForGrind(int grindNeeded);

		bool CanAffordExchange(int grindNeeded);

		int GetInvestmentTimeForTransaction(int transactionID);

		global::System.Collections.Generic.IList<global::Kampai.Game.Instance> GetInstancesByDefinition<T>() where T : global::Kampai.Game.Definition;

		I GetFirstInstanceByDefintion<I, D>() where I : class, global::Kampai.Game.Instance where D : global::Kampai.Game.Definition;

		global::System.Collections.Generic.IList<global::Kampai.Game.Instance> GetInstancesByDefinitionID(int defId);

		global::System.Collections.Generic.List<T> GetInstancesByType<T>() where T : class, global::Kampai.Game.Instance;

		void GetInstancesByType<T>(ref global::System.Collections.Generic.List<T> list) where T : class, global::Kampai.Game.Instance;

		global::System.Collections.Generic.IList<global::Kampai.Game.Item> GetItemsByDefinition<T>() where T : global::Kampai.Game.Definition;

		global::System.Collections.Generic.IList<global::Kampai.Game.Building> GetBuildingsWithState(global::Kampai.Game.BuildingState state);

		global::System.Collections.Generic.IList<global::Kampai.Game.Building> GetBuildingsWithoutState(global::Kampai.Game.BuildingState excludedState);

		global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerInstance> GetTriggers();

		void RemoveTrigger(global::Kampai.Game.Trigger.TriggerInstance triggerInstance);

		global::Kampai.Game.Trigger.TriggerInstance AddTrigger(global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition);

		bool HasTrigger(int triggerId);

		global::Kampai.Game.Trigger.TriggerInstance GetTriggerByDefinitionId(int defId);

		void AddLandExpansion(global::Kampai.Game.LandExpansionConfig expansionConfig);

		bool IsExpansionPurchased(int expansionId);

		int GetPurchasedExpansionCount();

		void QueueVillain(global::Kampai.Game.Prestige villainPrestige);

		int PopVillain();

		void SetTargetExpansion(int id);

		int GetTargetExpansion();

		void ClearTargetExpansion();

		bool HasTargetExpansion();

		bool HasStorageBuilding();

		bool isStorageFull();

		uint GetAvailableStorageCapacity();

		uint GetCurrentStorageCapacity();

		global::System.Collections.Generic.List<global::Kampai.Game.Minion> GetMinions(bool has, params global::Kampai.Game.MinionState[] states);

		global::System.Collections.Generic.List<global::Kampai.Game.Minion> GetIdleMinions();

		void IncreaseCompletedOrders();

		void IncreaseCompletedQuests();

		int GetHighestFtueCompleted();

		void SetHighestFtueCompleted(int newLevel);

		int GetInventoryCountByDefinitionID(int defId);

		bool CheckIfBuildingIsCapped(int defID);

		global::Kampai.Game.SocialClaimRewardItem.ClaimState GetSocialClaimReward(int eventID);

		void AddSocialClaimReward(int eventID, global::Kampai.Game.SocialClaimRewardItem.ClaimState claimState);

		void CleanupSocialClaimReward(global::System.Collections.Generic.List<int> recentEventIDs);

		uint GetStorageCount();

		void TrackMTXPurchase(string SKU);

		global::System.Collections.Generic.IList<string> GetMTXPurchaseTracking();

		int MTXPurchaseCount(string sku);

		bool IsMinionPartyUnlocked();

		bool HasPurchasedMinigamePack();

		global::Kampai.Game.Player.SanityCheckFailureReason DeepScan(global::Kampai.Game.Player prevSave);

		void addPendingRemption(global::Kampai.Game.Mtx.ReceiptValidationResult validationResult);

		global::Kampai.Game.Mtx.ReceiptValidationResult topPendingRedemption();

		global::Kampai.Game.Mtx.ReceiptValidationResult popPendingRedemption();

		bool ItemUsesBuildingsInList(global::Kampai.Game.IngredientsItemDefinition item, global::System.Collections.Generic.List<int> buildingIDs);

		bool ItemUsesBuildingsInList(int itemID, global::System.Collections.Generic.List<int> buildingIDs);

		global::System.Collections.Generic.List<int> GetUnavailableBuildingIDSForItem(int itemID);

		void NotifyBuildMenuNewBuilding(int buildingID);

		void AddSocialInvitationSeen(long invitationId);

		bool SeenSocialInvitation(long invitationId);

		int getAndIncrementRequestCounter();

		void GrantInputs(global::Kampai.Game.Transaction.TransactionDefinition transaction);

		int GetMinionCountByLevel(int level);

		int GetMinionCountAtOrAboveLevel(int level);

		void TrackHelpTipShown(int tipDefinitionId, int time);

		int GetHighestUntaskedMinionLevel();

		int GetHighestMinionForLeisure(int requiredMinionCount);

		global::Kampai.Game.Minion GetUntaskedMinionWithHighestLevel();

		void IngestPlayerMeta(PlayerMetaData meta);

		global::System.Collections.Generic.List<global::Kampai.Game.Minion> GetMinionsSortedByLevel();

		global::System.Collections.Generic.List<global::Kampai.Game.Minion> GetMinionsByLevel(int level);

		bool IsInSegment(string segmentName);

		void LevelupMinion(int instanceID);

		float Churn();

		bool HasPurchasedBuildingAssociatedWithItem(global::Kampai.Game.IngredientsItemDefinition item);
	}
}
