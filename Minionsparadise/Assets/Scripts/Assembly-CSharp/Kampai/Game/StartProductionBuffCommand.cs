namespace Kampai.Game
{
	public class StartProductionBuffCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public float multiplier { get; set; }

		[Inject]
		public int currentTime { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		public override void Execute()
		{
			timeEventService.StartBuff(global::Kampai.Game.TimeEventType.ProductionBuff, multiplier, currentTime);
		}
	}
}
