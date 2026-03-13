namespace Kampai.Game
{
	public class FinishPremiumPurchaseCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("FinishPremiumPurchaseCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public string ExternalIdentifier { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public global::Kampai.Common.Service.HealthMetrics.IClientHealthService clientHealthService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService durationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetGrindCurrencySignal setGrindCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal setStorageSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockMinionsSignal unlockMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.PurchaseLandExpansionSignal purchaseSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdatePlayerDLCTierSignal playerDLCTierSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateUIButtonsSignal updateStoreButtonsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenStoreHighlightItemSignal openStoreHighlightItemSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.FinishPurchasingSalePackSignal finishPurchasingSalePackSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		public override void Execute()
		{
			logger.Debug("[NCS] FinishPremiumPurchaseCommand.Execute()");
			global::Kampai.Game.Transaction.TransactionDefinition reward = GetReward();
			if (reward == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "{0} unknown SKU", ExternalIdentifier);
				return;
			}
			MarkSalePurchased(reward);
			RecordPurchase(reward);
			if (reward.Inputs != null && reward.Inputs.Count > 0)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Reward contains inputs {0}", reward.ID);
			}
			playerService.TrackMTXPurchase(ExternalIdentifier.Trim().ToLower());
			telemetryService.SendInAppPurchaseEventOnProductDelivery(ExternalIdentifier, reward);
			localPersistService.PutDataPlayer("IsSpender", "true");
			DispatchSignals();
			unlockMinionsSignal.Dispatch();
		}

		private global::System.Collections.IEnumerator UpdateDLCTier()
		{
			yield return new global::UnityEngine.WaitForSeconds(1f);
			playerDLCTierSignal.Dispatch();
		}

		private void ExpansionTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (!pct.Success)
			{
				return;
			}
			bool flag = false;
			int num = -1;
			foreach (global::Kampai.Util.QuantityItem output in pct.GetPendingTransaction().Outputs)
			{
				global::Kampai.Game.Definition definition = definitionService.Get<global::Kampai.Game.Definition>(output.ID);
				global::Kampai.Game.LandExpansionConfig landExpansionConfig = definition as global::Kampai.Game.LandExpansionConfig;
				if (landExpansionConfig != null)
				{
					flag = true;
					purchaseSignal.Dispatch(landExpansionConfig.expansionId, true);
					continue;
				}
				global::Kampai.Game.BuildingDefinition buildingDefinition = definition as global::Kampai.Game.BuildingDefinition;
				if (buildingDefinition != null)
				{
					updateStoreButtonsSignal.Dispatch(false);
					num = buildingDefinition.ID;
				}
			}
			if (flag)
			{
				routineRunner.StartCoroutine(UpdateDLCTier());
			}
			if (num > -1)
			{
				openStoreHighlightItemSignal.Dispatch(num, villainLairModel.currentActiveLair == null);
			}
		}

		private global::Kampai.Game.SalePackDefinition getReedemableSalePackDefinition()
		{
			foreach (global::Kampai.Game.SalePackDefinition item in definitionService.GetAll<global::Kampai.Game.SalePackDefinition>())
			{
				if (CompareSKU(item.SKU) && item.Type == global::Kampai.Game.SalePackType.Redeemable)
				{
					return item;
				}
			}
			return null;
		}

		private global::Kampai.Game.Transaction.TransactionDefinition GetReward()
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = null;
			global::Kampai.Game.SalePackDefinition reedemableSalePackDefinition = getReedemableSalePackDefinition();
			if (reedemableSalePackDefinition != null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "[NCS] Rewarding Redeemable SKU {0}", reedemableSalePackDefinition.SKU);
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition2 = reedemableSalePackDefinition.TransactionDefinition.ToDefinition();
				global::Kampai.Game.TransactionArg transactionArg = new global::Kampai.Game.TransactionArg();
				transactionArg.IsFromPremiumSource = false;
				playerService.RunEntireTransaction(transactionDefinition2.ID, global::Kampai.Game.TransactionTarget.CURRENCY, null, transactionArg);
				transactionDefinition = transactionDefinition2;
			}
			else if (!string.IsNullOrEmpty(ExternalIdentifier))
			{
				global::Kampai.Game.KampaiPendingTransaction kampaiPendingTransaction = playerService.ProcessPendingTransaction(ExternalIdentifier, true, ExpansionTransactionCallback);
				if (kampaiPendingTransaction != null)
				{
					transactionDefinition = kampaiPendingTransaction.TransactionInstance.ToDefinition();
				}
			}
			if (transactionDefinition == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "[NCS] {0} not found in pending transactions, trying to run transaction again", ExternalIdentifier);
				foreach (global::Kampai.Game.StoreItemDefinition item in definitionService.GetAll<global::Kampai.Game.StoreItemDefinition>())
				{
					global::Kampai.Game.PremiumCurrencyItemDefinition definition = null;
					if (definitionService.TryGet<global::Kampai.Game.PremiumCurrencyItemDefinition>(item.ReferencedDefID, out definition) && CompareSKU(definition.SKU))
					{
						global::Kampai.Game.SalePackDefinition salePackDefinition = definition as global::Kampai.Game.SalePackDefinition;
						global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition3 = ((salePackDefinition == null) ? definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(item.TransactionID) : salePackDefinition.TransactionDefinition.ToDefinition());
						global::Kampai.Game.TransactionArg transactionArg2 = new global::Kampai.Game.TransactionArg();
						transactionArg2.IsFromPremiumSource = true;
						playerService.RunEntireTransaction(transactionDefinition3.ID, global::Kampai.Game.TransactionTarget.CURRENCY, null, transactionArg2);
						transactionDefinition = transactionDefinition3;
					}
				}
			}
			return transactionDefinition;
		}

		private void RecordPurchase(global::Kampai.Game.Transaction.TransactionDefinition reward)
		{
			global::Kampai.Game.IPurchaseRecorder purchaseRecorder = playerService as global::Kampai.Game.IPurchaseRecorder;
			if (purchaseRecorder != null)
			{
				int premiumOutputForTransaction = global::Kampai.Game.Transaction.TransactionUtil.GetPremiumOutputForTransaction(reward);
				int grindOutputForTransaction = global::Kampai.Game.Transaction.TransactionUtil.GetGrindOutputForTransaction(reward);
				if (premiumOutputForTransaction > 0)
				{
					purchaseRecorder.AddPurchasedCurrency(true, (uint)premiumOutputForTransaction);
				}
				if (grindOutputForTransaction > 0)
				{
					purchaseRecorder.AddPurchasedCurrency(false, (uint)grindOutputForTransaction);
				}
				playerService.AlterQuantity(global::Kampai.Game.StaticItem.TRANSACTIONS_LIFETIME_COUNT_ID, 1);
				int totalGamePlaySeconds = durationService.TotalGamePlaySeconds;
				playerService.SetQuantity(global::Kampai.Game.StaticItem.LAST_GAME_TIME_PURCHASE, (totalGamePlaySeconds > 0) ? totalGamePlaySeconds : 0);
				int num = timeService.CurrentTime();
				playerService.SetQuantity(global::Kampai.Game.StaticItem.LAST_CAL_TIME_PURCHASE, (num > 0) ? num : 0);
			}
			else
			{
				logger.Error("Premium purchase occured without a purchase recorder");
			}
		}

		private void DispatchSignals()
		{
			setGrindCurrencySignal.Dispatch();
			setPremiumCurrencySignal.Dispatch();
			setStorageSignal.Dispatch();
			currencyService.CurrencyDialogClosed(true);
			clientHealthService.MarkMeterEvent("AppFlow.Purchase");
		}

		private void MarkSalePurchased(global::Kampai.Game.Transaction.TransactionDefinition reward)
		{
			if (string.IsNullOrEmpty(ExternalIdentifier))
			{
				return;
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.PackDefinition> all = definitionService.GetAll<global::Kampai.Game.PackDefinition>();
			foreach (global::Kampai.Game.PackDefinition item in all)
			{
				if (CompareSKU(item.SKU) && reward.ID == item.TransactionDefinition.ID)
				{
					finishPurchasingSalePackSignal.Dispatch(item.ID);
					break;
				}
			}
		}

		private bool CompareSKU(string SKU)
		{
			return SKU.Trim().ToLower().Equals(ExternalIdentifier.Trim().ToLower());
		}
	}
}
