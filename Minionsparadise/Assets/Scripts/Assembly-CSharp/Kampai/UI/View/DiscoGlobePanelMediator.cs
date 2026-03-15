namespace Kampai.UI.View
{
	public class DiscoGlobePanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DiscoGlobePanelMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.DiscoGlobePanelView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDiscoGlobeSignal displayDiscoGlobeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.KillDiscoGlobeSignal killDiscoGlobeSignal { get; set; }

		public global::Kampai.Game.PreLoadPartyAssetsSignal preLoadPartyAssetsSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService player { get; set; }

		public override void OnRegister()
		{
			preLoadPartyAssetsSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.PreLoadPartyAssetsSignal>();
			displayDiscoGlobeSignal.AddListener(DisplayDiscoGlobe);
			preLoadPartyAssetsSignal.AddListener(PreloadDiscoGlobe);
			killDiscoGlobeSignal.AddListener(KillDiscoGlobe);
		}

		public override void OnRemove()
		{
			displayDiscoGlobeSignal.RemoveListener(DisplayDiscoGlobe);
			preLoadPartyAssetsSignal.RemoveListener(PreloadDiscoGlobe);
			killDiscoGlobeSignal.RemoveListener(KillDiscoGlobe);
		}

		private void KillDiscoGlobe()
		{
			view.DestroyDiscoGlobeView();
		}

		private void PreloadDiscoGlobe()
		{
			view.PreLoadDiscoGlobe();
		}

		private void DisplayDiscoGlobe(bool display)
		{
			logger.Debug("Display disco globe: " + display);
			view.DisplayDiscoGlobe(display, player.GetMinionPartyInstance());
		}
	}
}
