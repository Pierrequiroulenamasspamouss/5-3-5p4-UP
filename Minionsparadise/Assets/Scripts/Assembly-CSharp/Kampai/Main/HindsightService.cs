namespace Kampai.Main
{
	public class HindsightService : global::Kampai.Main.IHindsightService
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("HindsightService") as global::Kampai.Util.IKampaiLogger;

		private bool isInitialized;

		private global::System.Collections.Generic.HashSet<global::Kampai.Main.HindsightCampaignDefinition> campaignCache;

		private global::System.Collections.Generic.List<global::Kampai.Main.HindsightCampaignDefinition> campaignDefinitions;

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionSerivce { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject(global::Kampai.Main.MainElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable mainContext { get; set; }

		[Inject]
		public global::Kampai.Main.HindsightContentRequestSignal contentRequestSignal { get; set; }

		[Inject]
		public global::Kampai.Main.HindsightContentPreloadSignal hindsightContentPreloadSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MainCompleteSignal mainCompleteSignal { get; set; }

		public void Initialize()
		{
			if (coppaService.Restricted())
			{
				logger.Info("Hindsight disabled by COPPA");
				return;
			}
			if (configurationsService.isKillSwitchOn(global::Kampai.Game.KillSwitch.HINDSIGHT))
			{
				logger.Info("Hindsight disabled by kill switch.");
				return;
			}
			campaignDefinitions = definitionSerivce.GetAll<global::Kampai.Main.HindsightCampaignDefinition>();
			if (campaignDefinitions == null)
			{
				logger.Warning("Hindsight Campaign Definitions is undefined (null)");
				return;
			}
			mainCompleteSignal.AddOnce(RegisterAppResume);
			campaignCache = new global::System.Collections.Generic.HashSet<global::Kampai.Main.HindsightCampaignDefinition>();
			isInitialized = true;
			ContentSync();
		}

		public void UpdateCache()
		{
			global::System.Collections.Generic.List<global::Kampai.Main.HindsightCampaign> instancesByType = playerService.GetInstancesByType<global::Kampai.Main.HindsightCampaign>();
			foreach (global::Kampai.Main.HindsightCampaign item in instancesByType)
			{
				if (!campaignCache.Contains(item.Definition))
				{
					campaignCache.Add(item.Definition);
				}
			}
		}

		public global::Kampai.Main.HindsightCampaign GetCachedContent(global::Kampai.Main.HindsightCampaign.Scope scope)
		{
			if (!isInitialized)
			{
				logger.Warning("HindsightService is not initialized");
				return null;
			}
			if (scope == global::Kampai.Main.HindsightCampaign.Scope.unknown)
			{
				logger.Error("Hindsight campaign scope is unknown");
				return null;
			}
			if (coppaService.Restricted())
			{
				logger.Info("Hindsight disabled by COPPA (scope = {0})", scope.ToString());
				return null;
			}
			if (configurationsService.isKillSwitchOn(global::Kampai.Game.KillSwitch.HINDSIGHT))
			{
				logger.Info("Hindsight disabled by kill switch.");
				return null;
			}
			if (campaignDefinitions == null || campaignDefinitions.Count == 0)
			{
				logger.Info("Hindsight campaign definition is undefined or contains no definitions");
				return null;
			}
			global::System.Collections.Generic.List<global::Kampai.Main.HindsightCampaignDefinition> list = campaignDefinitions.FindAll((global::Kampai.Main.HindsightCampaignDefinition c) => c.Scope == scope.ToString());
			if (list == null || list.Count == 0)
			{
				logger.Info("Hindsight campaign scope {0} is not found in definitions.", scope.ToString());
				return null;
			}
			string languageKey = localizationService.GetLanguageKey();
			foreach (global::Kampai.Main.HindsightCampaignDefinition item in list)
			{
				if (!campaignCache.Contains(item))
				{
					continue;
				}
				string contentCachePath = HindsightUtil.GetContentCachePath(item, languageKey);
#if !UNITY_WEBPLAYER
				if (!global::System.IO.File.Exists(contentCachePath))
#else
				if (false)
#endif
				{
					continue;
				}
				int num = timeService.CurrentTime();
				if (item.UTCStartDate <= num && num <= item.UTCEndDate)
				{
					global::Kampai.Main.HindsightCampaign firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Main.HindsightCampaign>(item.ID);
					if (firstInstanceByDefinitionId != null && (item.Limit == -1 || firstInstanceByDefinitionId.ViewCount < item.Limit))
					{
						return firstInstanceByDefinitionId;
					}
				}
			}
			return null;
		}

		private void RegisterAppResume()
		{
			mainContext.injectionBinder.GetInstance<global::Kampai.Common.AppResumeCompletedSignal>().AddListener(ContentSync);
		}

		private void ContentSync()
		{
			if (!isInitialized)
			{
				logger.Error("Hindsight is not initalized while attempting to sync content");
				return;
			}
#if !UNITY_WEBPLAYER
			if (global::System.IO.Directory.Exists(global::Kampai.Util.GameConstants.IMAGE_PATH))
			{
				string languageKey = localizationService.GetLanguageKey();
				string[] files = global::System.IO.Directory.GetFiles(global::Kampai.Util.GameConstants.IMAGE_PATH);
				foreach (string path in files)
				{
					string fileNameWithoutExtension = global::System.IO.Path.GetFileNameWithoutExtension(path);
					if (string.IsNullOrEmpty(fileNameWithoutExtension))
					{
						continue;
					}
					string[] array = fileNameWithoutExtension.Split('_');
					int campaignId;
					if (!int.TryParse(array[0], out campaignId))
					{
						continue;
					}
					global::Kampai.Main.HindsightCampaignDefinition hindsightCampaignDefinition = campaignDefinitions.Find((global::Kampai.Main.HindsightCampaignDefinition c) => c.ID == campaignId);
					if (hindsightCampaignDefinition == null || !languageKey.Equals(array[1]))
					{
						global::System.IO.File.Delete(path);
						global::Kampai.Main.HindsightCampaign firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Main.HindsightCampaign>(campaignId);
						if (firstInstanceByDefinitionId != null)
						{
							playerService.Remove(firstInstanceByDefinitionId);
						}
					}
					else
					{
						campaignCache.Add(hindsightCampaignDefinition);
						global::Kampai.Main.HindsightCampaign.Scope scope = HindsightUtil.GetScope(hindsightCampaignDefinition);
						hindsightContentPreloadSignal.Dispatch(scope);
					}
				}
			}
			foreach (global::Kampai.Main.HindsightCampaignDefinition campaignDefinition in campaignDefinitions)
			{
				if (!campaignCache.Contains(campaignDefinition))
				{
					contentRequestSignal.Dispatch(campaignDefinition);
				}
			}
#endif
		}
	}
}
