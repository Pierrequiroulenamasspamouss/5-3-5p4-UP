namespace Kampai.Game
{
	public class ReportMarketplaceTransactionCommand : global::strange.extensions.command.impl.Command
	{
		public class Report : global::Kampai.Util.IFastJSONSerializable
		{
			public int itemId;

			public int quantity;

			public int price;

			public bool buyTransaction;

			public void Serialize(global::Newtonsoft.Json.JsonWriter writer)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("itemId");
				writer.WriteValue(itemId);
				writer.WritePropertyName("quantity");
				writer.WriteValue(quantity);
				writer.WritePropertyName("price");
				writer.WriteValue(price);
				writer.WritePropertyName("buyTransaction");
				writer.WriteValue(buyTransaction);
				writer.WriteEndObject();
			}
		}

		[Inject]
		public global::Kampai.Game.Instance<global::Kampai.Game.MarketplaceItemDefinition> item { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		public override void Execute()
		{
			if (!marketplaceService.isServerKillSwitchEnabled && !coppaService.Restricted())
			{
				global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
				global::Kampai.Game.ReportMarketplaceTransactionCommand.Report report = new global::Kampai.Game.ReportMarketplaceTransactionCommand.Report();
				report.itemId = item.Definition.ItemID;
				global::Kampai.Game.MarketplaceBuyItem marketplaceBuyItem = item as global::Kampai.Game.MarketplaceBuyItem;
				if (marketplaceBuyItem != null)
				{
					report.quantity = marketplaceBuyItem.BuyQuantity;
					report.price = marketplaceBuyItem.BuyPrice;
					report.buyTransaction = true;
				}
				global::Kampai.Game.MarketplaceSaleItem marketplaceSaleItem = item as global::Kampai.Game.MarketplaceSaleItem;
				if (marketplaceSaleItem != null)
				{
					report.quantity = marketplaceSaleItem.QuantitySold;
					report.price = marketplaceSaleItem.SalePrice;
					report.buyTransaction = false;
				}
				downloadService.Perform(requestFactory.Resource(global::Kampai.Util.GameConstants.Marketplace.STAT_REPORTING_SERVER + "/rest/marketplace").WithHeaderParam("user_id", userSession.UserID).WithHeaderParam("session_key", userSession.SessionID)
					.WithContentType("application/json")
					.WithMethod("POST")
					.WithEntity(report));
			}
		}
	}
}
