namespace Kampai.Game.Trigger
{
	public static class TriggerDefinitionType
	{
		public enum Identifier
		{
			Unknown = 0,
			TSM = 1,
			Upsell = 2
		}

		public static global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier ParseIdentifier(string identifier)
		{
			if (string.IsNullOrEmpty(identifier))
			{
				return global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier.Unknown;
			}
			return (global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier), identifier, true);
		}

		public static global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier ReadFromJson(ref global::Newtonsoft.Json.JsonReader reader)
		{
			global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier result = global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier.Unknown;
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

		public static global::Kampai.Game.Trigger.TriggerDefinition CreateDefinitionFromIdentifier(global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier triggerType, global::Kampai.Util.IKampaiLogger logger)
		{
			switch (triggerType)
			{
			case global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier.TSM:
				return new global::Kampai.Game.Trigger.TSMTriggerDefinition();
			case global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier.Upsell:
				return new global::Kampai.Game.Trigger.UpsellTriggerDefinition();
			default:
				logger.Error("Invalid Trigger Definition type: {0}", triggerType);
				return null;
			}
		}
	}
}
