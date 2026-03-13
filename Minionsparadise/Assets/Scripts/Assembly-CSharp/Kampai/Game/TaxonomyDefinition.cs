namespace Kampai.Game
{
	public class TaxonomyDefinition : global::Kampai.Game.DisplayableDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1014;
			}
		}

		public string TaxonomyHighLevel { get; set; }

		public string TaxonomySpecific { get; set; }

		public string TaxonomyType { get; set; }

		public string TaxonomyOther { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, TaxonomyHighLevel);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, TaxonomySpecific);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, TaxonomyType);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, TaxonomyOther);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			TaxonomyHighLevel = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			TaxonomySpecific = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			TaxonomyType = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			TaxonomyOther = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TAXONOMYHIGHLEVEL":
				reader.Read();
				TaxonomyHighLevel = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TAXONOMYSPECIFIC":
				reader.Read();
				TaxonomySpecific = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TAXONOMYTYPE":
				reader.Read();
				TaxonomyType = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TAXONOMYOTHER":
				reader.Read();
				TaxonomyOther = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
