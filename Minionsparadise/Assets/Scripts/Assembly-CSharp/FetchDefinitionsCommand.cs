public class FetchDefinitionsCommand : global::strange.extensions.command.impl.Command
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("FetchDefinitionsCommand") as global::Kampai.Util.IKampaiLogger;

	private global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> downloadResponseSignal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();

	private string definitionPath;

	[Inject]
	public global::Kampai.Game.ConfigurationDefinition config { get; set; }

	[Inject]
	public global::Kampai.Splash.IDownloadService downloadService { get; set; }

	[Inject]
	public ILocalPersistanceService localPersistanceService { get; set; }

	[Inject]
	public global::Kampai.Game.DefinitionsFetchedSignal definitionsFetchedSignal { get; set; }

	[Inject]
	public global::Kampai.Game.LoadDefinitionsSignal loadDefinitionsSignal { get; set; }

	[Inject]
	public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

	[Inject]
	public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

	[Inject]
	public IResourceService resourceService { get; set; }

	public override void Execute()
	{
		logger.EventStart("FetchDefinitionsCommand.Execute");
		if (global::Kampai.Util.ABTestModel.abtestEnabled && global::Kampai.Util.ABTestModel.definitionURL != null)
		{
			config.definitions = global::Kampai.Util.ABTestModel.definitionURL;
		}
		definitionPath = GetDefinitionsPath();

		if (userSessionService.IsOffline)
		{
			logger.Info("[OfflineMode] Authoritative definitions check starting...");
			string text = resourceService.LoadText("definitions");
			if (!string.IsNullOrEmpty(text))
			{
				logger.Info("[OfflineMode] Authoritative definitions.json loaded from resources (Length: " + text.Length + ")");
				try
				{
					global::System.IO.File.WriteAllText(definitionPath, text);
					logger.Info("[OfflineMode] Successfully updated local cache at " + definitionPath);
				}
				catch (global::System.Exception ex)
				{
					logger.Error("[OfflineMode] Failed to update local cache: " + ex.Message);
				}
			}
			else if (!global::System.IO.File.Exists(definitionPath))
			{
				logger.Fatal(global::Kampai.Util.FatalCode.GS_ERROR_FETCH_DEFINITIONS, "[OfflineMode] No definitions found in resources or cache!");
			}
			else
			{
				logger.Warning("[OfflineMode] Resources version missing, falling back to cache at " + definitionPath);
			}

			definitionsFetchedSignal.Dispatch();
			LoadDefinitionsCommand.LoadDefinitionsData loadDefinitionsData = new LoadDefinitionsCommand.LoadDefinitionsData();
			loadDefinitionsData.Path = definitionPath;
			loadDefinitionsSignal.Dispatch(false, loadDefinitionsData);
			return;
		}

		downloadResponseSignal.AddListener(DownloadResponseHandler);
		logger.Error("FetchDefinitionsCommand:: Definitions URL: {0}", config.definitions);
		downloadService.Perform(requestFactory.Resource(config.definitions).WithOutputFile(definitionPath).WithGZip(true)
			.WithResponseSignal(downloadResponseSignal)
			.WithRetry()
			.WithAvoidBackup(true));
		logger.Debug("FetchDefinitionsCommand:Execute config.definitions=" + config.definitions);
	}

	public static string GetDefinitionsPath()
	{
		return global::System.IO.Path.Combine(global::Kampai.Util.GameConstants.PERSISTENT_DATA_PATH, "definitions.json");
	}

	private void DownloadResponseHandler(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
	{
		logger.Debug("FetchDefinitionsCommand:DownloadResponseHandler received response");
		if (!response.Success)
		{
			logger.FatalNoThrow(global::Kampai.Util.FatalCode.GS_ERROR_FETCH_DEFINITIONS, "GET {0} : status code {1}", response.Request.Uri, response.Code);
			return;
		}
		localPersistanceService.PutData("DefinitionsUrl", response.Request.Uri);
		logger.Debug("Load definitions");
		definitionsFetchedSignal.Dispatch();
		LoadDefinitionsCommand.LoadDefinitionsData loadDefinitionsData = new LoadDefinitionsCommand.LoadDefinitionsData();
		loadDefinitionsData.Path = GetDefinitionsPath();
		loadDefinitionsSignal.Dispatch(false, loadDefinitionsData);
		logger.EventStop("FetchDefinitionsCommand.Execute");
	}
}
