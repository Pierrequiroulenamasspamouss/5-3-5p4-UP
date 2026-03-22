namespace Kampai.Game
{
	public class DebugCurrencyService : global::Kampai.Game.CurrencyService
	{
		[Inject]
		public global::Kampai.UI.View.ShowMockStoreDialogSignal showMockStoreDialogSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		private global::System.Collections.Generic.Dictionary<string, string> serverPrices = new global::System.Collections.Generic.Dictionary<string, string>();

		private bool isSyncing = false;

		[PostConstruct]
		public void PostConstruct()
		{
			RefreshCatalog();
		}

		public override void RequestPurchase(global::Kampai.Game.KampaiPendingTransaction item)
		{
			showMockStoreDialogSignal.Dispatch(item);
		}

		public override void RefreshCatalog()
		{
			if (!isSyncing)
			{
				routineRunner.StartCoroutine(SyncPrices());
			}
		}

		private global::System.Collections.IEnumerator SyncPrices()
		{
			isSyncing = true;
			logger.Info("[PRICES] Syncing prices from server...");
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> responseSignal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			responseSignal.AddListener(OnPricesDownloaded);
			
			string url = string.Format("{0}/rest/market_prices", global::Kampai.Util.GameConstants.UpSell.SALES_SERVER);
			downloadService.Perform(requestFactory.Resource(url).WithResponseSignal(responseSignal));
			yield break;
		}

		private void OnPricesDownloaded(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			isSyncing = false;
			if (response.Success)
			{
				try
				{
					using (global::System.IO.StringReader reader = new global::System.IO.StringReader(response.Body))
					{
						using (global::Newtonsoft.Json.JsonTextReader reader2 = new global::Newtonsoft.Json.JsonTextReader(reader))
						{
							global::Newtonsoft.Json.JsonSerializer serializer = new global::Newtonsoft.Json.JsonSerializer();
							serverPrices = serializer.Deserialize<global::System.Collections.Generic.Dictionary<string, string>>(reader2);
							logger.Info("[PRICES] Sync Successful. Found {0} prices.", serverPrices.Count);
						}
					}
				}
				catch (global::System.Exception e)
				{
					logger.Error("[PRICES] Error parsing prices: {0}", e);
				}
			}
			else
			{
				logger.Error("[PRICES] Error downloading prices: {0}", response.Body ?? "null");
			}
		}

		public override string GetPriceWithCurrencyAndFormat(string SKU)
		{
			if (serverPrices != null && serverPrices.ContainsKey(SKU))
			{
				return serverPrices[SKU];
			}

			if (serverPrices != null && serverPrices.ContainsKey("default"))
			{
				return serverPrices["default"];
			}

			switch (SKU)
			{
			case "SKU_FEW_DIAMONDS":
				return "$1.99";
			case "SKU_PILE_DIAMONDS":
				return "$4.99";
			case "SKU_SACK_DIAMONDS":
				return "$9.99";
			case "SKU_BAGS_DIAMONDS":
				return "$19.99";
			case "SKU_CHEST_DIAMONDS":
				return "$39.99";
			case "SKU_BIG_CHEST_DIAMONDS":
				return "$49.00";
			case "SKU_PILE_SAND_DOLLARS":
				return "$0.99";
			case "SKU_BAG_SAND_DOLLARS":
				return "$2.99";
			case "SKU_SACK_SAND_DOLLARS":
				return "$7.99";
			case "SKU_BOX_SAND_DOLLARS":
				return "$14.99";
			case "SKU_CHEST_SAND_DOLLARS":
				return "$29.99";
			case "SKU_TRUNK_SAND_DOLLARS":
				return "$79.00";
			case "SKU_STARTER_PACK":
				return "$5.99";
			case "SKU_MIGNETTE_UNLOCK_1":
				return "$7.99";
			case "SKU_MIGNETTE_UNLOCK_2":
				return "$6.99";
			case "SKU_FREE_REDEMPTION":
				return "$0.00";
			case "SKU_HOLIDAY_OFFER":
				return "$3.99";
			case "SKU_DOUBLE_DRIBBLE":
				return "$9.99";
			case "SKU_DUBS_AND_A_TUB":
				return "$19.99";
			case "SKU_BAZOOKA_STARTER_KIT":
				return "$9.99";
			case "SKU_CHILI_AND_CHILL":
				return "$9.99";
			case "SKU_BEATS_AND_BANANAS":
				return "$14.99";
			case "SKU_LET_THERE_BE_LIGHT":
				return "$14.99";
			case "SKU_SPROING_IN_YOUR_STEP":
				return "$19.99";
			case "SKU_THE_WRITE_STUFF":
				return "$19.99";
			case "SKU_GUN_AND_GAMES":
				return "$9.99";
			case "SKU_HIGH_EATS_FINE_EATS":
				return "$9.99";
			case "SKU_TREATS_AND_TOYS":
				return "$9.99";
			case "SKU_MR_FIX_IT":
				return "$14.99";
			case "SKU_HAPPY_CAPPER":
				return "$14.99";
			case "SKU_DOOHICKEYS":
				return "$14.99";
			case "SKU_TECH_BOOM":
				return "$14.99";
			case "SKU_MORE_TO_STORE":
				return "$1.99";
			case "SKU_EXPAND_AND_EXPLORE":
				return "$1.99";
			default:
				return "$9.99";
			}
		}

		public override void ReceiptValidationCallback(global::Kampai.Game.Mtx.ReceiptValidationResult result)
		{
		}

		public override void PauseTransactionsHandling()
		{
		}

		public override void ResumeTransactionsHandling()
		{
		}

		public override void CollectRedemption(global::Kampai.Game.Mtx.ReceiptValidationResult pendingRedemption)
		{
			PurchaseSucceededAndValidatedCallback(pendingRedemption.sku);
		}

		public override void RestorePurchases()
		{
		}

		public override bool TransactionProcessingEnabled()
		{
			return true;
		}
	}
}
