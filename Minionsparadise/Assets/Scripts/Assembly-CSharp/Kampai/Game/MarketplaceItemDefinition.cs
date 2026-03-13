namespace Kampai.Game
{
	public class MarketplaceItemDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1103;
			}
		}

		public int ItemID { get; set; }

		public int FloorPrice { get; set; }

		public int MinStrikePrice { get; set; }

		public int StartingStrikePrice { get; set; }

		public int MaxStrikePrice { get; set; }

		public int CeilingPrice { get; set; }

		public int ProbabilityWeight { get; set; }

		public int TransactionID { get; set; }

		public int LowPriceBuyTimeSeconds { get; set; }

		public int HighPriceBuyTimeSeconds { get; set; }

		public int PriceTrend { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(ItemID);
			writer.Write(FloorPrice);
			writer.Write(MinStrikePrice);
			writer.Write(StartingStrikePrice);
			writer.Write(MaxStrikePrice);
			writer.Write(CeilingPrice);
			writer.Write(ProbabilityWeight);
			writer.Write(TransactionID);
			writer.Write(LowPriceBuyTimeSeconds);
			writer.Write(HighPriceBuyTimeSeconds);
			writer.Write(PriceTrend);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			ItemID = reader.ReadInt32();
			FloorPrice = reader.ReadInt32();
			MinStrikePrice = reader.ReadInt32();
			StartingStrikePrice = reader.ReadInt32();
			MaxStrikePrice = reader.ReadInt32();
			CeilingPrice = reader.ReadInt32();
			ProbabilityWeight = reader.ReadInt32();
			TransactionID = reader.ReadInt32();
			LowPriceBuyTimeSeconds = reader.ReadInt32();
			HighPriceBuyTimeSeconds = reader.ReadInt32();
			PriceTrend = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ITEMID":
				reader.Read();
				ItemID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "FLOORPRICE":
				reader.Read();
				FloorPrice = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MINSTRIKEPRICE":
				reader.Read();
				MinStrikePrice = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STARTINGSTRIKEPRICE":
				reader.Read();
				StartingStrikePrice = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MAXSTRIKEPRICE":
				reader.Read();
				MaxStrikePrice = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CEILINGPRICE":
				reader.Read();
				CeilingPrice = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PROBABILITYWEIGHT":
				reader.Read();
				ProbabilityWeight = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TRANSACTIONID":
				reader.Read();
				TransactionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LOWPRICEBUYTIMESECONDS":
				reader.Read();
				LowPriceBuyTimeSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "HIGHPRICEBUYTIMESECONDS":
				reader.Read();
				HighPriceBuyTimeSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PRICETREND":
				reader.Read();
				PriceTrend = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
