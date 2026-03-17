public class LoadConfigurationsCommand : global::strange.extensions.command.impl.Command
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadConfigurationsCommand") as global::Kampai.Util.IKampaiLogger;

	private global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> downloadResponse = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();

	[Inject]
	public bool init { get; set; }

	[Inject]
	public global::Kampai.Splash.IDownloadService downloadService { get; set; }

	[Inject]
	public global::Kampai.Game.IConfigurationsService ConfigurationsService { get; set; }

	[Inject]
	public ILocalPersistanceService localPersistService { get; set; }

	[Inject]
	public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

	public override void Execute()
	{
		logger.EventStart("LoadConfigurationsCommand.Execute");
		logger.Info("Executing LoadConfigurationsCommand:{0}", init);
		global::Kampai.Util.TimeProfiler.StartSection("retrieve config");
		string configURL = ConfigurationsService.GetConfigURL();
		logger.Info("ClientConfigUrl: {0}", configURL);
		localPersistService.PutData("configURL", configURL);
		downloadResponse.AddListener(ConfigurationsService.GetConfigurationCallback);
		ConfigurationsService.setInitonCallback(init);
		downloadService.Perform(requestFactory.Resource(configURL).WithResponseSignal(downloadResponse).WithRetry());
		logger.EventStop("LoadConfigurationsCommand.Execute");
	}
}
