namespace Kampai.Game
{
	public class MarketplaceDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1102;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.MarketplaceItemDefinition> itemDefinitions { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.MarketplaceSaleSlotDefinition> slotDefinitions { get; set; }

		public global::System.Collections.Generic.IList<global::UnityEngine.Vector3> buyTimeSpline { get; set; }

		public global::Kampai.Game.MarketplaceRefreshTimerDefinition refreshTimerDefinition { get; set; }

		public int MaxSellQuantity { get; set; }

		public int MaxDropQuantity { get; set; }

		public float VariabilityBuyTimePercent { get; set; }

		public int CraftableWeight { get; set; }

		public int BaseResourceWeight { get; set; }

		public int DropWeight { get; set; }

		public int TotalSaleAds { get; set; }

		public int TotalBuyAds { get; set; }

		public int StartingBuyAds { get; set; }

		public int StandardSlots { get; set; }

		public int FacebookSlots { get; set; }

		public int MaxPremiumSlots { get; set; }

		public int PremiumInitialCost { get; set; }

		public int PremiumIncrementCost { get; set; }

		public int DeleteSaleCost { get; set; }

		public int LevelGate { get; set; }

		public int SaleCancellationCost { get; set; }

		public global::Kampai.Util.KampaiColor SellPriceUpTextColor { get; set; }

		public global::Kampai.Util.KampaiColor SellPriceUpBackgroundColor { get; set; }

		public global::Kampai.Util.KampaiColor SellPriceDownTextColor { get; set; }

		public global::Kampai.Util.KampaiColor SellPriceDownBackgroundColor { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, itemDefinitions);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, slotDefinitions);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteVector3, buyTimeSpline);
			global::Kampai.Util.BinarySerializationUtil.WriteObject(writer, refreshTimerDefinition);
			writer.Write(MaxSellQuantity);
			writer.Write(MaxDropQuantity);
			writer.Write(VariabilityBuyTimePercent);
			writer.Write(CraftableWeight);
			writer.Write(BaseResourceWeight);
			writer.Write(DropWeight);
			writer.Write(TotalSaleAds);
			writer.Write(TotalBuyAds);
			writer.Write(StartingBuyAds);
			writer.Write(StandardSlots);
			writer.Write(FacebookSlots);
			writer.Write(MaxPremiumSlots);
			writer.Write(PremiumInitialCost);
			writer.Write(PremiumIncrementCost);
			writer.Write(DeleteSaleCost);
			writer.Write(LevelGate);
			writer.Write(SaleCancellationCost);
			global::Kampai.Util.BinarySerializationUtil.WriteKampaiColor(writer, SellPriceUpTextColor);
			global::Kampai.Util.BinarySerializationUtil.WriteKampaiColor(writer, SellPriceUpBackgroundColor);
			global::Kampai.Util.BinarySerializationUtil.WriteKampaiColor(writer, SellPriceDownTextColor);
			global::Kampai.Util.BinarySerializationUtil.WriteKampaiColor(writer, SellPriceDownBackgroundColor);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			itemDefinitions = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, itemDefinitions);
			slotDefinitions = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, slotDefinitions);
			buyTimeSpline = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadVector3, buyTimeSpline);
			refreshTimerDefinition = global::Kampai.Util.BinarySerializationUtil.ReadObject<global::Kampai.Game.MarketplaceRefreshTimerDefinition>(reader);
			MaxSellQuantity = reader.ReadInt32();
			MaxDropQuantity = reader.ReadInt32();
			VariabilityBuyTimePercent = reader.ReadSingle();
			CraftableWeight = reader.ReadInt32();
			BaseResourceWeight = reader.ReadInt32();
			DropWeight = reader.ReadInt32();
			TotalSaleAds = reader.ReadInt32();
			TotalBuyAds = reader.ReadInt32();
			StartingBuyAds = reader.ReadInt32();
			StandardSlots = reader.ReadInt32();
			FacebookSlots = reader.ReadInt32();
			MaxPremiumSlots = reader.ReadInt32();
			PremiumInitialCost = reader.ReadInt32();
			PremiumIncrementCost = reader.ReadInt32();
			DeleteSaleCost = reader.ReadInt32();
			LevelGate = reader.ReadInt32();
			SaleCancellationCost = reader.ReadInt32();
			SellPriceUpTextColor = global::Kampai.Util.BinarySerializationUtil.ReadKampaiColor(reader);
			SellPriceUpBackgroundColor = global::Kampai.Util.BinarySerializationUtil.ReadKampaiColor(reader);
			SellPriceDownTextColor = global::Kampai.Util.BinarySerializationUtil.ReadKampaiColor(reader);
			SellPriceDownBackgroundColor = global::Kampai.Util.BinarySerializationUtil.ReadKampaiColor(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ITEMDEFINITIONS":
				reader.Read();
				itemDefinitions = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, itemDefinitions);
				break;
			case "SLOTDEFINITIONS":
				reader.Read();
				slotDefinitions = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, slotDefinitions);
				break;
			case "BUYTIMESPLINE":
				reader.Read();
				buyTimeSpline = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadVector3, buyTimeSpline);
				break;
			case "REFRESHTIMERDEFINITION":
				reader.Read();
				refreshTimerDefinition = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.MarketplaceRefreshTimerDefinition>(reader, converters);
				break;
			case "MAXSELLQUANTITY":
				reader.Read();
				MaxSellQuantity = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MAXDROPQUANTITY":
				reader.Read();
				MaxDropQuantity = global::System.Convert.ToInt32(reader.Value);
				break;
			case "VARIABILITYBUYTIMEPERCENT":
				reader.Read();
				VariabilityBuyTimePercent = global::System.Convert.ToSingle(reader.Value);
				break;
			case "CRAFTABLEWEIGHT":
				reader.Read();
				CraftableWeight = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BASERESOURCEWEIGHT":
				reader.Read();
				BaseResourceWeight = global::System.Convert.ToInt32(reader.Value);
				break;
			case "DROPWEIGHT":
				reader.Read();
				DropWeight = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TOTALSALEADS":
				reader.Read();
				TotalSaleAds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TOTALBUYADS":
				reader.Read();
				TotalBuyAds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STARTINGBUYADS":
				reader.Read();
				StartingBuyAds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STANDARDSLOTS":
				reader.Read();
				StandardSlots = global::System.Convert.ToInt32(reader.Value);
				break;
			case "FACEBOOKSLOTS":
				reader.Read();
				FacebookSlots = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MAXPREMIUMSLOTS":
				reader.Read();
				MaxPremiumSlots = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREMIUMINITIALCOST":
				reader.Read();
				PremiumInitialCost = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREMIUMINCREMENTCOST":
				reader.Read();
				PremiumIncrementCost = global::System.Convert.ToInt32(reader.Value);
				break;
			case "DELETESALECOST":
				reader.Read();
				DeleteSaleCost = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LEVELGATE":
				reader.Read();
				LevelGate = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SALECANCELLATIONCOST":
				reader.Read();
				SaleCancellationCost = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SELLPRICEUPTEXTCOLOR":
				reader.Read();
				SellPriceUpTextColor = global::Kampai.Util.ReaderUtil.ReadKampaiColor(reader, converters);
				break;
			case "SELLPRICEUPBACKGROUNDCOLOR":
				reader.Read();
				SellPriceUpBackgroundColor = global::Kampai.Util.ReaderUtil.ReadKampaiColor(reader, converters);
				break;
			case "SELLPRICEDOWNTEXTCOLOR":
				reader.Read();
				SellPriceDownTextColor = global::Kampai.Util.ReaderUtil.ReadKampaiColor(reader, converters);
				break;
			case "SELLPRICEDOWNBACKGROUNDCOLOR":
				reader.Read();
				SellPriceDownBackgroundColor = global::Kampai.Util.ReaderUtil.ReadKampaiColor(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}
