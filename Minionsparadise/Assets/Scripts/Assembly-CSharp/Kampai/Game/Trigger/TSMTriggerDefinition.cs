namespace Kampai.Game.Trigger
{
	public class TSMTriggerDefinition : global::Kampai.Game.Trigger.TriggerDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1149;
			}
		}

		public override global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier.TSM;
			}
		}

		public override global::Kampai.Game.Trigger.TriggerInstance Build()
		{
			return new global::Kampai.Game.Trigger.TSMTriggerInstance(this);
		}
	}
}
