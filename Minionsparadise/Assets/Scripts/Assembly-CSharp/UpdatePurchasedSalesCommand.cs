public class UpdatePurchasedSalesCommand : global::strange.extensions.command.impl.Command
{
	private const string RESOURCE = "{0}/rest/sales/sale/{1}/incrementPurchasedSales";

	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UpdatePurchasedSalesCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public string serverSaleId { get; set; }

	[Inject]
	public global::Kampai.Splash.IDownloadService downloadService { get; set; }

	[Inject]
	public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

	[Inject]
	public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

	public override void Execute()
	{
		if (string.IsNullOrEmpty(serverSaleId))
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Unable to update purchased sales: invalid serverSaleId was provided");
			return;
		}
		global::Kampai.Game.UserSession userSession = userSessionService.UserSession;
		string uri = string.Format("{0}/rest/sales/sale/{1}/incrementPurchasedSales", global::Kampai.Util.GameConstants.UpSell.SALES_SERVER, serverSaleId);
		global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback = delegate(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			OnResponse(response, serverSaleId);
		};
		global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
		signal.AddListener(callback);
		downloadService.Perform(requestFactory.Resource(uri).WithMethod("POST").WithHeaderParam("user_id", userSession.UserID)
			.WithHeaderParam("session_key", userSession.SessionID)
			.WithContentType("application/json")
			.WithResponseSignal(signal));
	}

	private void OnResponse(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response, string serverSaleId)
	{
		if (response.Success)
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, string.Format("Updated purchased sales for sale {0}", serverSaleId));
		}
		else
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, string.Format("Unable to update purchased sales for sale {0}: request failed with status code {1}", serverSaleId, response.Code));
		}
	}
}
