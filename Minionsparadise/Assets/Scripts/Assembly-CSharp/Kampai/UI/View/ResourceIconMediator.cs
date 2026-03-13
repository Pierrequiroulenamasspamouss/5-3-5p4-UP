namespace Kampai.UI.View
{
	public class ResourceIconMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ResourceIconMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.ResourceIconView View { get; set; }

		[Inject]
		public global::Kampai.UI.View.UITryHarvestSignal TryHarvestSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HighlightHarvestButtonSignal HighlightHarvestSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveResourceIconSignal RemoveResourceIconSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable GameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService DefinitionService { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.TryCollectLeisurePointsSignal tryCollectPoints { get; set; }

		public override void OnRegister()
		{
			View.Init(GameContext, logger, PlayerService, DefinitionService, positionService, localizationService);
			HighlightHarvestSignal.AddListener(HighlightHarvest);
			View.RemoveResourceIconSignal.AddListener(RemoveResourceIcon);
			View.ClickedSignal.AddListener(ButtonClicked);
		}

		public override void OnRemove()
		{
			HighlightHarvestSignal.RemoveListener(HighlightHarvest);
			View.RemoveResourceIconSignal.RemoveListener(RemoveResourceIcon);
			View.ClickedSignal.RemoveListener(ButtonClicked);
			View.Cleanup();
		}

		private void RemoveResourceIcon()
		{
			RemoveResourceIconSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(View.TrackedId, View.ItemDefID));
		}

		private void ButtonClicked()
		{
			if (View.leisureBuilding != null)
			{
				tryCollectPoints.Dispatch(View.leisureBuilding);
			}
			else
			{
				TryHarvestSignal.Dispatch(View.TrackedId, delegate
				{
				}, false);
			}
		}

		private void HighlightHarvest(bool isHighlighted)
		{
			View.HighlightHarvest(isHighlighted);
		}
	}
}
