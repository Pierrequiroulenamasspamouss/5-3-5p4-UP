namespace Kampai.UI.View
{
	public class StorageBuildingWayFinderMediator : global::Kampai.UI.View.AbstractWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.StorageBuildingWayFinderView StorageBuildingWayFinderView { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService MarketplaceService { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return StorageBuildingWayFinderView;
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();
			if (MarketplaceService.AreThereSoldItems())
			{
				StorageBuildingWayFinderView.UpdateIcon(base.DefinitionService.Get<global::Kampai.Game.WayFinderDefinition>(1000008086).MarketplaceSoldIcon);
			}
		}
	}
}
