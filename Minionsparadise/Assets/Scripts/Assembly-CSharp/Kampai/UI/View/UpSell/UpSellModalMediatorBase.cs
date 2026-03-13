namespace Kampai.UI.View.UpSell
{
	public class UpSellModalMediatorBase<T> : global::Kampai.UI.View.UIStackMediator<T>, global::Kampai.Game.IDefinitionsHotSwapHandler where T : global::Kampai.UI.View.UpSell.UpSellModalView
	{
		private global::Kampai.Game.PackDefinition m_packDefinition;

		private string m_prefabName;

		private bool m_disableSkrimButton;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PurchaseSalePackSignal purchaseSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenUpSellModalSignal openUpSellModalSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IUpsellService upsellService { get; set; }

		[Inject]
		public global::Kampai.Game.EndSaleSignal endSaleSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			m_packDefinition = args.Get<global::Kampai.Game.PackDefinition>();
			T val = base.view;
			val.Init(m_packDefinition, upsellService, playerService, currencyService, definitionService, localizationService, timeEventService);
			m_prefabName = args.Get<string>();
			m_disableSkrimButton = args.Get<bool>();
			if (m_disableSkrimButton)
			{
				base.view.backGroundButton.gameObject.SetActive(false);
			}
			T val2 = base.view;
			val2.Open();
			playSFXSignal.Dispatch("Play_menu_popUp_01");
			telemetryService.Send_Telemetry_EVT_UPSELL(m_packDefinition.SKU, global::Kampai.Common.Service.Telemetry.UpsellStatus.Viewed);
		}

		public override void OnRegister()
		{
			base.OnRegister();
			endSaleSignal.AddListener(SaleEnded);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.purchaseCurrencyButton.ClickedSignal.AddListener(OnPurchaseButtonClicked);
			base.view.backGroundButton.ClickedSignal.AddListener(Close);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			endSaleSignal.RemoveListener(SaleEnded);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.purchaseCurrencyButton.ClickedSignal.RemoveListener(OnPurchaseButtonClicked);
			base.view.backGroundButton.ClickedSignal.RemoveListener(Close);
		}

		protected override void Close()
		{
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			T val = base.view;
			val.Release();
			T val2 = base.view;
			val2.Close();
		}

		private void OnMenuClose()
		{
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, m_prefabName);
		}

		private void SaleEnded(int saleInstance)
		{
			base.view.purchaseCurrencyButton.GetComponent<global::UnityEngine.UI.Button>().enabled = false;
			Close();
		}

		private void OnPurchaseButtonClicked()
		{
			Close();
			purchaseSignal.Dispatch(m_packDefinition.ID);
			telemetryService.Send_Telemetry_EVT_UPSELL(m_packDefinition.SKU, global::Kampai.Common.Service.Telemetry.UpsellStatus.Clicked);
		}

		public virtual void OnDefinitionsHotSwap(global::Kampai.Game.IDefinitionService definitionService)
		{
			if (m_packDefinition != null)
			{
				m_packDefinition = definitionService.Get<global::Kampai.Game.PackDefinition>(m_packDefinition.ID);
				global::Kampai.Game.SalePackDefinition salePackDefinition = m_packDefinition as global::Kampai.Game.SalePackDefinition;
				if (salePackDefinition != null)
				{
					global::Kampai.Game.Sale saleInstanceFromID = upsellService.GetSaleInstanceFromID(playerService.GetInstancesByType<global::Kampai.Game.Sale>(), salePackDefinition.ID);
					saleInstanceFromID.OnDefinitionHotSwap(salePackDefinition);
				}
				Close();
				openUpSellModalSignal.Dispatch(m_packDefinition, m_prefabName, false);
			}
		}
	}
}
