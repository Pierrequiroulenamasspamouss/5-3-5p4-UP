namespace Kampai.Game
{
	public class PendingCurrencyTransaction
	{
		private global::Kampai.Game.Transaction.TransactionDefinition pendingTransaction;

		private int rushCost;

		private bool rushing;

		private global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> rushOutputItems;

		private global::System.Collections.Generic.IList<global::Kampai.Game.Instance> outputs;

		private global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback;

		private global::Kampai.Game.TransactionTarget target;

		private global::Kampai.Game.TransactionArg transactionArg;

		public global::Kampai.Game.CurrencyTransactionFailReason FailReason;

		public bool Success { get; set; }

		public bool ParentSuccess { get; set; }

		public PendingCurrencyTransaction(global::Kampai.Game.Transaction.TransactionDefinition pendingTransaction, bool isRush, int rushCost, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> rushOutputItems, global::System.Collections.Generic.IList<global::Kampai.Game.Instance> outputs, global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> callback = null, global::Kampai.Game.TransactionTarget target = global::Kampai.Game.TransactionTarget.NO_VISUAL, global::Kampai.Game.TransactionArg transactionArg = null)
		{
			this.target = target;
			this.pendingTransaction = pendingTransaction;
			rushing = isRush;
			this.rushCost = rushCost;
			this.rushOutputItems = rushOutputItems;
			this.outputs = outputs;
			this.callback = callback;
			this.transactionArg = transactionArg;
		}

		public global::Kampai.Game.TransactionArg GetTransactionArg()
		{
			return transactionArg;
		}

		public global::Kampai.Game.TransactionTarget GetTransactionTarget()
		{
			return target;
		}

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> GetInputs()
		{
			return pendingTransaction.Inputs;
		}

		public global::Kampai.Game.Transaction.TransactionDefinition GetPendingTransaction()
		{
			return pendingTransaction;
		}

		public bool IsRushing()
		{
			return rushing;
		}

		public int GetRushCost()
		{
			return rushCost;
		}

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> GetRushOutputItems()
		{
			return rushOutputItems;
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.Instance> GetOutputs()
		{
			return outputs;
		}

		public global::System.Action<global::Kampai.Game.PendingCurrencyTransaction> GetCallback()
		{
			return callback;
		}
	}
}
