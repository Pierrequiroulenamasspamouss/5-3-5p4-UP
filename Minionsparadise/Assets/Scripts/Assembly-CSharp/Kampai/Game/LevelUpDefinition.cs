namespace Kampai.Game
{
	public class LevelUpDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1009;
			}
		}

		public global::System.Collections.Generic.IList<int> transactionList { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, transactionList);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			transactionList = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, transactionList);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TRANSACTIONLIST":
				reader.Read();
				transactionList = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, transactionList);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
