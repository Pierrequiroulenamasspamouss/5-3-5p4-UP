namespace Kampai.UI.View
{
	public class CraftingModalParams
	{
		public int itemId { get; set; }

		public bool highlight { get; set; }

		public void Clear()
		{
			itemId = 0;
		}
	}
}
