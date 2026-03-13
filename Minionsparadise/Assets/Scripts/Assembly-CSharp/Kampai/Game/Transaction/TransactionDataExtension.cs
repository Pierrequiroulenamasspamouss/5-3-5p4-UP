namespace Kampai.Game.Transaction
{
	public static class TransactionDataExtension
	{
		public static int GetInputCount(this global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition)
		{
			return (transactionDefinition != null) ? SafeListCount(transactionDefinition.Inputs) : 0;
		}

		public static int GetOutputCount(this global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition)
		{
			return (transactionDefinition != null) ? SafeListCount(transactionDefinition.Outputs) : 0;
		}

		public static int GetInputCount(this global::Kampai.Game.Transaction.TransactionInstance transactionInstance)
		{
			return (transactionInstance != null) ? SafeListCount(transactionInstance.Inputs) : 0;
		}

		public static int GetOutputCount(this global::Kampai.Game.Transaction.TransactionInstance transactionInstance)
		{
			return (transactionInstance != null) ? SafeListCount(transactionInstance.Outputs) : 0;
		}

		public static global::Kampai.Util.QuantityItem GetInputItem(this global::Kampai.Game.Transaction.TransactionInstance transactionInstance, int index)
		{
			return (transactionInstance != null) ? SafeListCount(transactionInstance.Inputs, index) : null;
		}

		public static global::Kampai.Util.QuantityItem GetInputItem(this global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, int index)
		{
			return (transactionDefinition != null) ? SafeListCount(transactionDefinition.Inputs, index) : null;
		}

		public static global::Kampai.Util.QuantityItem GetOutputItem(this global::Kampai.Game.Transaction.TransactionInstance transactionInstance, int index)
		{
			return (transactionInstance != null) ? SafeListCount(transactionInstance.Outputs, index) : null;
		}

		public static global::Kampai.Util.QuantityItem GetOutputItem(this global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, int index)
		{
			return (transactionDefinition != null) ? SafeListCount(transactionDefinition.Outputs, index) : null;
		}

		public static global::Kampai.Util.QuantityItem GetInputItemId(this global::Kampai.Game.Transaction.TransactionInstance transactionInstance, int itemId)
		{
			return (transactionInstance != null) ? SafeListByItemId(transactionInstance.Inputs, itemId) : null;
		}

		public static global::Kampai.Util.QuantityItem GetInputItemId(this global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, int itemId)
		{
			return (transactionDefinition != null) ? SafeListByItemId(transactionDefinition.Inputs, itemId) : null;
		}

		public static global::Kampai.Util.QuantityItem GetOutputItemId(this global::Kampai.Game.Transaction.TransactionInstance transactionInstance, int itemId)
		{
			return (transactionInstance != null) ? SafeListByItemId(transactionInstance.Outputs, itemId) : null;
		}

		public static global::Kampai.Util.QuantityItem GetOutputItemId(this global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition, int itemId)
		{
			return (transactionDefinition != null) ? SafeListByItemId(transactionDefinition.Outputs, itemId) : null;
		}

		private static global::Kampai.Util.QuantityItem SafeListByItemId(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> list, int itemId)
		{
			if (list == null)
			{
				return null;
			}
			for (int i = 0; i < list.Count; i++)
			{
				global::Kampai.Util.QuantityItem quantityItem = list[i];
				if (quantityItem != null && quantityItem.ID == itemId)
				{
					return quantityItem;
				}
			}
			return null;
		}

		private static global::Kampai.Util.QuantityItem SafeListCount(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> list, int index)
		{
			return (list == null) ? null : ((index >= list.Count) ? null : list[index]);
		}

		private static int SafeListCount(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> list)
		{
			return (list != null) ? list.Count : 0;
		}
	}
}
