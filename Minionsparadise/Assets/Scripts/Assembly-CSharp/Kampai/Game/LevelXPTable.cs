namespace Kampai.Game
{
	public class LevelXPTable : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1010;
			}
		}

		public global::System.Collections.Generic.IList<int> xpNeededList { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, xpNeededList);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			xpNeededList = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, xpNeededList);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "XPNEEDEDLIST":
				reader.Read();
				xpNeededList = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, xpNeededList);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
