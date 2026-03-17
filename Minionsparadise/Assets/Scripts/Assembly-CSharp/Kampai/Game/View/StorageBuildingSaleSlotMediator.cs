namespace Kampai.Game.View
{
	public class StorageBuildingSaleSlotMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("StorageBuildingSaleSlotMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.StorageBuildingSaleSlotView view { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowSocialPartyFBConnectSignal showFacebookPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SocialLoginSuccessSignal loginSuccess { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenCreateNewSalePanelSignal createNewSalePanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateSaleSlotSignal updateSaleSlot { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateSaleSlotsStateSignal updateSaleSlotState { get; set; }

		[Inject]
		public global::Kampai.Game.CollectMarketplaceSaleSignal collectSaleSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PurchaseMarketplaceSlotSignal purchaseSlotSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshSlotsSignal refreshSlotsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateMarketplaceSlotStateSignal updateSlotStateSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseCreateNewSalePanelSignal closeCreateNewSaleSignal { get; set; }

		public override void OnRegister()
		{
			view.Init(localizationService, definitionService.Get<global::Kampai.Game.MarketplaceDefinition>().SaleCancellationCost);
			updateSaleSlotState.AddListener(CheckIfNoValidItems);
			view.CheckIfValidItemsSignal.AddListener(CheckIfNoValidItems);
			view.CreateButtonView.ClickedSignal.AddListener(CreateNewSell);
			view.CollectButtonView.ClickedSignal.AddListener(CollectSale);
			view.FacebookButtonView.ClickedSignal.AddListener(FacebookButton);
			view.PremiumButtonView.ClickedSignal.AddListener(PurchaseSlot);
			view.PendingPanel.ClickedSignal.AddListener(FlipButton);
			view.CancelPendingButtonView.ClickedSignal.AddListener(CancelSaleButton);
			loginSuccess.AddListener(OnLoginSuccess);
			updateSaleSlot.AddListener(UpdateView);
			closeCreateNewSaleSignal.AddListener(CloseCreateNewSalePanel);
			updateSlotStateSignal.Dispatch();
			UpdateView(view.slotId);
		}

		public override void OnRemove()
		{
			updateSaleSlotState.RemoveListener(CheckIfNoValidItems);
			view.CheckIfValidItemsSignal.RemoveListener(CheckIfNoValidItems);
			view.CreateButtonView.ClickedSignal.RemoveListener(CreateNewSell);
			view.CollectButtonView.ClickedSignal.RemoveListener(CollectSale);
			view.FacebookButtonView.ClickedSignal.RemoveListener(FacebookButton);
			view.PremiumButtonView.ClickedSignal.RemoveListener(PurchaseSlot);
			view.PendingPanel.ClickedSignal.RemoveListener(FlipButton);
			view.CancelPendingButtonView.ClickedSignal.RemoveListener(CancelSaleButton);
			loginSuccess.RemoveListener(OnLoginSuccess);
			updateSaleSlot.RemoveListener(UpdateView);
			closeCreateNewSaleSignal.RemoveListener(CloseCreateNewSalePanel);
		}

		private void UpdateView(int slotId)
		{
			if (slotId != view.slotId)
			{
				return;
			}
			global::Kampai.Game.MarketplaceSaleSlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleSlot>(view.slotId);
			if (byInstanceId == null)
			{
				return;
			}
			global::Kampai.Game.MarketplaceSaleItem byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleItem>(byInstanceId.itemId);
			view.UpdateSlot(byInstanceId);
			if (byInstanceId2 != null)
			{
				global::Kampai.Util.QuantityItem quantityItem = MarketplaceUtil.GetQuantityItem(definitionService, byInstanceId2.Definition);
				int rewardValue = MarketplaceUtil.GetRewardValue(quantityItem, byInstanceId2);
				global::Kampai.Game.ItemDefinition quantityItemDef = definitionService.Get<global::Kampai.Game.ItemDefinition>(quantityItem.ID);
				global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(byInstanceId2.Definition.ItemID);
				if (byInstanceId.Definition.type != global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType.FACEBOOK_UNLOCKABLE || facebookService.isLoggedIn || marketplaceService.DebugFacebook)
				{
					view.UpdateItem(byInstanceId2, itemDefinition, quantityItemDef, rewardValue);
				}
				view.EnableDebugTimer(marketplaceService.IsDebugMode, byInstanceId2.LengthOfSale + byInstanceId2.SaleStartTime - timeService.CurrentTime());
			}
		}

		private void CollectSale()
		{
			global::Kampai.Game.TransactionArg transactionArg = new global::Kampai.Game.TransactionArg();
			transactionArg.fromGlass = true;
			transactionArg.StartPosition = uiCamera.WorldToScreenPoint(view.CollectButtonView.gameObject.transform.position);
			transactionArg.Source = "Marketplace";
			collectSaleSignal.Dispatch(view.slotId, transactionArg);
		}

		private void FlipButton()
		{
			view.Flip(soundFXSignal);
		}

		private void CancelSaleButton()
		{
			global::Kampai.Game.MarketplaceSaleSlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleSlot>(view.slotId);
			if (byInstanceId == null)
			{
				return;
			}
			global::Kampai.Game.MarketplaceSaleItem byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleItem>(byInstanceId.itemId);
			if (byInstanceId2 != null)
			{
				if (view.CancelPendingButtonView.isDoubleConfirmed())
				{
					soundFXSignal.Dispatch("Play_delete_ticket_01");
					gameContext.injectionBinder.GetInstance<global::Kampai.Game.CancelMarketPlaceSaleSignal>().Dispatch(byInstanceId2.ID);
				}
			}
			else
			{
				logger.Error("Failed to find the pending item");
			}
		}

		private void FacebookButton()
		{
			SocialButton();
		}

		private void PurchaseSlot()
		{
			if (view.PremiumButtonView.isDoubleConfirmed())
			{
				purchaseSlotSignal.Dispatch(view.slotId);
			}
		}

		private void SocialButton()
		{
			facebookService.LoginSource = "Marketplace";
			showFacebookPopupSignal.Dispatch(delegate
			{
			});
		}

		private void OnLoginSuccess(global::Kampai.Game.ISocialService socialService)
		{
			global::Kampai.Game.MarketplaceSaleSlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MarketplaceSaleSlot>(view.slotId);
			if (socialService.type == global::Kampai.Game.SocialServices.FACEBOOK && byInstanceId.Definition.type == global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType.FACEBOOK_UNLOCKABLE)
			{
				UpdateView(byInstanceId.ID);
				refreshSlotsSignal.Dispatch(false);
			}
		}

		private void CreateNewSell()
		{
			createNewSalePanelSignal.Dispatch(view.slotId);
		}

		private void CheckIfNoValidItems()
		{
			global::UnityEngine.UI.Button component = view.CreateButtonView.GetComponent<global::UnityEngine.UI.Button>();
			if (component != null)
			{
				component.interactable = marketplaceService.AreThereValidItemsInStorage();
			}
		}

		private void CloseCreateNewSalePanel()
		{
			view.PremiumButtonView.ResetTapState();
		}
	}
}
