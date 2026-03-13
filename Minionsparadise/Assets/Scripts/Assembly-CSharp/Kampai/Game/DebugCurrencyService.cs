namespace Kampai.Game
{
	public class DebugCurrencyService : global::Kampai.Game.CurrencyService
	{
		[Inject]
		public global::Kampai.UI.View.ShowMockStoreDialogSignal showMockStoreDialogSignal { get; set; }

		public override void RequestPurchase(global::Kampai.Game.KampaiPendingTransaction item)
		{
			showMockStoreDialogSignal.Dispatch(item);
		}

		public override string GetPriceWithCurrencyAndFormat(string SKU)
		{
			switch (SKU)
			{
			case "SKU_FEW_DIAMONDS":
				return "$1.99";
			case "SKU_PILE_DIAMONDS":
				return "$4.99";
			case "SKU_SACK_DIAMONDS":
				return "$9.99";
			case "SKU_BAGS_DIAMONDS":
				return "$19.99";
			case "SKU_CHEST_DIAMONDS":
				return "$39.99";
			case "SKU_BIG_CHEST_DIAMONDS":
				return "$49.00";
			case "SKU_PILE_SAND_DOLLARS":
				return "$0.99";
			case "SKU_BAG_SAND_DOLLARS":
				return "$2.99";
			case "SKU_SACK_SAND_DOLLARS":
				return "$7.99";
			case "SKU_BOX_SAND_DOLLARS":
				return "$14.99";
			case "SKU_CHEST_SAND_DOLLARS":
				return "$29.99";
			case "SKU_TRUNK_SAND_DOLLARS":
				return "$79.00";
			case "SKU_STARTER_PACK":
				return "$5.99";
			case "SKU_MIGNETTE_UNLOCK_1":
				return "$7.99";
			case "SKU_MIGNETTE_UNLOCK_2":
				return "$6.99";
			case "SKU_FREE_REDEMPTION":
				return "$0.00";
			case "SKU_HOLIDAY_OFFER":
				return "$3.99";
			case "SKU_DOUBLE_DRIBBLE":
				return "$9.99";
			case "SKU_DUBS_AND_A_TUB":
				return "$19.99";
			case "SKU_BAZOOKA_STARTER_KIT":
				return "$9.99";
			case "SKU_CHILI_AND_CHILL":
				return "$9.99";
			case "SKU_BEATS_AND_BANANAS":
				return "$14.99";
			case "SKU_LET_THERE_BE_LIGHT":
				return "$14.99";
			case "SKU_SPROING_IN_YOUR_STEP":
				return "$19.99";
			case "SKU_THE_WRITE_STUFF":
				return "$19.99";
			case "SKU_GUN_AND_GAMES":
				return "$9.99";
			case "SKU_HIGH_EATS_FINE_EATS":
				return "$9.99";
			case "SKU_TREATS_AND_TOYS":
				return "$9.99";
			case "SKU_MR_FIX_IT":
				return "$14.99";
			case "SKU_HAPPY_CAPPER":
				return "$14.99";
			case "SKU_DOOHICKEYS":
				return "$14.99";
			case "SKU_TECH_BOOM":
				return "$14.99";
			case "SKU_MORE_TO_STORE":
				return "$1.99";
			case "SKU_EXPAND_AND_EXPLORE":
				return "$1.99";
			default:
				return "$9.99";
			}
		}

		public override void ReceiptValidationCallback(global::Kampai.Game.Mtx.ReceiptValidationResult result)
		{
		}

		public override void PauseTransactionsHandling()
		{
		}

		public override void ResumeTransactionsHandling()
		{
		}

		public override void CollectRedemption(global::Kampai.Game.Mtx.ReceiptValidationResult pendingRedemption)
		{
			PurchaseSucceededAndValidatedCallback(pendingRedemption.sku);
		}

		public override void RestorePurchases()
		{
		}
	}
}
