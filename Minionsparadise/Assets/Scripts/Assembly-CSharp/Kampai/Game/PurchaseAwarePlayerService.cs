namespace Kampai.Game
{
	public class PurchaseAwarePlayerService : global::Kampai.Game.PlayerService, global::Kampai.Game.IPurchaseRecorder
	{
		public void AddPurchasedCurrency(bool isPremium, uint quantity)
		{
			if (quantity != 0)
			{
				if (isPremium)
				{
					player.AlterQuantity(global::Kampai.Game.StaticItem.PREMIUM_PURCHASED_ID, (int)quantity);
				}
				else
				{
					player.AlterQuantity(global::Kampai.Game.StaticItem.GRIND_PURCHASED_ID, (int)quantity);
				}
			}
		}

		public bool CurrencySpent(bool isPremium, uint quantity)
		{
			if (quantity != 0)
			{
				if (isPremium)
				{
					return CurrencySpent(global::Kampai.Game.StaticItem.PREMIUM_PURCHASED_ID, quantity);
				}
				return CurrencySpent(global::Kampai.Game.StaticItem.GRIND_PURCHASED_ID, quantity);
			}
			return false;
		}

		private bool CurrencySpent(global::Kampai.Game.StaticItem item, uint quantity)
		{
			uint quantity2 = player.GetQuantity(item);
			if (quantity2 != 0)
			{
				int amount = (int)((quantity2 < quantity) ? (0 - quantity2) : (0 - quantity));
				player.AlterQuantity(item, amount);
				return true;
			}
			return false;
		}

		public static bool PurchasedCurrencyUsed(global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IPlayerService playerService, bool isPremium, uint quantity)
		{
			global::Kampai.Game.IPurchaseRecorder purchaseRecorder = playerService as global::Kampai.Game.IPurchaseRecorder;
			if (purchaseRecorder == null)
			{
				logger.Error("Purchase recorder not available");
				return false;
			}
			return purchaseRecorder.CurrencySpent(isPremium, quantity);
		}
	}
}
