namespace Kampai.Game.Transaction
{
	public class TransactionInstance : global::Kampai.Util.Identifiable
	{
		public int ID { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> Inputs { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> Outputs { get; set; }

		public override string ToString()
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			stringBuilder.Append(base.ToString());
			stringBuilder.Append(" [I=");
			AppendQuantityItems(Inputs, stringBuilder);
			stringBuilder.Append("][O=");
			AppendQuantityItems(Outputs, stringBuilder);
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		private void AppendQuantityItems(global::System.Collections.Generic.ICollection<global::Kampai.Util.QuantityItem> group, global::System.Text.StringBuilder sb)
		{
			bool flag = false;
			if (group == null)
			{
				return;
			}
			foreach (global::Kampai.Util.QuantityItem item in group)
			{
				if (flag)
				{
					sb.Append(" ");
				}
				sb.Append(item.ID);
				sb.Append("@");
				sb.Append(item.Quantity);
				flag = true;
			}
		}

		public global::Kampai.Game.Transaction.TransactionDefinition ToDefinition()
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
			transactionDefinition.ID = ID;
			transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			if (Inputs != null)
			{
				foreach (global::Kampai.Util.QuantityItem input in Inputs)
				{
					transactionDefinition.Inputs.Add(new global::Kampai.Util.QuantityItem(input.ID, input.Quantity));
				}
			}
			if (Outputs != null)
			{
				foreach (global::Kampai.Util.QuantityItem output in Outputs)
				{
					transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(output.ID, output.Quantity));
				}
			}
			return transactionDefinition;
		}
	}
}
