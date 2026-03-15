namespace Kampai.Game
{
	public abstract class RepairableBuildingDefinition : global::Kampai.Game.BuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1033;
			}
		}

		public string brokenPrefab { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, brokenPrefab);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			brokenPrefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BROKENPREFAB":
				reader.Read();
				brokenPrefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
