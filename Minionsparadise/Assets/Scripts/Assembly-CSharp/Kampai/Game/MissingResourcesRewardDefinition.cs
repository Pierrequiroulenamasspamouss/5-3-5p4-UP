namespace Kampai.Game
{
	public class MissingResourcesRewardDefinition : global::Kampai.Game.AdPlacementDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1026;
			}
		}

		public int MaxCostPremiumCurrency { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(MaxCostPremiumCurrency);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			MaxCostPremiumCurrency = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MAXCOSTPREMIUMCURRENCY":
				reader.Read();
				MaxCostPremiumCurrency = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}
