namespace Kampai.Game
{
	public class CommonLandExpansionDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1097;
			}
		}

		public string MinionPrefab { get; set; }

		public string VFXGrassClearing { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, MinionPrefab);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, VFXGrassClearing);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			MinionPrefab = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			VFXGrassClearing = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "VFXGRASSCLEARING":
				reader.Read();
				VFXGrassClearing = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "MINIONPREFAB":
				reader.Read();
				MinionPrefab = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			}
			return true;
		}
	}
}
