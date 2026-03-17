namespace Kampai.UI.View
{
	public class StartUpSellImpressionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("StartUpSellImpressionCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int salePackDefID { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenUpSellModalSignal openUpSellModalSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.SalePackDefinition definition;
			if (!definitionService.TryGet<global::Kampai.Game.SalePackDefinition>(salePackDefID, out definition))
			{
				logger.Error("The impression's salePackDefinition is null. returning");
			}
			else
			{
				openUpSellModalSignal.Dispatch(definition, "Impression", false);
			}
		}
	}
}
