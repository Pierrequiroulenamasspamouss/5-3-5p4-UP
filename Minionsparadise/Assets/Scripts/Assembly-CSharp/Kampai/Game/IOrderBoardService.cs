namespace Kampai.Game
{
	public interface IOrderBoardService
	{
		void Initialize();

		void ReplaceCharacterTickets(int characterDefinitionID);

		int GetLongestIdleOrderDuration();

		global::Kampai.Game.Transaction.TransactionDefinition GetLongestIdleOrderTransaction();

		void AddPriorityPrestigeCharacter(int prestigeDefinitionID);

		void GetNewTicket(int orderBoardIndex);

		void UpdateOrderNumber();

		global::Kampai.Game.OrderBoard GetBoard();

		void SetEnabled(bool b);
	}
}
