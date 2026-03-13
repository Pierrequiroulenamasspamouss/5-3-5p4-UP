namespace Kampai.Main
{
	public class HindsightCampaignDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>, global::Kampai.Util.IUTCRangeable
	{
		public global::System.Collections.Generic.Dictionary<string, object> Content;

		public global::System.Collections.Generic.Dictionary<string, object> URI;

		public override int TypeCode
		{
			get
			{
				return 1193;
			}
		}

		public string Name { get; set; }

		public string Scope { get; set; }

		public string Platform { get; set; }

		public int Limit { get; set; }

		public int UTCStartDate { get; set; }

		public int UTCEndDate { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Name);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Scope);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Platform);
			writer.Write(Limit);
			writer.Write(UTCStartDate);
			writer.Write(UTCEndDate);
			global::Kampai.Util.BinarySerializationUtil.WriteDictionary(writer, Content);
			global::Kampai.Util.BinarySerializationUtil.WriteDictionary(writer, URI);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Name = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Scope = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Platform = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Limit = reader.ReadInt32();
			UTCStartDate = reader.ReadInt32();
			UTCEndDate = reader.ReadInt32();
			Content = global::Kampai.Util.BinarySerializationUtil.ReadDictionary(reader);
			URI = global::Kampai.Util.BinarySerializationUtil.ReadDictionary(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "NAME":
				reader.Read();
				Name = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SCOPE":
				reader.Read();
				Scope = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PLATFORM":
				reader.Read();
				Platform = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "LIMIT":
				reader.Read();
				Limit = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCSTARTDATE":
				reader.Read();
				UTCStartDate = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCENDDATE":
				reader.Read();
				UTCEndDate = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CONTENT":
				reader.Read();
				Content = global::Kampai.Util.ReaderUtil.ReadDictionary(reader);
				break;
			case "URI":
				reader.Read();
				URI = global::Kampai.Util.ReaderUtil.ReadDictionary(reader);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Main.HindsightCampaign(this);
		}
	}
}
