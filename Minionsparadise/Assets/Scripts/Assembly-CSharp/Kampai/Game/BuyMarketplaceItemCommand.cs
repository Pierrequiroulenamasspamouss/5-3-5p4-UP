namespace Kampai.Game
{
	public class BuyMarketplaceItemCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("BuyMarketplaceItemCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.Transaction.TransactionDefinition transaction;

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetGrindCurrencySignal setGrindCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.Game.MarketplaceBuyItem marketplaceItem { get; set; }

		[Inject]
		public int slotIndex { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateBuySlotSignal updateBuySlot { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateStorageItemsSignal updateStorageItemsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ReportMarketplaceTransactionSignal reportMarketplaceTransactionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displaySignal { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		public override void Execute()
		{
			if (marketplaceItem == null)
			{
				logger.Error(string.Format("Can't load marketplace buy item for slot index {0}", slotIndex));
				return;
			}
			transaction = new global::Kampai.Game.Transaction.TransactionDefinition();
			transaction.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			transaction.Inputs.Add(new global::Kampai.Util.QuantityItem(0, (uint)marketplaceItem.BuyPrice));
			transaction.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			transaction.Outputs.Add(new global::Kampai.Util.QuantityItem(marketplaceItem.Definition.ItemID, (uint)marketplaceItem.BuyQuantity));
			playerService.RunEntireTransaction(transaction, global::Kampai.Game.TransactionTarget.MARKETPLACE, TransactionCallback, NewTransactionArg());
		}

		private global::Kampai.Game.TransactionArg NewTransactionArg()
		{
			global::Kampai.Game.TransactionArg transactionArg = new global::Kampai.Game.TransactionArg();
			transactionArg.InstanceId = 314;
			transactionArg.Source = "Marketplace";
			return transactionArg;
		}

		private void TransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pendingTransaction)
		{
			if (pendingTransaction.Success)
			{
				marketplaceItem.BoughtFlag = true;
				updateStorageItemsSignal.Dispatch();
				setGrindCurrencySignal.Dispatch();
				questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Harvest, global::Kampai.Game.QuestTaskTransition.Complete);
				updateBuySlot.Dispatch(slotIndex, true);
				reportMarketplaceTransactionSignal.Dispatch(marketplaceItem);
				displaySignal.Dispatch(19000010, false, new global::strange.extensions.signal.impl.Signal<bool>());
				return;
			}
			if (pendingTransaction.ParentSuccess)
			{
				playerService.RunEntireTransaction(transaction, global::Kampai.Game.TransactionTarget.MARKETPLACE, TransactionCallback, NewTransactionArg());
				return;
			}
			logger.Info(string.Format("Marketplace buy item transaction failed for itemID {0} in slot {1}", marketplaceItem.ID, slotIndex));
			if (pendingTransaction.FailReason == global::Kampai.Game.CurrencyTransactionFailReason.STORAGE)
			{
				updateBuySlot.Dispatch(slotIndex, false);
				global::Kampai.Game.StorageBuilding storageBuilding = GetStorageBuilding();
				if (storageBuilding.CurrentStorageBuildingLevel == storageBuilding.Definition.StorageUpgrades.Count - 1)
				{
					string type = localizationService.GetString("MaxStorageCapacityReached");
					popupMessageSignal.Dispatch(type, global::Kampai.UI.View.PopupMessageType.NORMAL);
				}
			}
		}

		private global::Kampai.Game.StorageBuilding GetStorageBuilding()
		{
			global::Kampai.Game.StorageBuilding result = null;
			using (global::System.Collections.Generic.IEnumerator<global::Kampai.Game.StorageBuilding> enumerator = playerService.GetByDefinitionId<global::Kampai.Game.StorageBuilding>(3018).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					global::Kampai.Game.StorageBuilding current = enumerator.Current;
					result = current;
				}
			}
			return result;
		}
	}
}
