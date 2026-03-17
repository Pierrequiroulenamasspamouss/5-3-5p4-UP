namespace Kampai.UI.View
{
	public class ResourceIconSettings : global::Kampai.UI.View.WorldToGlassUISettings
	{
		public int ItemDefId { get; private set; }

		public int Count { get; private set; }

		public bool isRare { get; private set; }

		public ResourceIconSettings(int trackedId, int itemDefId, int count, bool isRare = false)
			: base(trackedId)
		{
			ItemDefId = itemDefId;
			Count = count;
			this.isRare = isRare;
		}

		public void SetCount(int count)
		{
			Count = count;
		}
	}
}
