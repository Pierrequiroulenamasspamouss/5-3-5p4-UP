namespace Kampai.Game.Trigger
{
	public class UpsellTriggerDefinition : global::Kampai.Game.Trigger.TriggerDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1151;
			}
		}

		public override global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier.Upsell;
			}
		}

		public override global::Kampai.Game.Trigger.TriggerInstance Build()
		{
			return new global::Kampai.Game.Trigger.UpsellTriggerInstance(this);
		}
	}
}
