namespace Kampai.Main
{
	public class DisplayHindsightContentCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DisplayHindsightContentCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.HindsightCampaign.Scope scope { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.IHindsightService hindsightService { get; set; }

		public override void Execute()
		{
			global::Kampai.Main.HindsightCampaign cachedContent = hindsightService.GetCachedContent(scope);
			if (cachedContent == null)
			{
				logger.Info("There is no campaign to display right now");
				return;
			}
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "HindsightContentView");
			iGUICommand.Args.Add(cachedContent);
			iGUICommand.darkSkrim = true;
			iGUICommand.skrimScreen = "HindsightContentSkrim";
			guiService.Execute(iGUICommand);
			cachedContent.ViewCount++;
		}
	}
}
