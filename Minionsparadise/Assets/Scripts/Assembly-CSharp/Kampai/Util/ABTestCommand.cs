namespace Kampai.Util
{
	public class ABTestCommand : global::strange.extensions.command.impl.Command
	{
		public class GameMetaData
		{
			public string configurationVariant { get; set; }

			public string definitionId { get; set; }

			public string definitionVariants { get; set; }

			public bool debugConsoleTest { get; set; }
		}

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ABTestCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::Kampai.Main.MainElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable mainContext { get; set; }

		[Inject]
		public global::Kampai.Util.ABTestCommand.GameMetaData defData { get; set; }

		[Inject("cdn.server.host")]
		public string ServerUrl { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		public override void Execute()
		{
			global::Kampai.Util.ABTestModel.resetState();
			global::Kampai.Util.ABTestModel.abtestEnabled = true;
			global::Kampai.Util.ABTestModel.debugConsoleTest = defData.debugConsoleTest;
			if (defData.configurationVariant != null)
			{
				global::Kampai.Util.ABTestModel.configurationVariant = defData.configurationVariant;
			}
			else if (defData.definitionId != null)
			{
				global::Kampai.Util.ABTestModel.definitionURL = string.Format("{0}/rest/def/{1}/definitions", ServerUrl, defData.definitionId);
				if (defData.definitionVariants != null)
				{
					global::Kampai.Util.ABTestModel.definitionVariants = defData.definitionVariants;
					global::Kampai.Util.ABTestModel.definitionURL = string.Format("{0}/rest/def/{1}/definitions/{2}", ServerUrl, defData.definitionId, defData.definitionVariants);
				}
			}
			else if (defData.definitionVariants != null)
			{
				global::Kampai.Util.ABTestModel.definitionVariants = defData.definitionVariants;
				string text = configurationsService.GetConfigurations().definitions;
				if (text != null)
				{
					int num = text.LastIndexOf("/definitions");
					if (num != -1)
					{
						text = text.Substring(0, num);
					}
					global::Kampai.Util.ABTestModel.definitionURL = string.Format("{0}{1}/{2}", text, "/definitions", defData.definitionVariants);
				}
				else
				{
					logger.Error("ABTestCommand: unexpected null definitions in configuration. Skip setting of definitions URL for A/B testing");
				}
			}
			if (defData.debugConsoleTest)
			{
				mainContext.injectionBinder.GetInstance<global::Kampai.Main.ReloadGameSignal>().Dispatch();
			}
		}
	}
}
