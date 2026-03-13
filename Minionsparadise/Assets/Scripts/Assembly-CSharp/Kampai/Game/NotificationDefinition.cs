namespace Kampai.Game
{
	public class NotificationDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1011;
			}
		}

		public string Type { get; set; }

		public int Seconds { get; set; }

		public string Title { get; set; }

		public string Text { get; set; }

		public int Track { get; set; }

		public string Sound { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Type);
			writer.Write(Seconds);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Title);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Text);
			writer.Write(Track);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Sound);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Type = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Seconds = reader.ReadInt32();
			Title = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Text = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Track = reader.ReadInt32();
			Sound = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TYPE":
				reader.Read();
				Type = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SECONDS":
				reader.Read();
				Seconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TITLE":
				reader.Read();
				Title = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TEXT":
				reader.Read();
				Text = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TRACK":
				reader.Read();
				Track = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SOUND":
				reader.Read();
				Sound = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
