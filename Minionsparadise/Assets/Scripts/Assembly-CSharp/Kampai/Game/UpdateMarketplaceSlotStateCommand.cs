namespace Kampai.Game
{
	public class UpdateMarketplaceSlotStateCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService socialService { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			if (socialService.type != global::Kampai.Game.SocialServices.FACEBOOK)
			{
				return;
			}
			global::Kampai.Game.MarketplaceSaleSlot.State state = ((socialService.isLoggedIn || marketplaceService.DebugFacebook) ? global::Kampai.Game.MarketplaceSaleSlot.State.UNLOCKED : global::Kampai.Game.MarketplaceSaleSlot.State.LOCKED);
			global::System.Collections.Generic.ICollection<global::Kampai.Game.MarketplaceSaleSlot> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.MarketplaceSaleSlot>(1000008095);
			foreach (global::Kampai.Game.MarketplaceSaleSlot item in byDefinitionId)
			{
				item.state = state;
			}
		}
	}
}
