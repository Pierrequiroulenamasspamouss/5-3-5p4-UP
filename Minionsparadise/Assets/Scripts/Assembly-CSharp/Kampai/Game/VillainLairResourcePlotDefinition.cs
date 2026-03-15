namespace Kampai.Game
{
	public class VillainLairResourcePlotDefinition : global::Kampai.Game.AnimatingBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1068;
			}
		}

		public string brokenPrefab_loaded { get; set; }

		public string prefab_loaded { get; set; }

		public int randomGagMin { get; set; }

		public int randomGagMax { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, brokenPrefab_loaded);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, prefab_loaded);
			writer.Write(randomGagMin);
			writer.Write(randomGagMax);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			brokenPrefab_loaded = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			prefab_loaded = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			randomGagMin = reader.ReadInt32();
			randomGagMax = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BROKENPREFAB_LOADED":
				reader.Read();
				brokenPrefab_loaded = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PREFAB_LOADED":
				reader.Read();
				prefab_loaded = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "RANDOMGAGMIN":
				reader.Read();
				randomGagMin = global::System.Convert.ToInt32(reader.Value);
				break;
			case "RANDOMGAGMAX":
				reader.Read();
				randomGagMax = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.VillainLairResourcePlot(this);
		}
	}
}
