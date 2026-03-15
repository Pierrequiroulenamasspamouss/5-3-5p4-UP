namespace Kampai.Game
{
	public class CurrencyStoreDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1143;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.CurrencyStoreCategoryDefinition> CategoryDefinitions { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, CategoryDefinitions);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			CategoryDefinitions = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, CategoryDefinitions);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "CATEGORYDEFINITIONS":
				reader.Read();
				CategoryDefinitions = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, CategoryDefinitions);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
