namespace Kampai.Game
{
	public class QuestResourceDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1133;
			}
		}

		public string resourcePath { get; set; }

		public string maskPath { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, resourcePath);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, maskPath);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			resourcePath = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			maskPath = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MASKPATH":
				reader.Read();
				maskPath = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "RESOURCEPATH":
				reader.Read();
				resourcePath = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			}
			return true;
		}
	}
}
