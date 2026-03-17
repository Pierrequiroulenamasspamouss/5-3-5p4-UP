namespace Kampai.Game.Trigger
{
	public class TSMMessageTriggerRewardDefinition : global::Kampai.Game.Trigger.TriggerRewardDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1185;
			}
		}

		public override global::Kampai.Game.Trigger.TriggerRewardType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerRewardType.Identifier.TSMMesssage;
			}
		}

		public override bool IsUniquePerInstance
		{
			get
			{
				return false;
			}
		}

		public override void RewardPlayer(global::strange.extensions.context.api.ICrossContextCapable context)
		{
		}
	}
}
