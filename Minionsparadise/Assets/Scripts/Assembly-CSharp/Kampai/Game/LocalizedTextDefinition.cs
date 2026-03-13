namespace Kampai.Game
{
	public class LocalizedTextDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1101;
			}
		}

		public string Language { get; set; }

		public string Translation { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Language);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Translation);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Language = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Translation = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TRANSLATION":
				reader.Read();
				Translation = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "LANGUAGE":
				reader.Read();
				Language = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			}
			return true;
		}
	}
}
