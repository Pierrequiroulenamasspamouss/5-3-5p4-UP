namespace Kampai.Game
{
	public class StartPartyFavorAnimationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Util.IPartyFavorAnimationService partyFavorService { get; set; }

		public override void Execute()
		{
			partyFavorService.CreateRandomPartyFavor();
		}
	}
}
