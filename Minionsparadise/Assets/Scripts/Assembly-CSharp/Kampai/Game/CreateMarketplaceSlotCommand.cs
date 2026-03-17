namespace Kampai.Game
{
	public class CreateMarketplaceSlotCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CreateMarketplaceSlotCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject]
		public global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType slotType { get; set; }

		public override void Execute()
		{
			int num = (int)(1000008094 + slotType);
			global::Kampai.Game.MarketplaceSaleSlotDefinition marketplaceSaleSlotDefinition = definitionService.Get<global::Kampai.Game.MarketplaceSaleSlotDefinition>(num);
			if (marketplaceSaleSlotDefinition == null)
			{
				logger.Error("Unable to get marketplace slot definition: {0}", num);
				return;
			}
			global::Kampai.Game.MarketplaceSaleSlot marketplaceSaleSlot = new global::Kampai.Game.MarketplaceSaleSlot(marketplaceSaleSlotDefinition);
			marketplaceSaleSlot.state = global::Kampai.Game.MarketplaceSaleSlot.State.UNLOCKED;
			if (slotType == global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType.FACEBOOK_UNLOCKABLE && !facebookService.isLoggedIn)
			{
				marketplaceSaleSlot.state = global::Kampai.Game.MarketplaceSaleSlot.State.LOCKED;
			}
			else if (slotType == global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType.PREMIUM_UNLOCKABLE)
			{
				marketplaceSaleSlot.state = global::Kampai.Game.MarketplaceSaleSlot.State.LOCKED;
				marketplaceSaleSlot.premiumCost = MarketplaceUtil.GetPremiumSlotCost(definitionService, playerService);
			}
			playerService.Add(marketplaceSaleSlot);
		}
	}
}
