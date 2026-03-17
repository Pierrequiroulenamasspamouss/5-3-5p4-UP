namespace Kampai.Game
{
	public class IngredientsItemDefinition : global::Kampai.Game.ItemDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1093;
			}
		}

		public uint TimeToHarvest { get; set; }

		public int TransactionId { get; set; }

		public int Tier { get; set; }

		public int BaseXP { get; set; }

		public int BasePartyPoint { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(TimeToHarvest);
			writer.Write(TransactionId);
			writer.Write(Tier);
			writer.Write(BaseXP);
			writer.Write(BasePartyPoint);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			TimeToHarvest = reader.ReadUInt32();
			TransactionId = reader.ReadInt32();
			Tier = reader.ReadInt32();
			BaseXP = reader.ReadInt32();
			BasePartyPoint = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TIMETOHARVEST":
				reader.Read();
				TimeToHarvest = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "TRANSACTIONID":
				reader.Read();
				TransactionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TIER":
				reader.Read();
				Tier = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BASEXP":
				reader.Read();
				BaseXP = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BASEPARTYPOINT":
				reader.Read();
				BasePartyPoint = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
