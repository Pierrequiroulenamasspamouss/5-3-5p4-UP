namespace Kampai.UI.View
{
	public class BuyMarketplaceSlotMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private bool isCOPPAGated;

		[Inject]
		public global::Kampai.UI.View.BuyMarketplaceSlotView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateBuySlotSignal updateBuySlot { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowSocialPartyFBConnectSignal showFacebookPopupSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal tweenSignal { get; set; }

		[Inject]
		public global::Kampai.Game.HaltSlotMachine haltSlotMachine { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketPlaceService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			view.BuyButtonView.ClickedSignal.AddListener(OnBuyButtonClick);
			view.FacebookButtonView.ClickedSignal.AddListener(OnBuyButtonClick);
			isCOPPAGated = coppaService.Restricted();
			view.Init();
			view.SetupBuyItem(localService, definitionService, facebookService, isCOPPAGated, view.BuyItem, soundFXSignal, marketPlaceService);
			updateBuySlot.AddListener(UpdateSlot);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			view.BuyButtonView.ClickedSignal.RemoveListener(OnBuyButtonClick);
			view.FacebookButtonView.ClickedSignal.RemoveListener(OnBuyButtonClick);
			updateBuySlot.RemoveListener(UpdateSlot);
			view.ClearOldTweens();
		}

		private void UpdateSlot(int slotIndex, bool success)
		{
			view.BuyButtonView.ResetAnim();
			if (slotIndex == view.slotIndex)
			{
				if (success)
				{
					view.SetIsSold(true);
					tweenSignal.Dispatch(uiCamera.WorldToScreenPoint(view.ItemImage.transform.position), global::Kampai.UI.View.DestinationType.STORAGE, view.BuyItem.Definition.ItemID, false);
				}
				else
				{
					view.ShakeIcon();
					soundFXSignal.Dispatch("Play_error_button_01");
				}
			}
		}

		private void OnBuyButtonClick()
		{
			if (view.isSlotAnimationPlaying)
			{
				haltSlotMachine.Dispatch();
			}
			else if (view.CurrentState == global::Kampai.UI.View.BuyMarketplaceSlotView.State.Discord)
			{
				facebookService.LoginSource = "Marketplace";
				showFacebookPopupSignal.Dispatch(delegate
				{
				});
			}
			else if (view.BuyButtonView.isDoubleConfirmed())
			{
				soundFXSignal.Dispatch("Play_marketplace_purchased_01");
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.BuyMarketplaceItemSignal>().Dispatch(view.BuyItem, view.slotIndex);
			}
		}
	}
}
