namespace Kampai.Game
{
	public class StartPartyPointBuffCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public float multiplier { get; set; }

		[Inject]
		public int currentTime { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		public override void Execute()
		{
			timeEventService.StartBuff(global::Kampai.Game.TimeEventType.LeisureBuff, multiplier, currentTime);
		}
	}
}
