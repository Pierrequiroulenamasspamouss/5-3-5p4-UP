namespace Kampai.Game.Trigger
{
	public class TriggerRewardDefinitionConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.Trigger.TriggerRewardDefinition>
	{
		private global::Kampai.Game.Trigger.TriggerRewardType.Identifier rewardType;

		private global::Kampai.Util.IKampaiLogger logger;

		public TriggerRewardDefinitionConverter(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			rewardType = global::Kampai.Game.Trigger.TriggerRewardType.ReadFromJson(ref reader);
			if (rewardType == global::Kampai.Game.Trigger.TriggerRewardType.Identifier.Unknown)
			{
				return null;
			}
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.Trigger.TriggerRewardDefinition Create(global::System.Type objectType)
		{
			return global::Kampai.Game.Trigger.TriggerRewardType.CreateFromIdentifier(rewardType, logger);
		}
	}
}
