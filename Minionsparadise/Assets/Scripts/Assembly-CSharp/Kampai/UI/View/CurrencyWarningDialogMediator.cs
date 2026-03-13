namespace Kampai.UI.View
{
	public class CurrencyWarningDialogMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.CurrencyWarningDialogView>
	{
		private global::Kampai.UI.View.CurrencyWarningModel model;

		private bool purchaseInProgress;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowMTXStoreSignal showMTXStoreSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetGrindCurrencySignal setGrindCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateWayFinderSignal createWayFinderSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.PurchaseButton.ClickedSignal.AddListener(PurchaseButtonClicked);
			base.view.CancelButton.ClickedSignal.AddListener(CancelButtonClicked);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.PurchaseButton.ClickedSignal.RemoveListener(PurchaseButtonClicked);
			base.view.CancelButton.ClickedSignal.RemoveListener(CancelButtonClicked);
		}

		protected override void Close()
		{
			Close();
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.UI.View.CurrencyWarningModel currencyQuantity = args.Get<global::Kampai.UI.View.CurrencyWarningModel>();
			SetCurrencyQuantity(currencyQuantity);
			purchaseInProgress = false;
			sfxSignal.Dispatch("Play_not_enough_items_01");
		}

		private void SetCurrencyQuantity(global::Kampai.UI.View.CurrencyWarningModel model)
		{
			this.model = model;
			base.view.SetCurrencyNeeded(model.Cost, model.Amount);
		}

		private bool CheckCurrencyType()
		{
			if (model.Type.Equals(global::Kampai.Game.StoreItemType.GrindCurrency) && playerService.CanAffordExchange(model.Amount))
			{
				playerService.ExchangePremiumForGrind(model.Amount, TransactionCallback);
				return true;
			}
			if (model.Type.Equals(global::Kampai.Game.StoreItemType.PremiumCurrency) && playerService.GetQuantity(global::Kampai.Game.StaticItem.PREMIUM_CURRENCY_ID) >= model.Amount)
			{
				Close(true);
				return true;
			}
			return false;
		}

		private void PurchaseButtonClicked()
		{
			if ((base.view.PurchaseButton.isDoubleConfirmed() || model.Type == global::Kampai.Game.StoreItemType.PremiumCurrency) && !CheckCurrencyType())
			{
				purchaseInProgress = true;
				currencyService.CurrencyDialogOpened(new global::Kampai.Game.PendingCurrencyTransaction(null, false, model.Amount, null, null, PremiumStoreClosedCallback));
				showMTXStoreSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(800002, model.Cost));
			}
		}

		private void PremiumStoreClosedCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			CheckCurrencyType();
		}

		private void TransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				sfxSignal.Dispatch("Play_button_premium_01");
				Close(true);
			}
			else if (pct.ParentSuccess)
			{
				PurchaseButtonClicked();
			}
			else
			{
				Close();
			}
			setPremiumCurrencySignal.Dispatch();
			setGrindCurrencySignal.Dispatch();
		}

		private void CancelButtonClicked()
		{
			Close();
			createWayFinderSignal.Dispatch(new global::Kampai.UI.View.WayFinderSettings(309, false));
		}

		private void Close(bool success = false)
		{
			hideSkrim.Dispatch("CurrencySkrim");
			currencyService.CurrencyDialogClosed(success);
			SendRushTelemetry(model.PendingTransaction, model.PendingTransaction.GetPendingTransaction().Inputs, success | purchaseInProgress);
			if (model.Type.Equals(global::Kampai.Game.StoreItemType.GrindCurrency))
			{
				guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "GrindCurrencyWarning");
			}
			else if (model.Type.Equals(global::Kampai.Game.StoreItemType.PremiumCurrency))
			{
				guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "PremiumCurrencyWarning");
			}
		}

		private void SendRushTelemetry(global::Kampai.Game.PendingCurrencyTransaction pct, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> requiredItems, bool purchaseSuccess)
		{
			string sourceName = "unknown";
			if (pct != null && pct.GetTransactionArg() != null)
			{
				sourceName = pct.GetTransactionArg().Source;
			}
			telemetryService.Send_Telemetry_EVT_PINCH_PROMPT(sourceName, pct, requiredItems, purchaseSuccess.ToString());
		}
	}
}
