public class ScheduleCooldownCommand : global::strange.extensions.command.impl.Command
{
	[Inject]
	public global::Kampai.Util.Tuple<int, bool> commandParams { get; set; }

	[Inject]
	public bool triggerStateChange { get; set; }

	[Inject]
	public global::Kampai.Game.BuildingCooldownCompleteSignal cooldownCompleteSignal { get; set; }

	[Inject]
	public global::Kampai.Game.BuildingCooldownUpdateViewSignal cooldownUpdateViewSignal { get; set; }

	[Inject]
	public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

	[Inject]
	public global::Kampai.Game.MignetteNotificationSignal mignetteNotificationSignal { get; set; }

	[Inject]
	public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

	[Inject]
	public global::Kampai.Game.IPlayerService playerService { get; set; }

	[Inject]
	public global::Kampai.Game.IDevicePrefsService devicePrefsService { get; set; }

	public override void Execute()
	{
		int item = commandParams.Item1;
		bool item2 = commandParams.Item2;
		global::Kampai.Game.IBuildingWithCooldown byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.IBuildingWithCooldown>(item);
		if (byInstanceId == null || byInstanceId.GetCooldown() <= 0)
		{
			cooldownCompleteSignal.Dispatch(item);
			return;
		}
		if (triggerStateChange)
		{
			buildingChangeStateSignal.Dispatch(item, global::Kampai.Game.BuildingState.Cooldown);
		}
		timeEventService.AddEvent(item, byInstanceId.StateStartTime, byInstanceId.GetCooldown(), cooldownCompleteSignal);
		if (devicePrefsService.GetDevicePrefs().MinionsParadiseNotif)
		{
			mignetteNotificationSignal.Dispatch(false, byInstanceId.ID);
		}
		if (item2)
		{
			int num = byInstanceId.GetCooldown() / 10;
			for (int i = 0; i < 10; i++)
			{
				timeEventService.AddEvent(item, byInstanceId.StateStartTime, i * num, cooldownUpdateViewSignal);
			}
		}
	}
}
