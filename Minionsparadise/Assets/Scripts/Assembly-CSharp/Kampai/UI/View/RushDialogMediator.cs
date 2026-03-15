namespace Kampai.UI.View
{
	public abstract class RushDialogMediator<T> : global::Kampai.UI.View.UIStackMediator<T> where T : global::Kampai.UI.View.RushDialogView
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RushDialogMediator") as global::Kampai.Util.IKampaiLogger;

		protected int rushCost;

		protected global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> requiredItems;

		protected global::System.Collections.Generic.List<int> requiredItemPremiumCosts;

		protected global::Kampai.UI.View.RushDialogView.RushDialogType dialogType;

		protected global::Kampai.Game.PendingCurrencyTransaction pendingCurrencyTransaction;

		protected bool purchaseInProgress;

		protected bool purchaseSuccess;

		protected bool isClosing;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RushDialogConfirmationSignal confirmedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateStorageItemsSignal updateStorageItemsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetGrindCurrencySignal setGrindCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenStorageBuildingSignal openStorageBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.UI.View.GoToResourceButtonClickedSignal gotoSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			requiredItems = null;
			openStorageBuildingSignal.AddListener(StorageBuildingOpened);
			base.view.PurchaseButtonView.ClickedSignal.AddListener(PurchaseButtonClicked);
			base.view.UpgradeButton.ClickedSignal.AddListener(PurchaseOrUpgradeButtonClicked);
			base.view.OnMenuClose.AddListener(OnMenuClose);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			openStorageBuildingSignal.RemoveListener(StorageBuildingOpened);
			base.view.PurchaseButtonView.ClickedSignal.RemoveListener(PurchaseButtonClicked);
			base.view.UpgradeButton.ClickedSignal.RemoveListener(PurchaseOrUpgradeButtonClicked);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			T val = base.view;
			val.Init(localService);
			dialogType = args.Get<global::Kampai.UI.View.RushDialogView.RushDialogType>();
			global::Kampai.Game.PendingCurrencyTransaction pendingCurrencyTransaction = (this.pendingCurrencyTransaction = args.Get<global::Kampai.Game.PendingCurrencyTransaction>());
			purchaseInProgress = false;
			isClosing = false;
			LoadItems(pendingCurrencyTransaction.GetPendingTransaction(), dialogType);
			SetHeadline(pendingCurrencyTransaction);
		}

		protected virtual void SetHeadline(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
		}

		private void StorageBuildingOpened(global::Kampai.Game.StorageBuilding storageBuilding, bool directOpen)
		{
			purchaseInProgress = false;
		}

		protected override void Close()
		{
			isClosing = true;
			if (pendingCurrencyTransaction != null)
			{
				global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback = pendingCurrencyTransaction.GetCallback();
				if (callback != null)
				{
					pendingCurrencyTransaction.Success = false;
					callback(pendingCurrencyTransaction);
				}
			}
			if (base.view != null)
			{
				T val = base.view;
				val.Close();
			}
		}

		protected virtual void OnMenuClose()
		{
			if (requiredItems != null)
			{
				SendRushTelemetry(pendingCurrencyTransaction, requiredItems, purchaseSuccess | purchaseInProgress);
			}
			isClosing = true;
			if (dialogType == global::Kampai.UI.View.RushDialogView.RushDialogType.STORAGE_EXPAND)
			{
				hideSkrim.Dispatch("RushStorageSkrim");
				guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_OutOfResourceForStorage");
			}
			else
			{
				hideSkrim.Dispatch("RushSkrim");
				guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_MissingResources");
			}
		}

		internal virtual global::Kampai.UI.View.RequiredItemView BuildItem(global::Kampai.Game.Definition definition, uint itemsInInventory, int itemsLack, bool isAvailable, global::Kampai.Main.ILocalizationService localService)
		{
			if (definition == null)
			{
				throw new global::System.ArgumentNullException("definition", "RequiredItemBuilder: You are passing in null definitions!");
			}
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_RequiredItem_ForShow") as global::UnityEngine.GameObject;
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			global::Kampai.UI.View.RequiredItemView component = gameObject.GetComponent<global::Kampai.UI.View.RequiredItemView>();
			global::Kampai.Game.ItemDefinition itemDefinition = definition as global::Kampai.Game.ItemDefinition;
			global::UnityEngine.Sprite sprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			global::UnityEngine.Sprite maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
			component.ItemIcon.sprite = sprite;
			component.ItemIcon.maskSprite = maskSprite;
			int num = (int)itemsInInventory + itemsLack;
			int num2 = ((itemsLack >= 0) ? itemsLack : 0);
			component.ItemNeeded = num;
			component.ItemQuantity.text = string.Format("{0}/{1}", itemsInInventory, num);
			component.PurchasePanel.SetActive(!isAvailable);
			int num3 = global::UnityEngine.Mathf.FloorToInt(itemDefinition.BasePremiumCost * (float)num2);
			num3 = ((num3 == 0 && num2 > 0) ? 1 : num3);
			component.ItemCost.text = UIUtils.FormatLargeNumber(num3);
			component.ItemDefinitionID = itemDefinition.ID;
			if (!isAvailable)
			{
				global::Kampai.Util.QuantityItem item = new global::Kampai.Util.QuantityItem(itemDefinition.ID, (uint)num2);
				component.RushBtn.RushCost = num3;
				component.RushBtn.Item = item;
			}
			return component;
		}

		protected virtual void LoadItems(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, global::Kampai.UI.View.RushDialogView.RushDialogType type)
		{
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = transactionDefinition.Inputs;
			if (inputs != null)
			{
				int count = inputs.Count;
				bool showPurchaseButton = false;
				requiredItems = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				requiredItemPremiumCosts = new global::System.Collections.Generic.List<int>();
				for (int i = 0; i < count; i++)
				{
					global::Kampai.Game.ItemDefinition definition = definitionService.Get<global::Kampai.Game.ItemDefinition>(inputs[i].ID);
					global::Kampai.Util.QuantityItem quantityItem = null;
					uint quantityByDefinitionId = playerService.GetQuantityByDefinitionId(inputs[i].ID);
					int num = (int)(inputs[i].Quantity - quantityByDefinitionId);
					bool flag = false;
					if (num <= 0)
					{
						flag = true;
					}
					else
					{
						flag = false;
						quantityItem = new global::Kampai.Util.QuantityItem(inputs[i].ID, (uint)num);
						requiredItems.Add(quantityItem);
					}
					global::Kampai.UI.View.RequiredItemView requiredItemView = BuildItem(definition, quantityByDefinitionId, num, flag, localService);
					if (!flag)
					{
						requiredItemView.RushBtn.RushButtonClickedSignal.AddListener(IndividualRushButtonClicked);
					}
					int intFromFormattedLargeNumber = UIUtils.GetIntFromFormattedLargeNumber(requiredItemView.ItemCost.text);
					if (intFromFormattedLargeNumber != 0)
					{
						requiredItemPremiumCosts.Add(intFromFormattedLargeNumber);
					}
					T val = base.view;
					val.AddRequiredItem(requiredItemView, i, base.view.ScrollViewParent);
				}
				if (requiredItems.Count != 0)
				{
					rushCost = playerService.CalculateRushCost(requiredItems);
					T val2 = base.view;
					val2.SetupItemCost(rushCost);
					showPurchaseButton = true;
				}
				T val3 = base.view;
				val3.SetupItemCount(count);
				T val4 = base.view;
				val4.SetupDialog(type, showPurchaseButton);
				base.gameObject.SetActive(true);
			}
			else
			{
				logger.Debug("Showing rush dialog without require items");
			}
		}

		protected virtual void PurchaseSuccess()
		{
			purchaseSuccess = true;
			confirmedSignal.Dispatch();
		}

		protected virtual void IndividualPurchaseSuccess()
		{
			updateStorageItemsSignal.Dispatch();
		}

		protected virtual void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			purchaseInProgress = false;
			if (pct.Success)
			{
				PurchaseSuccess();
				soundFXSignal.Dispatch("Play_button_premium_01");
				OnMenuClose();
				setPremiumCurrencySignal.Dispatch();
				setGrindCurrencySignal.Dispatch();
			}
			else if (pct.ParentSuccess)
			{
				PurchaseButtonClicked();
			}
		}

		protected void IndividualRushButtonClicked(int myRushCost, global::Kampai.Util.QuantityItem item, bool proceedTransaction)
		{
			if (!purchaseInProgress && !isClosing)
			{
				T val = base.view;
				val.resetAllExceptRequiredItemTapState(item.ID);
				if (proceedTransaction)
				{
					global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> list = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
					list.Add(item);
					playerService.ProcessItemPurchase(myRushCost, list, true, IndividualRushTransactionCallback(myRushCost, item));
				}
			}
		}

		protected void PurchaseButtonClicked()
		{
			T val = base.view;
			val.resetAllRequiredItemsTapState();
			if (base.view.PurchaseButtonView.isDoubleConfirmed())
			{
				ExecuteRushTransaction();
			}
		}

		private void PurchaseOrUpgradeButtonClicked()
		{
			ExecuteRushTransaction();
		}

		protected void ExecuteRushTransaction()
		{
			if (!purchaseInProgress && !isClosing)
			{
				if (requiredItems != null)
				{
					purchaseInProgress = true;
					if (requiredItems.Count == 0)
					{
						PurchaseSuccess();
						OnMenuClose();
					}
					else
					{
						bool byPassStorageCheck = SkipStorageCheckOnRushTransaction();
						playerService.ProcessItemPurchase(requiredItemPremiumCosts, requiredItems, true, RushTransactionCallback, byPassStorageCheck);
					}
				}
				else
				{
					logger.Debug("no required items found");
				}
			}
			else
			{
				logger.Debug("Purchase is already in progress");
			}
		}

		protected bool SkipStorageCheckOnRushTransaction()
		{
			return dialogType == global::Kampai.UI.View.RushDialogView.RushDialogType.STORAGE_EXPAND || dialogType == global::Kampai.UI.View.RushDialogView.RushDialogType.BRIDGE_QUEST || dialogType == global::Kampai.UI.View.RushDialogView.RushDialogType.VILLAIN_LAIR_PORTAL_REPAIR || dialogType == global::Kampai.UI.View.RushDialogView.RushDialogType.VILLAIN_LAIR_RESOURCE_PLOT;
		}

		private global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> IndividualRushTransactionCallback(int myRushCost, global::Kampai.Util.QuantityItem item)
		{
			return delegate(global::Kampai.Game.PendingCurrencyTransaction pct)
			{
				purchaseInProgress = false;
				if (pct.Success)
				{
					RemovingRequiredItems(item);
					T val = base.view;
					val.DeleteItem(item.ID);
					soundFXSignal.Dispatch("Play_button_premium_01");
					setPremiumCurrencySignal.Dispatch();
					rushCost -= myRushCost;
					if (rushCost == 0)
					{
						PurchaseSuccess();
						OnMenuClose();
					}
					else
					{
						T val2 = base.view;
						val2.SetupItemCost(rushCost);
						IndividualPurchaseSuccess();
					}
				}
			};
		}

		private void RemovingRequiredItems(global::Kampai.Util.QuantityItem item)
		{
			foreach (global::Kampai.Util.QuantityItem requiredItem in requiredItems)
			{
				if (requiredItem.ID == item.ID)
				{
					requiredItem.Quantity -= item.Quantity;
					if (requiredItem.Quantity == 0)
					{
						requiredItems.Remove(requiredItem);
						break;
					}
				}
			}
		}

		private void SendRushTelemetry(global::Kampai.Game.PendingCurrencyTransaction pct, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> requiredItems, bool purchaseSuccess)
		{
			string sourceName = "unknown";
			if (dialogType == global::Kampai.UI.View.RushDialogView.RushDialogType.STORAGE_EXPAND)
			{
				sourceName = "StorageExpand";
			}
			telemetryService.Send_Telemetry_EVT_PINCH_PROMPT(sourceName, pct, requiredItems, purchaseSuccess.ToString());
		}

		protected void gotoButtonHandler(int itemDefinitionId)
		{
			Close();
			gotoSignal.Dispatch(itemDefinitionId);
		}
	}
}
