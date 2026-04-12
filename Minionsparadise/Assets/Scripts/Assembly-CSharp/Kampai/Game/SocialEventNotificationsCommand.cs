namespace Kampai.Game
{
	public class SocialEventNotificationsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SocialEventNotificationsCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimedSocialEventService timedSocialEventService { get; set; }

		[Inject]
		public global::Kampai.Game.ScheduleNotificationSignal scheduleNotificationSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
			if (firstInstanceByDefinitionId == null || !firstInstanceByDefinitionId.IsBuildingRepaired())
			{
				logger.Error("The Stage {0} isn't repaired yet!", firstInstanceByDefinitionId);
				return;
			}
			global::Kampai.Game.TimedSocialEventDefinition nextSocialEvent = timedSocialEventService.GetNextSocialEvent();
			if (nextSocialEvent != null)
			{
				global::Kampai.Game.NotificationDefinition definition = null;
				if (definitionService.TryGet<global::Kampai.Game.NotificationDefinition>(10020, out definition))
				{
					int num = timeService.CurrentTime();
					if (nextSocialEvent.StartTime > num)
					{
						definition.Seconds = nextSocialEvent.StartTime - num;
						scheduleNotificationSignal.Dispatch(definition);
					}
					else
					{
						logger.Error("The current time is not greater than the next start time! This indicates an error. Don't schedule the new notification until it is.");
					}
				}
			}
			global::Kampai.Game.TimedSocialEventDefinition currentSocialEvent = timedSocialEventService.GetCurrentSocialEvent();
			if (currentSocialEvent == null)
			{
				return;
			}
			global::Kampai.Game.NotificationDefinition definition2 = null;
			if (definitionService.TryGet<global::Kampai.Game.NotificationDefinition>(10019, out definition2))
			{
				int num2 = timeService.CurrentTime();
				int num3 = 900;
				int num4 = currentSocialEvent.FinishTime - num3;
				if (num4 > num2)
				{
					definition2.Seconds = num4 - num2;
					scheduleNotificationSignal.Dispatch(definition2);
				}
				else
				{
					logger.Info("The current event will be ending in less than 15 minutes. Cancel the notification.");
				}
			}
		}
	}
}
