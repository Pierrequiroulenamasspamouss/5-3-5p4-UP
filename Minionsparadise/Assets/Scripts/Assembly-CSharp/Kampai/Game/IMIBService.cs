namespace Kampai.Game
{
	public interface IMIBService
	{
		bool IsUserReturning();

		void SetReturningKey();

		void ClearReturningKey();

		global::Kampai.Game.Transaction.TransactionDefinition PickWeightedTransaction(int weightedDefId);

		global::Kampai.Game.ItemDefinition GetItemDefinition(global::Kampai.Game.Transaction.TransactionDefinition transactionDef);

		global::Kampai.Game.ItemDefinition[] GetItemDefinitions(int weightedDefId);
	}
}
