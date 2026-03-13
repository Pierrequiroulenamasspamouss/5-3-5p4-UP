namespace Kampai.Game.Trigger
{
	public class ValidateCurrentTriggerCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.ITriggerService triggerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		[Inject]
		public global::Kampai.Game.CheckTriggersSignal checkTriggersSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Trigger.TriggerInstance activeTrigger = triggerService.ActiveTrigger;
			if (activeTrigger != null && activeTrigger.StartGameTime == 0 && !activeTrigger.IsTriggered(gameContext))
			{
				activeTrigger.StartGameTime = playerDurationService.TotalGamePlaySeconds;
				checkTriggersSignal.Dispatch(301);
			}
		}
	}
}
