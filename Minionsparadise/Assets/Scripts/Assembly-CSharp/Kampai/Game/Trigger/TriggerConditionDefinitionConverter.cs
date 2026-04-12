namespace Kampai.Game.Trigger
{
	public class TriggerConditionDefinitionConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.Trigger.TriggerConditionDefinition>
	{
		private global::Kampai.Game.Trigger.TriggerConditionType.Identifier conditionType;

		private global::Kampai.Util.IKampaiLogger logger;

		public TriggerConditionDefinitionConverter(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			conditionType = global::Kampai.Game.Trigger.TriggerConditionType.ReadFromJson(ref reader);
			if (conditionType == global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Unknown)
			{
				return null;
			}
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.Trigger.TriggerConditionDefinition Create(global::System.Type objectType)
		{
			return global::Kampai.Game.Trigger.TriggerConditionType.CreateFromIdentifier(conditionType, logger);
		}
	}
}
