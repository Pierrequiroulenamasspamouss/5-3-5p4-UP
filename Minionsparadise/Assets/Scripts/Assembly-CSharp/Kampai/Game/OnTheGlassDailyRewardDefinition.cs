namespace Kampai.Game
{
	public class OnTheGlassDailyRewardDefinition : global::Kampai.Game.AdPlacementDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1021;
			}
		}

		public global::Kampai.Game.RewardTiers RewardTiers { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteRewardTiers(writer, RewardTiers);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			RewardTiers = global::Kampai.Util.BinarySerializationUtil.ReadRewardTiers(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "REWARDTIERS":
				reader.Read();
				RewardTiers = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.RewardTiers>(reader, converters);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
