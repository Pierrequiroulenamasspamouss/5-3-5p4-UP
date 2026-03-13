namespace Kampai.Game.Transaction
{
	[global::Kampai.Util.RequiresJsonConverter]
	public class TransactionDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1007;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> Inputs { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> Outputs { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, Inputs);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, Outputs);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Inputs = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, Inputs);
			Outputs = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, Outputs);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "OUTPUTS":
				reader.Read();
				Outputs = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, Outputs);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "INPUTS":
				reader.Read();
				Inputs = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, Inputs);
				break;
			}
			return true;
		}

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

		public global::Kampai.Game.Transaction.TransactionInstance ToInstance()
		{
			global::Kampai.Game.Transaction.TransactionInstance transactionInstance = new global::Kampai.Game.Transaction.TransactionInstance();
			transactionInstance.ID = ID;
			transactionInstance.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			transactionInstance.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			if (Inputs != null)
			{
				foreach (global::Kampai.Util.QuantityItem input in Inputs)
				{
					transactionInstance.Inputs.Add(new global::Kampai.Util.QuantityItem(input.ID, input.Quantity));
				}
			}
			if (Outputs != null)
			{
				foreach (global::Kampai.Util.QuantityItem output in Outputs)
				{
					transactionInstance.Outputs.Add(new global::Kampai.Util.QuantityItem(output.ID, output.Quantity));
				}
			}
			return transactionInstance;
		}

		public global::Kampai.Game.Transaction.TransactionDefinition CopyTransaction()
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
			transactionDefinition.ID = ID;
			transactionDefinition.LocalizedKey = base.LocalizedKey;
			transactionDefinition.Disabled = base.Disabled;
			if (Inputs != null)
			{
				transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>(Inputs.Count);
				foreach (global::Kampai.Util.QuantityItem input in Inputs)
				{
					transactionDefinition.Inputs.Add(new global::Kampai.Util.QuantityItem(input.ID, input.Quantity));
				}
			}
			if (Outputs != null)
			{
				transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>(Outputs.Count);
				foreach (global::Kampai.Util.QuantityItem output in Outputs)
				{
					transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(output.ID, output.Quantity));
				}
			}
			return transactionDefinition;
		}
	}
}
