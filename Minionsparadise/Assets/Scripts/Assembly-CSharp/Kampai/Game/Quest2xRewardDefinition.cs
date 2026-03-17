namespace Kampai.Game
{
	public class Quest2xRewardDefinition : global::Kampai.Game.AdPlacementDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1028;
			}
		}

		public global::System.Collections.Generic.List<int> AllowedRewardItemTypes { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, AllowedRewardItemTypes);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			AllowedRewardItemTypes = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, AllowedRewardItemTypes);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ALLOWEDREWARDITEMTYPES":
				reader.Read();
				AllowedRewardItemTypes = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, AllowedRewardItemTypes);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
