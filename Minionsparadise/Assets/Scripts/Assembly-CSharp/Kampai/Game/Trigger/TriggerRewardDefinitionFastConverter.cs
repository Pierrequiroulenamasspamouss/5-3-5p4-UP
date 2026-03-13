namespace Kampai.Game.Trigger
{
	public class TriggerRewardDefinitionFastConverter : global::Kampai.Util.FastJsonCreationConverter<global::Kampai.Game.Trigger.TriggerRewardDefinition>
	{
		private global::Kampai.Game.Trigger.TriggerRewardType.Identifier rewardType;

		private global::Kampai.Util.IKampaiLogger logger;

		public TriggerRewardDefinitionFastConverter(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public override global::Kampai.Game.Trigger.TriggerRewardDefinition ReadJson(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			rewardType = global::Kampai.Game.Trigger.TriggerRewardType.ReadFromJson(ref reader);
			if (rewardType == global::Kampai.Game.Trigger.TriggerRewardType.Identifier.Unknown)
			{
				return null;
			}
			return base.ReadJson(reader, converters);
		}

		public override global::Kampai.Game.Trigger.TriggerRewardDefinition Create()
		{
			return global::Kampai.Game.Trigger.TriggerRewardType.CreateFromIdentifier(rewardType, logger);
		}
	}
}
