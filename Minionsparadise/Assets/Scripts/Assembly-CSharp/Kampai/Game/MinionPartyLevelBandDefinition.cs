namespace Kampai.Game
{
	public class MinionPartyLevelBandDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1119;
			}
		}

		public int MinLevel { get; set; }

		public int PointsTotal { get; set; }

		public int Delta { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(MinLevel);
			writer.Write(PointsTotal);
			writer.Write(Delta);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			MinLevel = reader.ReadInt32();
			PointsTotal = reader.ReadInt32();
			Delta = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MINLEVEL":
				reader.Read();
				MinLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			case "POINTSTOTAL":
				reader.Read();
				PointsTotal = global::System.Convert.ToInt32(reader.Value);
				break;
			case "DELTA":
				reader.Read();
				Delta = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
