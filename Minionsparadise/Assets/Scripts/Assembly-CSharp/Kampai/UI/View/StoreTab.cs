namespace Kampai.UI.View
{
	public class StoreTab
	{
		public string LocalizedName { get; set; }

		public global::Kampai.Game.StoreItemType Type { get; set; }

		public StoreTab(string localizedName, global::Kampai.Game.StoreItemType type)
		{
			LocalizedName = localizedName;
			Type = type;
		}
	}
}
