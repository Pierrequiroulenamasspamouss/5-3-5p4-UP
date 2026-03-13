namespace Kampai.Game
{
	public class LevelFunTable : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1005;
			}
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.PartyUpDefinition> partiesNeededList { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, partiesNeededList);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			partiesNeededList = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, partiesNeededList);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PARTIESNEEDEDLIST":
				reader.Read();
				partiesNeededList = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, partiesNeededList);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
