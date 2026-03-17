namespace Kampai.Game
{
	public class LandExpansionDefinition : global::Kampai.Game.TaxonomyDefinition, global::Kampai.Game.Locatable
	{
		public override int TypeCode
		{
			get
			{
				return 1100;
			}
		}

		public int BuildingDefinitionID { get; set; }

		public int ExpansionID { get; set; }

		public global::Kampai.Game.Location Location { get; set; }

		public bool Grass { get; set; }

		public int MinimumLevel { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(BuildingDefinitionID);
			writer.Write(ExpansionID);
			global::Kampai.Util.BinarySerializationUtil.WriteLocation(writer, Location);
			writer.Write(Grass);
			writer.Write(MinimumLevel);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			BuildingDefinitionID = reader.ReadInt32();
			ExpansionID = reader.ReadInt32();
			Location = global::Kampai.Util.BinarySerializationUtil.ReadLocation(reader);
			Grass = reader.ReadBoolean();
			MinimumLevel = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BUILDINGDEFINITIONID":
				reader.Read();
				BuildingDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "EXPANSIONID":
				reader.Read();
				ExpansionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LOCATION":
				reader.Read();
				Location = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			case "GRASS":
				reader.Read();
				Grass = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "MINIMUMLEVEL":
				reader.Read();
				MinimumLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
