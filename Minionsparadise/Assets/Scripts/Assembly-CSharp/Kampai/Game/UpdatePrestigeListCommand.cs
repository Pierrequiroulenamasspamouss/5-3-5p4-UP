namespace Kampai.Game
{
	public class UpdatePrestigeListCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		public override void Execute()
		{
			prestigeService.UpdateEligiblePrestigeList();
		}
	}
}
