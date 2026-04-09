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
			logger.Info("[OfflineMode] FetchDefinitionsCommand skipping download in offline mode.");
			if (!global::System.IO.File.Exists(definitionPath))
			{
				logger.Info("[OfflineMode] Cached definitions not found at {0}, checking resources...", definitionPath);
				string text = resourceService.LoadText("definitions");
				if (string.IsNullOrEmpty(text))
				{
					// Try with .json extension just in case, though usually not needed for Resources.Load
					text = resourceService.LoadText("definitions.json");
				}

				if (!string.IsNullOrEmpty(text))
				{
					logger.Info("[OfflineMode] Successfully loaded definitions from resources, writing to {0}", definitionPath);
					string directoryName = global::System.IO.Path.GetDirectoryName(definitionPath);
					if (!global::System.IO.Directory.Exists(directoryName))
					{
						global::System.IO.Directory.CreateDirectory(directoryName);
					}
					global::System.IO.File.WriteAllText(definitionPath, text);
				}
				else
				{
					logger.Error("[OfflineMode] FAILED to load definitions from resources 'definitions'!");
				}
			}
			else
			{
				logger.Info("[OfflineMode] Found cached definitions at {0}", definitionPath);
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
		return global::Kampai.Util.OfflineModeUtility.DefinitionsCachePath;
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
