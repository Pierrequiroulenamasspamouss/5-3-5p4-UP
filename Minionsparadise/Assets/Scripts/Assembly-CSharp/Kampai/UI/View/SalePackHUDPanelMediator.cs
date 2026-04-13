namespace Kampai.UI.View
{
	public class SalePackHUDPanelMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		private global::System.Collections.Generic.IList<global::Kampai.Game.Sale> playerSales;

		private global::System.Collections.Generic.IList<global::Kampai.UI.View.SalepackHUDView> playerSaleItems;

		private global::Kampai.Game.StartSaleSignal startSaleSignal;

		private global::Kampai.Game.EndSaleSignal endSaleSignal;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SalePackHUDPanelMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveSalePackSignal removeSalePackSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.StartUpSellImpressionSignal startUpSellImpressionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SalePackHUDPanelView view { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			view.Init();
			removeSalePackSignal.AddListener(RemoveSalePack);
			startSaleSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.StartSaleSignal>();
			endSaleSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.EndSaleSignal>();
			startSaleSignal.AddListener(AddSalePack);
			endSaleSignal.AddListener(RemoveSalePack);
			LoadItems();
		}

		public override void OnRemove()
		{
			removeSalePackSignal.RemoveListener(RemoveSalePack);
			startSaleSignal.RemoveListener(AddSalePack);
			endSaleSignal.RemoveListener(RemoveSalePack);
		}

		internal void LoadItems()
		{
			if (playerSaleItems != null)
			{
				for (int num = playerSaleItems.Count - 1; num >= 0; num--)
				{
					playerSaleItems[num].Close();
					global::UnityEngine.Object.Destroy(playerSaleItems[num].gameObject);
					playerSaleItems.Remove(playerSaleItems[num]);
				}
			}
			playerSales = playerService.GetInstancesByType<global::Kampai.Game.Sale>();
			foreach (global::Kampai.Game.Sale playerSale in playerSales)
			{
				if (playerSale.Started && !playerSale.Purchased && !playerSale.Finished)
				{
					AddSalePack(playerSale);
				}
			}
			UpdateScrollState();
		}

		public void AddSalePack(int instanceID)
		{
			global::Kampai.Game.Sale byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Sale>(instanceID);
			if (byInstanceId != null)
			{
				AddSalePack(byInstanceId, false);
			}
		}

		public void AddSalePack(global::Kampai.Game.Sale salePackItem, bool tryImpression = true)
		{
			// Disable all HUD offers per user request.
			return;
			if (playerSaleItems == null)
			{
				playerSaleItems = new global::System.Collections.Generic.List<global::Kampai.UI.View.SalepackHUDView>();
			}
			if (salePackItem.Definition.Type == global::Kampai.Game.SalePackType.Upsell)
			{
				string localizedKey = salePackItem.Definition.LocalizedKey;
				if (localizedKey == "HolidayOffer" || localizedKey == "PileDiamonds" || localizedKey == "SackDiamonds")
				{
					logger.Info("Skipping HUD upsell for {0} per user request.", localizedKey);
					return;
				}
				global::Kampai.UI.View.SalepackHUDView salepackHUDView = BuildSaleItem(salePackItem);
				salepackHUDView.transform.SetParent(view.scrollList.content, false);
				salepackHUDView.transform.localScale = new global::UnityEngine.Vector3(1f, 1f, 1f);
				if (tryImpression && salePackItem.Definition.Impressions != 0 && salePackItem.Impressions < salePackItem.Definition.Impressions)
				{
					global::Kampai.Game.SalePackDefinition definition = salePackItem.Definition;
					timeEventService.AddEvent(definition.ID, salePackItem.UTCLastImpressionTime, definition.ImpressionInterval, startUpSellImpressionSignal);
				}
				playerSaleItems.Add(salepackHUDView);
				UpdateScrollState();
			}
		}

		public void RemoveSalePack(int instanceID)
		{
			if (playerSaleItems == null)
			{
				return;
			}
			for (int num = playerSaleItems.Count - 1; num >= 0; num--)
			{
				if (playerSaleItems[num].SalePackItem.ID == instanceID)
				{
					if (playerSales != null && playerSales.Contains(playerSaleItems[num].SalePackItem))
					{
						playerSales.Remove(playerSaleItems[num].SalePackItem);
					}
					global::UnityEngine.Object.Destroy(playerSaleItems[num].gameObject);
					playerSaleItems.Remove(playerSaleItems[num]);
				}
			}
			UpdateScrollState();
		}

		public global::Kampai.UI.View.SalepackHUDView BuildSaleItem(global::Kampai.Game.Sale item)
		{
			if (item.Definition == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.EX_NULL_ARG);
			}
			string path = "HUD_Upsell";
			global::UnityEngine.GameObject gameObject = global::Kampai.Util.KampaiResources.Load(path) as global::UnityEngine.GameObject;
			if (gameObject == null)
			{
				logger.Error("Unable to load SalePack HUD prefab.");
				return null;
			}
			global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject);
			global::Kampai.UI.View.SalepackHUDView component = gameObject2.GetComponent<global::Kampai.UI.View.SalepackHUDView>();
			component.SalePackItem = item;
			component.SetupIcon(item.Definition);
			return component;
		}

		private void UpdateScrollState()
		{
			if (playerSaleItems == null || playerSaleItems.Count == 1)
			{
				view.scrollList.enabled = false;
			}
			else
			{
				view.scrollList.enabled = true;
			}
		}
	}
}
