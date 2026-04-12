namespace Kampai.Game
{
	public class LegalDocumentDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1004;
			}
		}

		public global::Kampai.Util.LegalDocuments.LegalType type { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Game.LegalDocumentURL> urls { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, type);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteLegalDocumentURL, urls);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			type = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Util.LegalDocuments.LegalType>(reader);
			urls = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadLegalDocumentURL, urls);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "URLS":
				reader.Read();
				urls = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadLegalDocumentURL, urls);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "TYPE":
				reader.Read();
				type = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Util.LegalDocuments.LegalType>(reader);
				break;
			}
			return true;
		}
	}
}
