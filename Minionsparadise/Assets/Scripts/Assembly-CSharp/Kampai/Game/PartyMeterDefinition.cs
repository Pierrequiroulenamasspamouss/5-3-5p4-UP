namespace Kampai.Game
{
	public class PartyMeterDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1117;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.PartyMeterTierDefinition> Tiers { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, Tiers);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Tiers = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, Tiers);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TIERS":
				reader.Read();
				Tiers = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, Tiers);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
