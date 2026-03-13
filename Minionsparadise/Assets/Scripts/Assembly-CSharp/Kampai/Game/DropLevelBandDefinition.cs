namespace Kampai.Game
{
	public class DropLevelBandDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1002;
			}
		}

		public global::System.Collections.Generic.List<int> HarvestsPerDrop { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, HarvestsPerDrop);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			HarvestsPerDrop = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, HarvestsPerDrop);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "HARVESTSPERDROP":
				reader.Read();
				HarvestsPerDrop = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, HarvestsPerDrop);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
