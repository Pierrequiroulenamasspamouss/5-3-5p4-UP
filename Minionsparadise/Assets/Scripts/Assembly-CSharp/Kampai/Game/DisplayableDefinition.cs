namespace Kampai.Game
{
	public class DisplayableDefinition : global::Kampai.Game.Definition, global::Kampai.Game.IDisplayableDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1015;
			}
		}

		public string Image { get; set; }

		public string Mask { get; set; }

		public string Description { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Image);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Mask);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Description);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Image = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Mask = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Description = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "IMAGE":
				reader.Read();
				Image = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MASK":
				reader.Read();
				Mask = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "DESCRIPTION":
				reader.Read();
				Description = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
