namespace Kampai.Game
{
	public class OrderBoardTicket : global::Kampai.Game.IGameTimeTracker
	{
		public global::Kampai.Game.Transaction.TransactionInstance TransactionInst { get; set; }

		public int StartGameTime { get; set; }

		public int BoardIndex { get; set; }

		public int OrderNameTableIndex { get; set; }

		public int StartTime { get; set; }

		public int CharacterDefinitionId { get; set; }
	}
}
