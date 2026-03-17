namespace Kampai.Game.Trigger
{
	public class TriggerConditionDefinitionFastConverter : global::Kampai.Util.FastJsonCreationConverter<global::Kampai.Game.Trigger.TriggerConditionDefinition>
	{
		private global::Kampai.Game.Trigger.TriggerConditionType.Identifier conditionType;

		private global::Kampai.Util.IKampaiLogger logger;

		public TriggerConditionDefinitionFastConverter(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public override global::Kampai.Game.Trigger.TriggerConditionDefinition ReadJson(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			conditionType = global::Kampai.Game.Trigger.TriggerConditionType.ReadFromJson(ref reader);
			if (conditionType == global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Unknown)
			{
				return null;
			}
			return base.ReadJson(reader, converters);
		}

		public override global::Kampai.Game.Trigger.TriggerConditionDefinition Create()
		{
			return global::Kampai.Game.Trigger.TriggerConditionType.CreateFromIdentifier(conditionType, logger);
		}
	}
}
