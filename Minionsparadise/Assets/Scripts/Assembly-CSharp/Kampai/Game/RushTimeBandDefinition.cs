namespace Kampai.Game
{
	public class RushTimeBandDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1135;
			}
		}

		public int TimeRemainingInSeconds { get; set; }

		public int PremiumCostConstruction { get; set; }

		public int PremiumCostBaseResource { get; set; }

		public int PremiumCostCraftable { get; set; }

		public int PremiumCostCooldowns { get; set; }

		public int PremiumCostLeisure { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(TimeRemainingInSeconds);
			writer.Write(PremiumCostConstruction);
			writer.Write(PremiumCostBaseResource);
			writer.Write(PremiumCostCraftable);
			writer.Write(PremiumCostCooldowns);
			writer.Write(PremiumCostLeisure);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			TimeRemainingInSeconds = reader.ReadInt32();
			PremiumCostConstruction = reader.ReadInt32();
			PremiumCostBaseResource = reader.ReadInt32();
			PremiumCostCraftable = reader.ReadInt32();
			PremiumCostCooldowns = reader.ReadInt32();
			PremiumCostLeisure = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TIMEREMAININGINSECONDS":
				reader.Read();
				TimeRemainingInSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREMIUMCOSTCONSTRUCTION":
				reader.Read();
				PremiumCostConstruction = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREMIUMCOSTBASERESOURCE":
				reader.Read();
				PremiumCostBaseResource = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREMIUMCOSTCRAFTABLE":
				reader.Read();
				PremiumCostCraftable = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREMIUMCOSTCOOLDOWNS":
				reader.Read();
				PremiumCostCooldowns = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREMIUMCOSTLEISURE":
				reader.Read();
				PremiumCostLeisure = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public int GetCostForRushActionType(global::Kampai.Game.RushActionType rushActionType)
		{
			switch (rushActionType)
			{
			case global::Kampai.Game.RushActionType.CONSTRUCTION:
				return PremiumCostConstruction;
			case global::Kampai.Game.RushActionType.COOLDOWN:
				return PremiumCostCooldowns;
			case global::Kampai.Game.RushActionType.CRAFTING:
				return PremiumCostCraftable;
			case global::Kampai.Game.RushActionType.HARVESTING:
				return PremiumCostBaseResource;
			case global::Kampai.Game.RushActionType.LEISURE:
				return PremiumCostLeisure;
			default:
				return -1;
			}
		}
	}
}
