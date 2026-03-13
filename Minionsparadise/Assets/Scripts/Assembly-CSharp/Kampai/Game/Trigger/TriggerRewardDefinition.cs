namespace Kampai.Game.Trigger
{
	[global::Kampai.Util.RequiresJsonConverter]
	public abstract class TriggerRewardDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1178;
			}
		}

		public int SKUId { get; set; }

		public string buttonText { get; set; }

		public global::Kampai.Game.Transaction.TransactionInstance transaction { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerRewardLayout> layoutElements { get; set; }

		public global::Kampai.Game.Trigger.TriggerRewardLayout.Layout rewardLayout { get; set; }

		public abstract global::Kampai.Game.Trigger.TriggerRewardType.Identifier type { get; }

		public bool IsDynamicReward
		{
			get
			{
				return global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputCount(transaction) == 0;
			}
		}

		public virtual bool IsFree
		{
			get
			{
				return global::Kampai.Game.Transaction.TransactionDataExtension.GetInputCount(transaction) == 0 && SKUId == 0;
			}
		}

		public bool HasInputs
		{
			get
			{
				return global::Kampai.Game.Transaction.TransactionDataExtension.GetInputCount(transaction) != 0;
			}
		}

		public bool IsCash
		{
			get
			{
				return global::Kampai.Game.Transaction.TransactionDataExtension.GetInputCount(transaction) == 0 && SKUId != 0;
			}
		}

		public virtual bool IsUniquePerInstance
		{
			get
			{
				return true;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(SKUId);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, buttonText);
			global::Kampai.Util.BinarySerializationUtil.WriteTransactionInstance(writer, transaction);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteTriggerRewardLayout, layoutElements);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, rewardLayout);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			SKUId = reader.ReadInt32();
			buttonText = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			transaction = global::Kampai.Util.BinarySerializationUtil.ReadTransactionInstance(reader);
			layoutElements = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadTriggerRewardLayout, layoutElements);
			rewardLayout = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.Trigger.TriggerRewardLayout.Layout>(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SKUID":
				reader.Read();
				SKUId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BUTTONTEXT":
				reader.Read();
				buttonText = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TRANSACTION":
				reader.Read();
				transaction = global::Kampai.Util.ReaderUtil.ReadTransactionInstance(reader, converters);
				break;
			case "LAYOUTELEMENTS":
				reader.Read();
				layoutElements = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadTriggerRewardLayout, layoutElements);
				break;
			case "REWARDLAYOUT":
				reader.Read();
				rewardLayout = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.Trigger.TriggerRewardLayout.Layout>(reader);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public abstract void RewardPlayer(global::strange.extensions.context.api.ICrossContextCapable context);
	}
}
