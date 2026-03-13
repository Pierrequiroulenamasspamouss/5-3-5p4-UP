namespace Kampai.Game
{
	public class PartyPointsPerLevelDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1120;
			}
		}

		public int Level { get; set; }

		public int PointsPerTick { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Level);
			writer.Write(PointsPerTick);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Level = reader.ReadInt32();
			PointsPerTick = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			default:
			{
                        int num = 1; //FIX USE OF UNASSIGNED VARIABLE
                        if (num == 1)
				{
					reader.Read();
					PointsPerTick = global::System.Convert.ToInt32(reader.Value);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "LEVEL":
				reader.Read();
				Level = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}
	}
}
