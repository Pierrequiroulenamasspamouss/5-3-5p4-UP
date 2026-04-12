namespace Kampai.UI.View
{
	public class CurrencyStoreMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.CurrencyStoreView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CurrencyStoreMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.PremiumCurrencyCatalogUpdatedSignal premiumCurrencyCatalogUpdatedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CurrencyDialogClosedSignal currencyDialogClosedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestScriptService questScriptService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.FTUELevelChangedSignal ftueLevelChangedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshMTXStoreSignal refreshMTXStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CancelPurchaseSignal cancelPurchaseSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseHUDSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.ICurrencyStoreService currencyStoreService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			refreshMTXStoreSignal.AddListener(OnRefreshStore);
			premiumCurrencyCatalogUpdatedSignal.AddListener(OnPremiumCatalogUpdated);
			closeSignal.AddListener(CloseMenu);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			ftueLevelChangedSignal.AddListener(FTUELevelChanged);
			base.view.backgroundButton.ClickedSignal.AddListener(Close);
			base.view.Init(localService);
			if (playerService.GetHighestFtueCompleted() < 9)
			{
				questScriptService.PauseQuestScripts();
			}
		}

		public override void OnRemove()
		{
			base.OnRemove();
			refreshMTXStoreSignal.RemoveListener(OnRefreshStore);
			premiumCurrencyCatalogUpdatedSignal.RemoveListener(OnPremiumCatalogUpdated);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.backgroundButton.ClickedSignal.RemoveListener(Close);
			ftueLevelChangedSignal.RemoveListener(FTUELevelChanged);
			if (playerService.GetHighestFtueCompleted() < 9)
			{
				questScriptService.ResumeQuestScripts();
			}
		}

		protected override void Close()
		{
			OnClickBackgroundButton();
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.Util.Tuple<int, int> tuple = args.Get<global::Kampai.Util.Tuple<int, int>>();
			int item = tuple.Item1;
			OnStoreDefinitionLoaded(item);
			int item2 = tuple.Item2;
			base.view.ShowCategory(item, item2, true);
			if (!AllItemsAreLocked())
			{
				currencyStoreService.MarkCategoryAsViewed(item);
			}
			base.view.Open();
		}

		private void CloseMenu(bool closeCurrency)
		{
			closeSignal.RemoveListener(CloseMenu);
			base.view.Cleanup();
		}

		private void OnClickBackgroundButton()
		{
			cancelPurchaseSignal.Dispatch(false);
			closeSignal.RemoveListener(CloseMenu);
			closeSignal.Dispatch(false);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CancelBuildingMovementSignal>().Dispatch(false);
			base.view.Cleanup();
		}

		private void GetStrings(global::Kampai.Game.StoreItemDefinition storeItemDef, global::Kampai.Game.PremiumCurrencyItemDefinition premium, global::Kampai.Game.Transaction.TransactionDefinition transaction, ref string inputStr, ref string outputStr)
		{
			if (premium != null)
			{
				inputStr = currencyService.GetPriceWithCurrencyAndFormat(premium.SKU);
				if (storeItemDef.Type == global::Kampai.Game.StoreItemType.PremiumCurrency)
				{
					outputStr = UIUtils.FormatLargeNumber(global::Kampai.Game.Transaction.TransactionUtil.GetPremiumOutputForTransaction(transaction));
				}
				else if (storeItemDef.Type == global::Kampai.Game.StoreItemType.SalePack)
				{
					outputStr = localService.GetStringUpper("StarterPackMTXDiscountButton");
				}
				else if (transaction != null)
				{
					outputStr = UIUtils.FormatLargeNumber(global::Kampai.Game.Transaction.TransactionUtil.GetGrindOutputForTransaction(transaction));
				}
			}
			else if (transaction != null)
			{
				inputStr = global::Kampai.Game.Transaction.TransactionUtil.GetPremiumCostForTransaction(transaction).ToString();
				outputStr = UIUtils.FormatLargeNumber(global::Kampai.Game.Transaction.TransactionUtil.GetGrindOutputForTransaction(transaction));
			}
		}

		private void OnRefreshStore()
		{
			bool forceLocked = AllItemsAreLocked();
			base.view.RefreshButtons(forceLocked, currencyStoreService, localService);
		}

		private void OnStoreDefinitionLoaded(int storeCategoryDefinitionID)
		{
			base.view.ClearViews();
			global::System.Collections.Generic.IList<global::Kampai.Game.CurrencyStoreCategoryDefinition> cats = definitionService.GetCurrencyStoreCategoryDefinitions();
			
			if (cats == null) {
				logger.Error("[SHOPDEBUG] GetCurrencyStoreCategoryDefinitions returned NULL");
				return;
			}

			logger.Warning("[SHOPDEBUG] CATEGORIES COUNT: {0}", cats.Count);
			
			for (int i = 0; i < cats.Count; i++)
			{
				global::Kampai.Game.CurrencyStoreCategoryDefinition cat = cats[i];
				if (cat == null) continue;

				logger.Info("[SHOPDEBUG] Category {0} (ID: {1}) - Items: {2}", i, cat.ID, (cat.StoreItemDefinitionIDs != null) ? cat.StoreItemDefinitionIDs.Count : 0);
				
				if (cat.StoreItemDefinitionIDs == null) continue;

				base.view.viewCounts.Add(cat.StoreItemDefinitionIDs.Count);
				global::System.Collections.Generic.List<global::Kampai.Game.StoreItemDefinition> list = new global::System.Collections.Generic.List<global::Kampai.Game.StoreItemDefinition>();
				for (int j = 0; j < cat.StoreItemDefinitionIDs.Count; j++)
				{
					int id = cat.StoreItemDefinitionIDs[j];
					global::Kampai.Game.StoreItemDefinition itemDef;
					if (definitionService.TryGet<global::Kampai.Game.StoreItemDefinition>(id, out itemDef))
					{
						if (currencyStoreService.IsValidCurrencyItem(id, cat.StoreCategoryType))
						{
							list.Add(itemDef);
						}
					}
					else
					{
						logger.Error("[SHOPDEBUG] Missing StoreItemDef ID: {0}", id);
					}
				}
				
				logger.Info("[SHOPDEBUG] Category {0} validated {1} items.", cat.ID, list.Count);

				if (list.Count <= 0)
				{
					continue;
				}
				global::Kampai.UI.View.CurrencyStoreCategoryButtonView currencyStoreCategoryButtonView = base.view.BuildCategoryButton(cat);
				int badgeCount = 0;
				if (!AllItemsAreLocked() && cat.ID != storeCategoryDefinitionID)
				{
					badgeCount = currencyStoreService.GetBadgeCount(cat);
				}
				currencyStoreCategoryButtonView.SetBadgeCount(badgeCount);
				currencyStoreCategoryButtonView.ClickedSignal.AddListener(OnCategoryButtonClicked);
				global::Kampai.UI.View.CurrencyStoreCategoryView categoryView = base.view.BuildCategoryContainer(cat, currencyStoreCategoryButtonView);
				foreach (global::Kampai.Game.StoreItemDefinition item in list)
				{
					global::Kampai.Game.CurrencyItemDefinition currencyItemDefinition = definitionService.Get<global::Kampai.Game.CurrencyItemDefinition>(item.ReferencedDefID);
					global::Kampai.Game.PremiumCurrencyItemDefinition premium = currencyItemDefinition as global::Kampai.Game.PremiumCurrencyItemDefinition;
					global::Kampai.Game.Transaction.TransactionDefinition definition;
					definitionService.TryGet<global::Kampai.Game.Transaction.TransactionDefinition>(item.TransactionID, out definition);
					string inputStr = string.Empty;
					string outputStr = string.Empty;
					GetStrings(item, premium, definition, ref inputStr, ref outputStr);
					base.view.BuildCategoryItem(currencyItemDefinition, item, inputStr, outputStr, categoryView, true);
				}
			}
			OnRefreshStore();
		}

		private void OnCategoryButtonClicked(global::Kampai.Game.CurrencyStoreCategoryDefinition storeCategoryDefinition)
		{
			base.view.ShowCategory(storeCategoryDefinition.ID);
			currencyStoreService.MarkCategoryAsViewed(storeCategoryDefinition);
		}

		private void OnPremiumCatalogUpdated()
		{
			base.view.OnPremiumCatalogUpdated(currencyService, definitionService);
		}

		private void FTUELevelChanged()
		{
			bool forceLocked = AllItemsAreLocked();
			base.view.RefreshButtons(forceLocked, currencyStoreService, localService);
		}

		private bool AllItemsAreLocked()
		{
			return false; // Force unlocked
		}

		private void OnMenuClose()
		{
			currencyDialogClosedSignal.Dispatch();
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_Store");
		}
	}
}
