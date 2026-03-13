namespace Kampai.Game
{
	public class PartyUpDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1006;
			}
		}

		public float Multiplier { get; set; }

		public global::System.Collections.Generic.List<int> PointsNeeded { get; set; }

		public global::Kampai.Game.Transaction.TransactionDefinition PartyTransaction { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Multiplier);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, PointsNeeded);
			global::Kampai.Util.BinarySerializationUtil.WriteObject(writer, PartyTransaction);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Multiplier = reader.ReadSingle();
			PointsNeeded = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, PointsNeeded);
			PartyTransaction = global::Kampai.Util.BinarySerializationUtil.ReadObject<global::Kampai.Game.Transaction.TransactionDefinition>(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MULTIPLIER":
				reader.Read();
				Multiplier = global::System.Convert.ToSingle(reader.Value);
				break;
			case "POINTSNEEDED":
				reader.Read();
				PointsNeeded = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, PointsNeeded);
				break;
			case "PARTYTRANSACTION":
				reader.Read();
				PartyTransaction = ((converters.transactionDefinitionConverter == null) ? global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.Transaction.TransactionDefinition>(reader, converters) : converters.transactionDefinitionConverter.ReadJson(reader, converters));
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
