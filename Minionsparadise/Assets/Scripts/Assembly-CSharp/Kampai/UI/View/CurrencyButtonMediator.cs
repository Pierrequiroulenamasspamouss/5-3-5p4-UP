namespace Kampai.UI.View
{
	public class CurrencyButtonMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private bool purchaseOnDisable;

		[Inject]
		public global::Kampai.UI.View.CurrencyButtonView view { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseHUDSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenUpSellModalSignal openUpSellModalSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.CancelPurchaseSignal cancelPurchaseSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CurrencyButtonClickSignal clickedSignal { get; set; }

		public override void OnRegister()
		{
			view.InfoClickedSignal.AddListener(OnInfoClicked);
			view.PurchaseClickedSignal.AddListener(OnPurchaseClicked);
			purchaseOnDisable = false;
		}

		public override void OnRemove()
		{
			view.InfoClickedSignal.RemoveListener(OnInfoClicked);
			view.PurchaseClickedSignal.RemoveListener(OnPurchaseClicked);
		}

		private void OnInfoClicked()
		{
			if (view.PlaySoundOnClick)
			{
				playSFXSignal.Dispatch("Play_button_click_01");
			}
			openUpSellModalSignal.Dispatch(definitionService.Get<global::Kampai.Game.PackDefinition>(view.Definition.ReferencedDefID), "MTXStore", false);
			cancelPurchaseSignal.Dispatch(false);
		}

		private void OnPurchaseClicked()
		{
			if (view.PlaySoundOnClick)
			{
				playSFXSignal.Dispatch("Play_button_click_01");
			}
			purchaseOnDisable = true;
			closeSignal.Dispatch(false);
		}

		private void OnDisable()
		{
			if (purchaseOnDisable)
			{
				purchaseOnDisable = false;
				bool type = false;
				global::Kampai.Game.PackDefinition definition;
				if (definitionService.TryGet<global::Kampai.Game.PackDefinition>(view.Definition.ReferencedDefID, out definition))
				{
					type = definition.DisableDynamicUnlock;
				}
				clickedSignal.Dispatch(view.Definition, type);
			}
		}
	}
}
