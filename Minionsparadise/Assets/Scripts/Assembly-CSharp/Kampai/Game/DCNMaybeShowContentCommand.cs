namespace Kampai.Game
{
	public class DCNMaybeShowContentCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DCNMaybeShowContentCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.DCNFeaturedSignal dcnFeaturedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDCNService dcnService { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDCNScreenSignal showDCNScreenSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.DCNBuildingDefinition dCNBuildingDefinition = definitionService.Get<global::Kampai.Game.DCNBuildingDefinition>(3128);
			uint quantity = playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			if (quantity >= dCNBuildingDefinition.UnlockLevel)
			{
				if (configurationService.isKillSwitchOn(global::Kampai.Game.KillSwitch.DCN))
				{
					logger.Info("DCN disabled by killswitch");
				}
				else if (dcnService.HasFeaturedContent())
				{
					showDCNScreenSignal.Dispatch(true);
				}
				else
				{
					dcnFeaturedSignal.Dispatch();
				}
			}
		}
	}
}
