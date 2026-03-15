namespace Kampai.UI.View
{
	public class OpenStoreHighlightItemCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int defId { get; set; }

		[Inject]
		public bool openStore { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService service { get; set; }

		[Inject]
		public global::Kampai.UI.View.MoveBuildMenuSignal moveBuildMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HighlightStoreItemSignal highlightStoreItemSignal { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.StoreItemDefinition> all = service.GetAll<global::Kampai.Game.StoreItemDefinition>();
			foreach (global::Kampai.Game.StoreItemDefinition item in all)
			{
				if (item.ReferencedDefID == defId)
				{
					if (openStore)
					{
						moveBuildMenuSignal.Dispatch(true);
						showStoreSignal.Dispatch(true);
						highlightStoreItemSignal.Dispatch(item, global::Kampai.UI.View.HighlightType.DRAG);
					}
					break;
				}
			}
		}
	}
}
