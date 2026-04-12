namespace Kampai.Game.Trigger
{
	public class TriggerDefinitionConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.Trigger.TriggerDefinition>
	{
		private global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier triggerType;

		private global::Kampai.Util.IKampaiLogger logger;

		public TriggerDefinitionConverter(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			triggerType = global::Kampai.Game.Trigger.TriggerDefinitionType.ReadFromJson(ref reader);
			if (triggerType == global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier.Unknown)
			{
				return null;
			}
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.Trigger.TriggerDefinition Create(global::System.Type objectType)
		{
			return global::Kampai.Game.Trigger.TriggerDefinitionType.CreateDefinitionFromIdentifier(triggerType, logger);
		}
	}
}
