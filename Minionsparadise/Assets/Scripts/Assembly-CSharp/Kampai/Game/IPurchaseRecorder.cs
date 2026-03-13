namespace Kampai.Game
{
	public interface IPurchaseRecorder
	{
		void AddPurchasedCurrency(bool isPremium, uint quantity);

		bool CurrencySpent(bool isPremium, uint quantity);
	}
}
