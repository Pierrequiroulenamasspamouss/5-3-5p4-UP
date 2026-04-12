namespace Kampai.Game
{
	public class DynamicQuestDefinition : global::Kampai.Game.QuestDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1129;
			}
		}

		public global::Kampai.Game.Transaction.TransactionInstance RewardTransactionInstance { get; set; }

		public int DropStep { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteTransactionInstance(writer, RewardTransactionInstance);
			writer.Write(DropStep);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			RewardTransactionInstance = global::Kampai.Util.BinarySerializationUtil.ReadTransactionInstance(reader);
			DropStep = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "DROPSTEP":
				reader.Read();
				DropStep = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "REWARDTRANSACTIONINSTANCE":
				reader.Read();
				RewardTransactionInstance = global::Kampai.Util.ReaderUtil.ReadTransactionInstance(reader, converters);
				break;
			}
			return true;
		}

		public override global::Kampai.Game.Transaction.TransactionDefinition GetReward(global::Kampai.Game.IDefinitionService definitionService)
		{
			if (RewardTransactionInstance != null)
			{
				return RewardTransactionInstance.ToDefinition();
			}
			return base.GetReward(definitionService);
		}
	}
}
