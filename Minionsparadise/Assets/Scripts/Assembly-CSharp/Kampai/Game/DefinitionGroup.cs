namespace Kampai.Game
{
	public class DefinitionGroup : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1001;
			}
		}

		public global::System.Collections.Generic.IList<int> Group { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, Group);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Group = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, Group);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "GROUP":
				reader.Read();
				Group = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, Group);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
