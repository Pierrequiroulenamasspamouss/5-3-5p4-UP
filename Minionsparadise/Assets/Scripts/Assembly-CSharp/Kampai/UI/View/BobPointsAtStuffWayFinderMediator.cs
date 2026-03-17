namespace Kampai.UI.View
{
	public class BobPointsAtStuffWayFinderMediator : global::Kampai.UI.View.AbstractWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.BobPointsAtStuffWayFinderView BobPointsAtStuffWayFinderView { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionService LandExpansionService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionConfigService LandExpansionConfigService { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return BobPointsAtStuffWayFinderView;
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();
			BobPointsAtStuffWayFinderView.Init(LandExpansionConfigService, base.DefinitionService);
		}

		protected override void GoToClicked()
		{
			if (base.pickModel.PanningCameraBlocked || base.lairModel.goingToLair)
			{
				return;
			}
			if (View.IsTargetObjectVisible())
			{
				if (base.PlayerService.HasTargetExpansion())
				{
					global::Kampai.Game.LandExpansionBuilding landExpansionBuilding = LandExpansionService.GetBuildingsByExpansionID(base.PlayerService.GetTargetExpansion())[0];
					base.GameContext.injectionBinder.GetInstance<global::Kampai.Game.SelectLandExpansionSignal>().Dispatch(landExpansionBuilding.ID);
				}
			}
			else
			{
				PanToInstance();
			}
		}
	}
}
