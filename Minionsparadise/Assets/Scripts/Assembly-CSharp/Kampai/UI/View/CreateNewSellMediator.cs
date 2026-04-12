namespace Kampai.UI.View
{
	public class CreateNewSellMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private int m_slotId;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CreateNewSellMediator") as global::Kampai.Util.IKampaiLogger;

		private bool m_isSalePanelOpened;

		[Inject]
		public global::Kampai.UI.View.CreateNewSellView view { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetNewSellItemSignal setNewSellItemSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenCreateNewSalePanelSignal openSalePanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseCreateNewSalePanelSignal CloseCreateNewSalePanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.MarketplaceOpenSalePanelSignal openSaleSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.MarketplaceCloseSalePanelSignal closeSaleSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SelectStorageBuildingItemSignal selectStorageBuildingItemSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal setStorageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displaySignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			FillInColors();
			view.Init();
			closeSaleSignal.AddListener(SalePanelClosed);
			openSaleSignal.AddListener(SalePanelOpened);
			openSalePanelSignal.AddListener(OpenPanel);
			setNewSellItemSignal.AddListener(SetInitialItem);
			view.PutOnSaleButton.ClickedSignal.AddListener(CreateSaleOnClick);
			view.AddItemCountButton.heldDownSignal.AddListener(OnAddItemCountClick);
			view.MinusItemCountButton.heldDownSignal.AddListener(OnMinusItemCountClick);
			view.AddPriceButton.heldDownSignal.AddListener(OnAddPriceClick);
			view.MinusPriceButton.heldDownSignal.AddListener(OnMinusPriceClick);
			view.ResetPriceButton.ClickedSignal.AddListener(OnPriceResetClick);
			view.gameObject.SetActive(false);
			SalePanelOpened(true);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			closeSaleSignal.RemoveListener(SalePanelClosed);
			openSaleSignal.RemoveListener(SalePanelOpened);
			openSalePanelSignal.RemoveListener(OpenPanel);
			setNewSellItemSignal.RemoveListener(SetInitialItem);
			view.PutOnSaleButton.ClickedSignal.RemoveListener(CreateSaleOnClick);
			view.AddItemCountButton.heldDownSignal.RemoveListener(OnAddItemCountClick);
			view.MinusItemCountButton.heldDownSignal.RemoveListener(OnMinusItemCountClick);
			view.AddPriceButton.heldDownSignal.RemoveListener(OnAddPriceClick);
			view.MinusPriceButton.heldDownSignal.RemoveListener(OnMinusPriceClick);
			view.ResetPriceButton.ClickedSignal.RemoveListener(OnPriceResetClick);
		}

		private void FillInColors()
		{
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			if (marketplaceDefinition != null)
			{
				view.sellPriceUpBackgroundColor = marketplaceDefinition.SellPriceUpBackgroundColor.GetColor();
				view.sellPriceUpTextColor = marketplaceDefinition.SellPriceUpTextColor.GetColor();
				view.sellPriceDownBackgroundColor = marketplaceDefinition.SellPriceDownBackgroundColor.GetColor();
				view.sellPriceDownTextColor = marketplaceDefinition.SellPriceDownTextColor.GetColor();
			}
		}

		private bool GetFirstItemInStorage()
		{
			foreach (global::Kampai.Game.Item sellableItem in playerService.GetSellableItems())
			{
				global::Kampai.Game.DynamicIngredientsDefinition dynamicIngredientsDefinition = sellableItem.Definition as global::Kampai.Game.DynamicIngredientsDefinition;
				if (dynamicIngredientsDefinition == null)
				{
					SetInitialItem(sellableItem.Definition.ID);
					if (view.marketplaceDef != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		private global::Kampai.Game.Item GetItem(int id)
		{
			global::Kampai.Game.Item result = null;
			foreach (global::Kampai.Game.Item sellableItem in playerService.GetSellableItems())
			{
				if (sellableItem.Definition.ID != id)
				{
					continue;
				}
				result = sellableItem;
				break;
			}
			return result;
		}

		private global::Kampai.Game.MarketplaceItemDefinition GetMarketplaceItemDef(int itemId)
		{
			global::Kampai.Game.MarketplaceItemDefinition itemDefinition;
			marketplaceService.GetItemDefinitionByItemID(itemId, out itemDefinition);
			return itemDefinition;
		}

		private bool HasOpenSlot()
		{
			return marketplaceService.GetNextAvailableSlot() != null;
		}

		private void OnPriceResetClick()
		{
			SetItem(view.item, true);
		}

		private void SetInitialItem(int id)
		{
			if (view == null)
			{
				return;
			}
			if (id <= 0)
			{
				CloseCreateNewSalePanelSignal.Dispatch();
				return;
			}
			global::Kampai.Game.Item item = null;
			if (m_isSalePanelOpened && HasOpenSlot())
			{
				item = GetItem(id);
				SetItem(item, false);
				openSalePanelSignal.Dispatch(id);
			}
			else
			{
				item = GetItem(id);
				SetItem(item, false);
			}
		}

		private void SetItem(global::Kampai.Game.Item item, bool reset)
		{
			if (item == null)
			{
				logger.Warning("Item is null when trying to set the item for the create new sale panel.");
				return;
			}
			global::Kampai.Game.MarketplaceItemDefinition marketplaceItemDef = GetMarketplaceItemDef(item.Definition.ID);
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			int maxSellQuantity = 0;
			if (marketplaceDefinition != null)
			{
				maxSellQuantity = marketplaceDefinition.MaxSellQuantity;
			}
			global::Kampai.Game.DynamicIngredientsDefinition dynamicIngredientsDefinition = item.Definition as global::Kampai.Game.DynamicIngredientsDefinition;
			if (view != null && dynamicIngredientsDefinition == null && marketplaceItemDef != null)
			{
				int itemCount = view.ItemCount;
				view.SetForSaleItem(marketplaceItemDef, item, maxSellQuantity, reset, coppaService.Restricted());
				if (reset)
				{
					view.ItemCount = itemCount;
				}
			}
			selectStorageBuildingItemSignal.Dispatch(item.ID);
			view.UpdateCountRange();
			view.UpdatePriceRange();
		}

		private void OpenPanel(int slotIndex)
		{
			if (!(view == null) && m_isSalePanelOpened)
			{
				m_slotId = slotIndex;
				if (view.item == null)
				{
					GetFirstItemInStorage();
				}
				soundFXSignal.Dispatch("Play_marketplace_newSale_01");
				view.OpenPanel();
			}
		}

		private void CreateSaleOnClick()
		{
			soundFXSignal.Dispatch("Play_marketplace_putOnSale_01");
			int second = global::UnityEngine.Mathf.FloorToInt(view.Price);
			global::Kampai.Util.Tuple<int, int, int, int> type = new global::Kampai.Util.Tuple<int, int, int, int>(view.marketplaceDef.ID, second, view.ItemCount, m_slotId);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.SellToAISignal>().Dispatch(type);
			if (view.item.Definition.Storable)
			{
				setStorageSignal.Dispatch();
			}
			view.ClosePanel();
			CloseCreateNewSalePanelSignal.Dispatch();
			displaySignal.Dispatch(19000011, false, new global::strange.extensions.signal.impl.Signal<bool>());
		}

		private void OnAddItemCountClick(int delta)
		{
			view.ItemCount += delta;
			view.CheckAmountMaxOutState(view.ItemCount + delta);
		}

		private void OnMinusItemCountClick(int delta)
		{
			view.ItemCount -= delta;
			view.CheckAmountMaxOutState(view.ItemCount - delta);
		}

		private void OnAddPriceClick(int delta)
		{
			view.Price += delta;
			view.CheckPriceMaxOutState(view.Price + delta);
		}

		private void OnMinusPriceClick(int delta)
		{
			view.Price -= delta;
			view.CheckPriceMaxOutState(view.Price - delta);
		}

		private void SalePanelClosed()
		{
			m_isSalePanelOpened = false;
		}

		private void SalePanelOpened(bool isInstant)
		{
			m_isSalePanelOpened = true;
		}
	}
}
