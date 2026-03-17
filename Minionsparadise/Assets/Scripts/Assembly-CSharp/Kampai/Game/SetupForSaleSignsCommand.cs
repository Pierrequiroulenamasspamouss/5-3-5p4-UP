namespace Kampai.Game
{
	public class SetupForSaleSignsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.ILandExpansionService landExpansionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateForSaleSignSignal createSignal { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.List<int> list = landExpansionService.GetAllExpansionIDs() as global::System.Collections.Generic.List<int>;
			global::Kampai.Game.PurchasedLandExpansion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			foreach (int item in list)
			{
				bool type = false;
				if (byInstanceId.IsUnpurchasedAdjacentExpansion(item) && !landExpansionService.IsLevelGated(item, quantity))
				{
					type = true;
				}
				createSignal.Dispatch(item, type);
			}
		}
	}
}
