namespace Kampai.Game.Trigger
{
	public class TriggerDefinitionFastConverter : global::Kampai.Util.FastJsonCreationConverter<global::Kampai.Game.Trigger.TriggerDefinition>
	{
		private global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier triggerType;

		private global::Kampai.Util.IKampaiLogger logger;

		public TriggerDefinitionFastConverter(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public override global::Kampai.Game.Trigger.TriggerDefinition ReadJson(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			triggerType = global::Kampai.Game.Trigger.TriggerDefinitionType.ReadFromJson(ref reader);
			if (triggerType == global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier.Unknown)
			{
				return null;
			}
			return base.ReadJson(reader, converters);
		}

		public override global::Kampai.Game.Trigger.TriggerDefinition Create()
		{
			return global::Kampai.Game.Trigger.TriggerDefinitionType.CreateDefinitionFromIdentifier(triggerType, logger);
		}
	}
}
