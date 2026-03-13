namespace Kampai.Game
{
	public class DropItemDefinition : global::Kampai.Game.ItemDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1091;
			}
		}

		public float Rarity { get; set; }

		public global::Kampai.Game.DropType dropType { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(Rarity);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, dropType);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Rarity = reader.ReadSingle();
			dropType = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.DropType>(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "DROPTYPE":
				reader.Read();
				dropType = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.DropType>(reader);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "RARITY":
				reader.Read();
				Rarity = global::System.Convert.ToSingle(reader.Value);
				break;
			}
			return true;
		}
	}
}
