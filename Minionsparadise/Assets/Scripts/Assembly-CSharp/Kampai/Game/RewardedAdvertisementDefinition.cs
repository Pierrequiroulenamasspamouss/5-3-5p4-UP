namespace Kampai.Game
{
	public class RewardedAdvertisementDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1018;
			}
		}

		public int MaxRewardsPerDayGlobal { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Game.AdPlacementDefinition> PlacementDefinitions { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(MaxRewardsPerDayGlobal);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, PlacementDefinitions);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			MaxRewardsPerDayGlobal = reader.ReadInt32();
			PlacementDefinitions = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, PlacementDefinitions);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PLACEMENTDEFINITIONS":
				reader.Read();
				PlacementDefinitions = ((converters.adPlacementDefinitionConverter == null) ? global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, PlacementDefinitions) : global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, converters.adPlacementDefinitionConverter, PlacementDefinitions));
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "MAXREWARDSPERDAYGLOBAL":
				reader.Read();
				MaxRewardsPerDayGlobal = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}
	}
}
