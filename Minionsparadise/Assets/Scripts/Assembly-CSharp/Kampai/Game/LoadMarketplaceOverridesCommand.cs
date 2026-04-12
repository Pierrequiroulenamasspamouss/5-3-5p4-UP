namespace Kampai.Game
{
	public class LoadMarketplaceOverridesCommand : global::strange.extensions.command.impl.Command
	{
		public class MarketplaceOverrides
		{
			public global::Kampai.Game.MarketplaceDefinition marketplaceDefinition;
		}

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadMarketplaceOverridesCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			if (!marketplaceService.isServerKillSwitchEnabled && !coppaService.Restricted())
			{
				logger.Info("Loading marketplace overrides");
				global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
				signal.AddListener(OnDownloadComplete);
				string serverUrl = global::Kampai.Util.GameConstants.Marketplace.OVERRIDES_SERVER;
				if (string.IsNullOrEmpty(serverUrl))
				{
					logger.Error("Marketplace overrides: server URL is null or empty");
					return;
				}
				string uri = string.Format("{0}/{1}/{2}", serverUrl.TrimEnd('/'), "marketplace", "marketplace.json");
				downloadService.Perform(requestFactory.Resource(uri).WithResponseSignal(signal));
			}
		}

		private void OnDownloadComplete(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			if (response.Success)
			{
				try
				{
					global::Kampai.Game.LoadMarketplaceOverridesCommand.MarketplaceOverrides marketplaceOverrides = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Game.LoadMarketplaceOverridesCommand.MarketplaceOverrides>(response.Body);
					if (marketplaceOverrides != null)
					{
						global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = marketplaceOverrides.marketplaceDefinition;
						if (marketplaceDefinition != null)
						{
							foreach (global::Kampai.Game.MarketplaceItemDefinition itemDefinition2 in marketplaceDefinition.itemDefinitions)
							{
								global::Kampai.Game.MarketplaceItemDefinition itemDefinition;
								if (marketplaceService.GetItemDefinitionByItemID(itemDefinition2.ItemID, out itemDefinition))
								{
									itemDefinition.StartingStrikePrice = itemDefinition2.StartingStrikePrice;
									itemDefinition.MaxStrikePrice = itemDefinition2.MaxStrikePrice;
									itemDefinition.MinStrikePrice = itemDefinition2.MinStrikePrice;
									itemDefinition.FloorPrice = itemDefinition2.FloorPrice;
									itemDefinition.CeilingPrice = itemDefinition2.CeilingPrice;
									itemDefinition.ProbabilityWeight = itemDefinition2.ProbabilityWeight;
									itemDefinition.HighPriceBuyTimeSeconds = itemDefinition2.HighPriceBuyTimeSeconds;
									itemDefinition.LowPriceBuyTimeSeconds = itemDefinition2.LowPriceBuyTimeSeconds;
									itemDefinition.PriceTrend = itemDefinition2.PriceTrend;
								}
								else
								{
									logger.Info("Marketplace overrides: invalid itemID downloaded: {0}", itemDefinition2.ItemID.ToString());
								}
							}
							return;
						}
					}
					return;
				}
				catch (global::Newtonsoft.Json.JsonSerializationException ex)
				{
					logger.Info("Marketplace overrides JsonSerializationException: {0}", ex.Message);
					return;
				}
				catch (global::Newtonsoft.Json.JsonReaderException ex2)
				{
					logger.Info("Marketplace overrides JsonReaderException: {0}", ex2.Message);
					return;
				}
			}
			logger.Error("Error downloading marketplace overrides");
		}
	}
}
