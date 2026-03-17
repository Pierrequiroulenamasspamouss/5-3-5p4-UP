namespace Kampai.Game
{
	public class StartGameTimeTrackingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerDurationService playerDurationService { get; set; }

		[Inject]
		public global::Kampai.Game.IGameTimeTracker gameTimeTracker { get; set; }

		public override void Execute()
		{
			gameTimeTracker.StartGameTime = playerDurationService.TotalGamePlaySeconds;
		}
	}
}
