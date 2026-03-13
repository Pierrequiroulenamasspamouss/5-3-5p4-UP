namespace Kampai.UI.View
{
	public class PurchaseSalePackCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PurchaseSalePackCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartPremiumPurchaseSignal startPremiumPurchaseSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenStoreHighlightItemSignal openStoreHighlightItemSignal { get; set; }

		[Inject]
		public global::Kampai.Game.InsufficientInputsSignal insufficientInputsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CollectRedemptionSignal collectRedemptionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateUIButtonsSignal updateStoreButtonsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.FinishPurchasingSalePackSignal finishPurchasingSalePackSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveSalePackSignal removeSalePackSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseHUDSignal closeSignal { get; set; }

		[Inject]
		public int packDefinitionId { get; set; }

		public override void Execute()
		{
			logger.Debug("In Purchase Sale Command");
			global::Kampai.Game.PackDefinition packDefinition = definitionService.Get<global::Kampai.Game.PackDefinition>(packDefinitionId);
			if (packDefinition == null)
			{
				logger.Error("Unable to find the sale definition with id: {0}", packDefinitionId);
				return;
			}
			if (!packDefinition.DisableDynamicUnlock)
			{
				IncrementBuildingCount(packDefinition);
			}
			global::Kampai.Game.SalePackDefinition salePackDefinition = packDefinition as global::Kampai.Game.SalePackDefinition;
			if (PackUtil.HasPurchasedEnough(packDefinition, playerService))
			{
				logger.Error("Sale for definition id {0} already purchased.", packDefinitionId);
				if (salePackDefinition != null)
				{
					RemoveUpsellFromHUD(salePackDefinition);
				}
				return;
			}
			closeSignal.Dispatch(true);
			if (!global::Kampai.Util.NetworkUtil.IsConnected())
			{
				popupMessageSignal.Dispatch(localService.GetString("NoInternetConnection"), global::Kampai.UI.View.PopupMessageType.NORMAL);
			}
			else if (salePackDefinition != null && salePackDefinition.Type == global::Kampai.Game.SalePackType.Redeemable)
			{
				logger.Info("Attempting to collect Redeemable sku");
				collectRedemptionSignal.Dispatch();
			}
			else if (!string.IsNullOrEmpty(packDefinition.SKU) && packDefinition.TransactionType == global::Kampai.Game.UpsellTransactionType.Cash)
			{
				global::Kampai.Game.KampaiPendingTransaction kampaiPendingTransaction = new global::Kampai.Game.KampaiPendingTransaction();
				kampaiPendingTransaction.ExternalIdentifier = packDefinition.SKU;
				kampaiPendingTransaction.StoreItemDefinitionId = packDefinition.ID;
				kampaiPendingTransaction.TransactionInstance = packDefinition.TransactionDefinition;
				kampaiPendingTransaction.UTCTimeCreated = timeService.CurrentTime();
				startPremiumPurchaseSignal.Dispatch(kampaiPendingTransaction);
			}
			else if (packDefinition.TransactionDefinition != null)
			{
				PerformTransaction(packDefinition);
			}
		}

		private void PerformTransaction(global::Kampai.Game.PackDefinition definition)
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definition.TransactionDefinition.ToDefinition().CopyTransaction();
			if (definition.TransactionType == global::Kampai.Game.UpsellTransactionType.GrindDiscount)
			{
				uint quantity = (uint)((float)global::Kampai.Game.Transaction.TransactionUtil.GetTransactionCurrencyCost(transactionDefinition, definitionService, playerService, global::Kampai.Game.StaticItem.GRIND_CURRENCY_ID) * definition.getDiscountRate());
				global::Kampai.Util.QuantityItem item = new global::Kampai.Util.QuantityItem(0, quantity);
				transactionDefinition.Inputs.Add(item);
			}
			else if (definition.TransactionType == global::Kampai.Game.UpsellTransactionType.PremiumDiscount)
			{
				uint quantity2 = (uint)((float)global::Kampai.Game.Transaction.TransactionUtil.GetTransactionCurrencyCost(transactionDefinition, definitionService, playerService, global::Kampai.Game.StaticItem.PREMIUM_CURRENCY_ID) * definition.getDiscountRate());
				global::Kampai.Util.QuantityItem item2 = new global::Kampai.Util.QuantityItem(1, quantity2);
				transactionDefinition.Inputs.Add(item2);
			}
			if (playerService.VerifyTransaction(transactionDefinition))
			{
				playerService.RunEntireTransaction(transactionDefinition, global::Kampai.Game.TransactionTarget.CURRENCY, TransactionCallback, new global::Kampai.Game.TransactionArg("Upsell"));
				return;
			}
			global::Kampai.Game.PendingCurrencyTransaction pendingCurrencyTransaction = new global::Kampai.Game.PendingCurrencyTransaction(transactionDefinition, false, 0, null, null, InsufficientInputsCallback);
			currencyService.CurrencyDialogOpened(pendingCurrencyTransaction);
			insufficientInputsSignal.Dispatch(pendingCurrencyTransaction, true);
		}

		private void TransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (!pct.Success)
			{
				return;
			}
			global::Kampai.Game.PackDefinition packDefinition = definitionService.Get<global::Kampai.Game.PackDefinition>(packDefinitionId);
			if (packDefinition == null)
			{
				logger.Error("Unable to find the sale definition with id: {0}", packDefinitionId);
				return;
			}
			finishPurchasingSalePackSignal.Dispatch(packDefinitionId);
			foreach (global::Kampai.Util.QuantityItem output in packDefinition.TransactionDefinition.Outputs)
			{
				global::Kampai.Game.BuildingDefinition definition;
				if (definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(output.ID, out definition))
				{
					updateStoreButtonsSignal.Dispatch(false);
					openStoreHighlightItemSignal.Dispatch(definition.ID, true);
					break;
				}
			}
		}

		private void IncrementBuildingCount(global::Kampai.Game.PackDefinition definition)
		{
			global::Kampai.Util.BuildingPacksHelper.UpdateTransactionUnlocksList(definition.TransactionDefinition, base.injectionBinder);
		}

		private void InsufficientInputsCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
		}

		private void RemoveUpsellFromHUD(global::Kampai.Game.SalePackDefinition salePackDefinition)
		{
			if (salePackDefinition == null)
			{
				logger.Error("Unable to find the sale definition with id: {0}", packDefinitionId);
				return;
			}
			global::Kampai.Game.Sale firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Sale>(packDefinitionId);
			if (firstInstanceByDefinitionId == null)
			{
				logger.Error("Sale instance not found for definition id {0}", packDefinitionId);
			}
			else
			{
				removeSalePackSignal.Dispatch(firstInstanceByDefinitionId.ID);
			}
		}
	}
}
