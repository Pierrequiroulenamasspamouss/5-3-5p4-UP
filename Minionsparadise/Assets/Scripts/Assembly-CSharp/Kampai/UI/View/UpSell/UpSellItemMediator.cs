namespace Kampai.UI.View.UpSell
{
	public class UpSellItemMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UpSellItemMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.UpSell.UpSellItemView view { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			view.Init(localizationService, fancyUIService, definitionService, logger, moveAudioListenerSignal);
		}
	}
}
