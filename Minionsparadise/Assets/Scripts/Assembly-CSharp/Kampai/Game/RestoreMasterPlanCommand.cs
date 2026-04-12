namespace Kampai.Game
{
	public class RestoreMasterPlanCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanCooldownCompleteSignal cooldownCompleteSignal { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.MasterPlan> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MasterPlan>();
			for (int i = 0; i < instancesByType.Count; i++)
			{
				global::Kampai.Game.MasterPlan masterPlan = instancesByType[i];
				int cooldownUTCStartTime = masterPlan.cooldownUTCStartTime;
				if (masterPlan.cooldownUTCStartTime > 0)
				{
					int num = timeService.CurrentTime() - masterPlan.cooldownUTCStartTime;
					if (num >= masterPlan.Definition.CooldownDuration)
					{
						cooldownCompleteSignal.Dispatch(masterPlan.ID);
					}
					else
					{
						timeEventService.AddEvent(masterPlan.ID, cooldownUTCStartTime, masterPlan.Definition.CooldownDuration, cooldownCompleteSignal);
					}
				}
			}
		}
	}
}
