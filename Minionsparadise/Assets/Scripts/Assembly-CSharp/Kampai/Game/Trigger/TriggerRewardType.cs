namespace Kampai.Game.Trigger
{
	public static class TriggerRewardType
	{
		public enum Identifier
		{
			Unknown = 0,
			QuantityItem = 1,
			MignetteScore = 2,
			MarketplaceSaleSlot = 3,
			MarketplaceSaleItem = 4,
			PartyPoints = 5,
			Upsell = 6,
			SocialOrder = 7,
			TSMMesssage = 8,
			CaptainTease = 9
		}

		public static global::Kampai.Game.Trigger.TriggerRewardType.Identifier ParseIdentifier(string identifier)
		{
			if (string.IsNullOrEmpty(identifier))
			{
				return global::Kampai.Game.Trigger.TriggerRewardType.Identifier.Unknown;
			}
			return (global::Kampai.Game.Trigger.TriggerRewardType.Identifier)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.Trigger.TriggerRewardType.Identifier), identifier, true);
		}

		public static global::Kampai.Game.Trigger.TriggerRewardType.Identifier ReadFromJson(ref global::Newtonsoft.Json.JsonReader reader)
		{
			global::Kampai.Game.Trigger.TriggerRewardType.Identifier result = global::Kampai.Game.Trigger.TriggerRewardType.Identifier.Unknown;
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

		public static global::Kampai.Game.Trigger.TriggerRewardDefinition CreateFromIdentifier(global::Kampai.Game.Trigger.TriggerRewardType.Identifier conditionType, global::Kampai.Util.IKampaiLogger logger)
		{
			switch (conditionType)
			{
			case global::Kampai.Game.Trigger.TriggerRewardType.Identifier.QuantityItem:
				return new global::Kampai.Game.Trigger.QuantityItemTriggerRewardDefinition();
			case global::Kampai.Game.Trigger.TriggerRewardType.Identifier.MignetteScore:
				return new global::Kampai.Game.Trigger.MignetteScoreTriggerRewardDefinition();
			case global::Kampai.Game.Trigger.TriggerRewardType.Identifier.MarketplaceSaleSlot:
				return new global::Kampai.Game.Trigger.SaleSlotTriggerRewardDefinition();
			case global::Kampai.Game.Trigger.TriggerRewardType.Identifier.MarketplaceSaleItem:
				return new global::Kampai.Game.Trigger.SaleItemTriggerRewardDefinition();
			case global::Kampai.Game.Trigger.TriggerRewardType.Identifier.PartyPoints:
				return new global::Kampai.Game.Trigger.PartyPointsTriggerRewardDefinition();
			case global::Kampai.Game.Trigger.TriggerRewardType.Identifier.Upsell:
				return new global::Kampai.Game.Trigger.UpsellTriggerRewardDefinition();
			case global::Kampai.Game.Trigger.TriggerRewardType.Identifier.SocialOrder:
				return new global::Kampai.Game.Trigger.SocialOrderTriggerRewardDefinition();
			case global::Kampai.Game.Trigger.TriggerRewardType.Identifier.TSMMesssage:
				return new global::Kampai.Game.Trigger.TSMMessageTriggerRewardDefinition();
			case global::Kampai.Game.Trigger.TriggerRewardType.Identifier.CaptainTease:
				return new global::Kampai.Game.Trigger.CaptainTeaseTriggerRewardDefinition();
			default:
				logger.Error("Invalid Trigger Definition type: {0}", conditionType);
				return null;
			}
		}
	}
}
