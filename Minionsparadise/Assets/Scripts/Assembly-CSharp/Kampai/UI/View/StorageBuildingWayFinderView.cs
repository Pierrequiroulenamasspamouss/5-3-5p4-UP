namespace Kampai.UI.View
{
	public class StorageBuildingWayFinderView : global::Kampai.UI.View.AbstractWayFinderView
	{
		protected override string UIName
		{
			get
			{
				return "StorageBuildingWayFinder";
			}
		}

		protected override string WayFinderDefaultIcon
		{
			get
			{
				return wayFinderDefinition.StorageBuildingDefaultIcon;
			}
		}

		public void SetIconToDefault()
		{
			UpdateIcon(WayFinderDefaultIcon);
		}

		public void SetIconToItemSold()
		{
			UpdateIcon(wayFinderDefinition.MarketplaceSoldIcon);
		}

		protected override bool OnCanUpdate()
		{
			if (zoomCameraModel.ZoomedIn)
			{
				return false;
			}
			return true;
		}
	}
}
