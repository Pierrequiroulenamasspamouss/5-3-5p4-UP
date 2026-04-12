namespace Kampai.Game
{
	[global::Kampai.Util.RequiresJsonConverter]
	public abstract class PlotDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1128;
			}
		}

		public int X { get; set; }

		public int Y { get; set; }

		public int FootprintID { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(X);
			writer.Write(Y);
			writer.Write(FootprintID);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			X = reader.ReadInt32();
			Y = reader.ReadInt32();
			FootprintID = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "X":
				reader.Read();
				X = global::System.Convert.ToInt32(reader.Value);
				break;
			case "Y":
				reader.Read();
				Y = global::System.Convert.ToInt32(reader.Value);
				break;
			case "FOOTPRINTID":
				reader.Read();
				FootprintID = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public abstract global::Kampai.Game.Plot Instantiate();
	}
}
