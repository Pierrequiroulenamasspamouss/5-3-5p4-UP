namespace Kampai.Game
{
	public class RushMinionTaskCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int minionID { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		public override void Execute()
		{
			timeEventService.RushEvent(minionID);
		}
	}
}
