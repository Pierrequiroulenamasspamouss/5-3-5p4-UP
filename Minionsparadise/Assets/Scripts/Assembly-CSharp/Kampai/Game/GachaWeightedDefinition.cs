namespace Kampai.Game
{
	public class GachaWeightedDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1111;
			}
		}

		public int Minions { get; set; }

		public global::Kampai.Game.Transaction.WeightedDefinition WeightedDefinition { get; set; }

		public bool PartyOnly { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Minions);
			global::Kampai.Util.BinarySerializationUtil.WriteObject(writer, WeightedDefinition);
			writer.Write(PartyOnly);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Minions = reader.ReadInt32();
			WeightedDefinition = global::Kampai.Util.BinarySerializationUtil.ReadObject<global::Kampai.Game.Transaction.WeightedDefinition>(reader);
			PartyOnly = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MINIONS":
				reader.Read();
				Minions = global::System.Convert.ToInt32(reader.Value);
				break;
			case "WEIGHTEDDEFINITION":
				reader.Read();
				WeightedDefinition = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.Transaction.WeightedDefinition>(reader, converters);
				break;
			case "PARTYONLY":
				reader.Read();
				PartyOnly = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
