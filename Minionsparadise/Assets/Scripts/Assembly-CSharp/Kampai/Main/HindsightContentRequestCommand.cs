namespace Kampai.Main
{
	public class HindsightContentRequestCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("HindsightContentRequestCommand") as global::Kampai.Util.IKampaiLogger;

		private global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> responseSignal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();

		[Inject]
		public global::Kampai.Main.HindsightCampaignDefinition definition { get; set; }

		[Inject]
		public global::Kampai.Main.IHindsightService hindsightService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Main.HindsightContentPreloadSignal hindsightContentPreloadSignal { get; set; }

		public override void Execute()
		{
			if (definition.Content == null)
			{
				logger.Error("Hindsight Campaign Content is null");
				return;
			}
			if (!HindsightUtil.ValidPlatform(definition))
			{
				logger.Info("Invalid Platform {0} for campaign {1}", definition.Platform, definition.ID);
				return;
			}
			string languageKey = localizationService.GetLanguageKey();
			string contentUri = HindsightUtil.GetContentUri(definition, languageKey);
			if (string.IsNullOrEmpty(contentUri))
			{
				logger.Error("Unable to find uri for language key: {0}", languageKey);
				return;
			}
			string contentCachePath = HindsightUtil.GetContentCachePath(definition, languageKey);
			if (string.IsNullOrEmpty(contentCachePath))
			{
				logger.Error("Invalid cache path for language: {0}", languageKey);
			}
			else
			{
				responseSignal.AddOnce(OnResponse);
				global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request = requestFactory.Resource(contentUri).WithOutputFile(contentCachePath).WithGZip(true)
					.WithUdp(true)
					.WithResume(true)
					.WithRetry()
					.WithResponseSignal(responseSignal);
				downloadService.Perform(request);
			}
		}

		private void OnResponse(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			if (response.Success)
			{
				global::Kampai.Main.HindsightCampaign i = definition.Build() as global::Kampai.Main.HindsightCampaign;
				playerService.Add(i);
				hindsightService.UpdateCache();
				global::Kampai.Main.HindsightCampaign.Scope scope = HindsightUtil.GetScope(definition);
				hindsightContentPreloadSignal.Dispatch(scope);
			}
		}
	}
}
