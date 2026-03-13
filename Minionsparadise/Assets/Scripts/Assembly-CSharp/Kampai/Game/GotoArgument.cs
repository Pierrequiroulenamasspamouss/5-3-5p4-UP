namespace Kampai.Game
{
	public class GotoArgument
	{
		public int BuildingId;

		public int BuildingDefId;

		public int ItemId;

		public bool ForceStore;

		public GotoArgument(int buildingId = -1, int buildingDefId = -1, int itemId = -1, bool forceStore = false)
		{
			BuildingId = buildingId;
			BuildingDefId = buildingDefId;
			ItemId = itemId;
			ForceStore = forceStore;
		}
	}
}
