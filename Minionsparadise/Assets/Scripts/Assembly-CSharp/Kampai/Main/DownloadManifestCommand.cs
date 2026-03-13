namespace Kampai.Main
{
	public class DownloadManifestCommand : global::strange.extensions.command.impl.Command
	{
		private const string REST_DLC_MANIFEST = "manifests";

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DownloadManifestCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IConfigurationsService configService { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Util.IInvokerService invoker { get; set; }

		[Inject]
		public global::Kampai.Common.PostDownloadManifestSignal postSignal { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			global::Kampai.Util.TimeProfiler.StartSection("retrieve manifest");
			logger.EventStart("DownloadManifestCommand.Execute");
			string text = dlcService.GetDownloadQualityLevel();
			if (string.IsNullOrEmpty(text))
			{
				text = global::Kampai.Util.TargetPerformance.LOW.ToString().ToLower();
			}
			global::Kampai.Game.ConfigurationDefinition configurations = configService.GetConfigurations();
			logger.Debug("Downloading {0} Manifest ", text);
			logger.Debug(configurations.ToString());
			if (configurations.dlcManifests == null || configurations.dlcManifests.Count == 0)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.GS_ERROR_MISSING_DLC);
				return;
			}
			string text2 = configService.GetConfigurations().dlcManifests[text];
			bool flag = false;
#if !UNITY_WEBPLAYER
			if (global::System.IO.File.Exists(global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH))
#else
			if (false)
#endif
			{
				try
				{
					global::Kampai.Util.ManifestObject manifestObject = global::Kampai.Util.FastJSONDeserializer.DeserializeFromFile<global::Kampai.Util.ManifestObject>(global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH);
					if (!text2.Contains("manifests") || !text2.StartsWith("http"))
					{
						logger.Fatal(global::Kampai.Util.FatalCode.GS_ERROR_BAD_MANIFEST, text2);
						return;
					}
					string text3 = text2.Substring(text2.LastIndexOf('/') + 1, text2.LastIndexOf('.') - text2.LastIndexOf('/') - 1);
					logger.Debug("MANIFEST: {0}, existing: {1}", text3, manifestObject.id);
					if (text3 == manifestObject.id)
					{
						flag = true;
					}
				}
				catch (global::System.Exception ex)
				{
					logger.Debug("Exception in deserializing manifest = {0}", ex.ToString());
				}
			}
			if (flag)
			{
				logger.Debug("Manifest Exists No need to download it");
				postSignal.Dispatch();
			}
			else
			{
				logger.Debug("requested manifest does not exist, fetching it");
				global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
				signal.AddListener(ReceivedManifestCallback);
				downloadService.Perform(requestFactory.Resource(text2).WithOutputFile(global::Kampai.Util.GameConstants.RESOURCE_MANIFEST_PATH).WithGZip(true)
					.WithResponseSignal(signal)
					.WithRetry());
			}
			logger.EventStop("DownloadManifestCommand.Execute");
		}

		private void ReceivedManifestCallback(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			invoker.Add(delegate
			{
				if (!response.Success)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.GS_ERROR_DOWNLOAD_MANIFEST, "Unable to download manifest");
				}
				else
				{
					postSignal.Dispatch();
				}
			});
		}
	}
}
