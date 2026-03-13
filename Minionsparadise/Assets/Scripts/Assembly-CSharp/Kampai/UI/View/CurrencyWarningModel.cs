namespace Kampai.UI.View
{
	public class CurrencyWarningModel
	{
		public bool GrindFromPremium { get; private set; }

		public int Amount { get; private set; }

		public int Cost { get; private set; }

		public global::Kampai.Game.StoreItemType Type { get; private set; }

		public global::Kampai.Game.PendingCurrencyTransaction PendingTransaction { get; private set; }

		public CurrencyWarningModel(int amount, int cost, global::Kampai.Game.StoreItemType type, bool grindFromPremium = false, global::Kampai.Game.PendingCurrencyTransaction pendingTransaction = null)
		{
			GrindFromPremium = grindFromPremium;
			Amount = amount;
			Cost = cost;
			Type = type;
			PendingTransaction = pendingTransaction;
		}
	}
}
