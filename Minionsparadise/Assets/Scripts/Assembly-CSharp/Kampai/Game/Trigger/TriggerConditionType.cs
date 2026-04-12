namespace Kampai.Game.Trigger
{
	public static class TriggerConditionType
	{
		public enum Identifier
		{
			Unknown = 0,
			QuantityItem = 1,
			Storage = 2,
			MarketplaceSaleSlot = 3,
			MarketplaceSaleItem = 4,
			Quest = 5,
			Purchase = 6,
			OrderBoard = 7,
			Prestige = 8,
			MignetteScore = 9,
			LandExpansion = 10,
			SocialOrder = 11,
			SocialTime = 12,
			Platform = 13,
			Segment = 14,
			HelpButton = 15,
			Country = 16,
			Churn = 17,
			AvailableLand = 18,
			PrestigeState = 19,
			PrestigeLevel = 20,
			HoursPlayed = 21,
			DaysSinceInstall = 22,
			SessionCount = 23,
			ConsecutiveDays = 24
		}

		public static global::Kampai.Game.Trigger.TriggerConditionType.Identifier ParseIdentifier(string identifier)
		{
			if (string.IsNullOrEmpty(identifier))
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Unknown;
			}
			return (global::Kampai.Game.Trigger.TriggerConditionType.Identifier)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.Trigger.TriggerConditionType.Identifier), identifier, true);
		}

		public static global::Kampai.Game.Trigger.TriggerConditionType.Identifier ReadFromJson(ref global::Newtonsoft.Json.JsonReader reader)
		{
			global::Kampai.Game.Trigger.TriggerConditionType.Identifier result = global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Unknown;
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return result;
			}
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			if (jObject.Property("type") != null)
			{
				string identifier = jObject.Property("type").Value.ToString();
				result = ParseIdentifier(identifier);
			}
			reader = jObject.CreateReader();
			return result;
		}

		public static global::Kampai.Game.Trigger.TriggerConditionDefinition CreateFromIdentifier(global::Kampai.Game.Trigger.TriggerConditionType.Identifier conditionType, global::Kampai.Util.IKampaiLogger logger)
		{
			switch (conditionType)
			{
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.QuantityItem:
				return new global::Kampai.Game.Trigger.QuantityItemTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Storage:
				return new global::Kampai.Game.Trigger.StorageTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.MarketplaceSaleSlot:
				return new global::Kampai.Game.Trigger.SaleSlotTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.MarketplaceSaleItem:
				return new global::Kampai.Game.Trigger.SaleItemTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Quest:
				return new global::Kampai.Game.Trigger.QuestTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Purchase:
				return new global::Kampai.Game.Trigger.PurchaseTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.OrderBoard:
				return new global::Kampai.Game.Trigger.OrderBoardTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Prestige:
				return new global::Kampai.Game.Trigger.PrestigeTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.SocialOrder:
				return new global::Kampai.Game.Trigger.SocialOrderTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.SocialTime:
				return new global::Kampai.Game.Trigger.SocialTimeTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Platform:
				return new global::Kampai.Game.Trigger.PlatformTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Segment:
				return new global::Kampai.Game.Trigger.SegmentTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Country:
				return new global::Kampai.Game.Trigger.CountryTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.MignetteScore:
				return new global::Kampai.Game.Trigger.MignetteScoreTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.LandExpansion:
				return new global::Kampai.Game.Trigger.LandExpansionTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.HelpButton:
				return new global::Kampai.Game.Trigger.HelpButtonTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Churn:
				return new global::Kampai.Game.Trigger.ChurnTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.AvailableLand:
				return new global::Kampai.Game.Trigger.AvailableLandTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.PrestigeState:
				return new global::Kampai.Game.Trigger.PrestigeStateTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.PrestigeLevel:
				return new global::Kampai.Game.Trigger.PrestigeLevelTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.HoursPlayed:
				return new global::Kampai.Game.Trigger.HoursPlayedTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.DaysSinceInstall:
				return new global::Kampai.Game.Trigger.DaysSinceInstallTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.SessionCount:
				return new global::Kampai.Game.Trigger.SessionCountTriggerConditionDefinition();
			case global::Kampai.Game.Trigger.TriggerConditionType.Identifier.ConsecutiveDays:
				return new global::Kampai.Game.Trigger.ConsecutiveDaysConditionDefinition();
			default:
				logger.Error("Invalid Trigger Definition type: {0}", conditionType);
				return null;
			}
		}
	}
}
