namespace Kampai.Game
{
	public class FountainBuildingDefinition : global::Kampai.Game.AnimatingBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1050;
			}
		}

		public string AspirationalMessage { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AspirationalMessage);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			AspirationalMessage = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ASPIRATIONALMESSAGE":
				reader.Read();
				AspirationalMessage = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.FountainBuilding(this);
		}
	}
}
