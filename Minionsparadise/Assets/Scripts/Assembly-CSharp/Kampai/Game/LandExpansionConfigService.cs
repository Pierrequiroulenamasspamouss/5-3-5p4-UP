namespace Kampai.Game
{
	public class LandExpansionConfigService : global::Kampai.Game.ILandExpansionConfigService
	{
		private global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.LandExpansionConfig> expansionConfigLookup = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.LandExpansionConfig>();

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public global::System.Collections.Generic.IList<int> GetExpansionIds()
		{
			TryInitialize();
			return new global::System.Collections.Generic.List<int>(expansionConfigLookup.Keys);
		}

		public global::Kampai.Game.LandExpansionConfig GetExpansionConfig(int expansion)
		{
			TryInitialize();
			global::Kampai.Game.LandExpansionConfig value = null;
			expansionConfigLookup.TryGetValue(expansion, out value);
			return value;
		}

		private void TryInitialize()
		{
			if (expansionConfigLookup.Count != 0)
			{
				return;
			}
			foreach (global::Kampai.Game.LandExpansionConfig item in definitionService.GetAll<global::Kampai.Game.LandExpansionConfig>())
			{
				expansionConfigLookup.Add(item.expansionId, item);
			}
		}
	}
}
