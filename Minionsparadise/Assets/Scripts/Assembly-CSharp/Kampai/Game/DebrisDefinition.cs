namespace Kampai.Game
{
	public class DebrisDefinition : global::Kampai.Game.Definition, global::Kampai.Game.Locatable
	{
		public override int TypeCode
		{
			get
			{
				return 1098;
			}
		}

		public int BuildingDefinitionID { get; set; }

		public global::Kampai.Game.Location Location { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(BuildingDefinitionID);
			global::Kampai.Util.BinarySerializationUtil.WriteLocation(writer, Location);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			BuildingDefinitionID = reader.ReadInt32();
			Location = global::Kampai.Util.BinarySerializationUtil.ReadLocation(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "LOCATION":
				reader.Read();
				Location = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "BUILDINGDEFINITIONID":
				reader.Read();
				BuildingDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}
	}
}
