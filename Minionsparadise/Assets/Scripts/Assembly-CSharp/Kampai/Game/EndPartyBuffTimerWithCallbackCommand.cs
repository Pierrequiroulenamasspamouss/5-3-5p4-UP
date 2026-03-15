namespace Kampai.Game
{
	public class EndPartyBuffTimerWithCallbackCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDiscoGlobeSignal displayDiscoGlobeSignal { get; set; }

		[Inject]
		public global::Kampai.Util.Boxed<global::System.Action> onCompleteEvent { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			int timePassedSinceBuffStarts = timeService.CurrentTime() - minionPartyInstance.BuffStartTime;
			int buffStartTime = minionPartyInstance.BuffStartTime;
			minionPartyInstance.BuffStartTime = 0;
			minionPartyInstance.NewBuffStartTime = 0;
			minionPartyInstance.IsBuffHappening = false;
			guestService.StopBuff(timePassedSinceBuffStarts, buffStartTime);
			playerService.UpdateMinionPartyPointValues();
			if (onCompleteEvent.Value != null)
			{
				onCompleteEvent.Value();
			}
			else
			{
				displayDiscoGlobeSignal.Dispatch(false);
			}
		}
	}
}
