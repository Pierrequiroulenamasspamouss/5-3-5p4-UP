namespace Kampai.Game
{
	public class CheckMinionPartyLevelCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.AwardLevelSignal awardLevelSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPartyService partyService { get; set; }

		[Inject]
		public bool IsPartyStart { get; set; }

		public override void Execute()
		{
			if (!playerService.IsMinionPartyUnlocked())
			{
				return;
			}
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_PARTY_INDEX_ID);
			int newIndex = quantity;
			int quantity2 = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			int newLevel = quantity2;
			int quantity3 = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.XP_ID);
			int newPoints = quantity3;
			if (IsPartyStart)
			{
				partyService.GetNewLevelIndexAndPointsAfterParty(quantity2, quantity, quantity3, out newLevel, out newIndex, out newPoints);
				int amount = (quantity - newIndex) * -1;
				playerService.AlterQuantity(global::Kampai.Game.StaticItem.LEVEL_PARTY_INDEX_ID, amount);
				int amount2 = (quantity3 - newPoints) * -1;
				playerService.AlterQuantity(global::Kampai.Game.StaticItem.XP_ID, amount2);
				if (quantity2 != newLevel)
				{
					gameContext.injectionBinder.GetInstance<global::Kampai.Game.LevelUpSignal>().Dispatch();
					return;
				}
				global::Kampai.Game.Transaction.TransactionDefinition partyTransaction = RewardUtil.GetPartyTransaction(definitionService, playerService);
				awardLevelSignal.Dispatch(partyTransaction);
				base.injectionBinder.GetInstance<global::Kampai.UI.View.DisplayLevelUpRewardSignal>().Dispatch(false);
			}
		}
	}
}
