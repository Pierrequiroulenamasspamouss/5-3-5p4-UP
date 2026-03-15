namespace Kampai.UI.View
{
	public class HindsightContentMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.HindsightContentView>
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("HindsightContentMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::Kampai.Main.MainElement.UI_GLASSCANVAS)]
		public global::UnityEngine.GameObject glassCanvas { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Main.HindsightContentDismissSignal dismissSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.Main.HindsightCampaign hindsightCampaign = args.Get<global::Kampai.Main.HindsightCampaign>();
			if (hindsightCampaign == null)
			{
				logger.Error("Campaign is null");
				return;
			}
			base.view.definition = hindsightCampaign.Definition;
			base.view.dismissSignal = dismissSignal;
			base.view.hideSkrimSignal = hideSkrimSignal;
			base.view.guiService = guiService;
			base.view.telemetryService = telemetryService;
			base.view.Open(glassCanvas, hindsightCampaign, localizationService.GetLanguageKey());
		}

		protected override void Close()
		{
			base.view.Close(global::Kampai.Main.HindsightCampaign.DismissType.DECLINED);
		}
	}
}
