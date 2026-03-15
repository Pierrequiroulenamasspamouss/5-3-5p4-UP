namespace Kampai.Game
{
	public class QueueCabanaCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Prestige prestige { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		public override void Execute()
		{
			if (prestigeService.GetEmptyCabana() != null)
			{
				prestigeService.ChangeToPrestigeState(prestige, global::Kampai.Game.PrestigeState.Questing);
			}
			else
			{
				playerService.QueueVillain(prestige);
			}
		}
	}
}
