namespace Kampai.Game
{
	public class KampaiPendingTransaction
	{
		public string ExternalIdentifier { get; set; }

		[global::Kampai.Util.Serializer("KampaiPendingTransaction.SerializeDefinition")]
		public global::Kampai.Game.Transaction.TransactionDefinition Transaction { get; set; }

		public global::Kampai.Game.Transaction.TransactionInstance TransactionInstance { get; set; }

		public int StoreItemDefinitionId { get; set; }

		public int UTCTimeCreated { get; set; }

		internal static void SerializeDefinition(global::Newtonsoft.Json.JsonWriter writer, global::Kampai.Game.Transaction.TransactionDefinition value)
		{
			writer.WriteValue(value.ID);
		}
	}
}
