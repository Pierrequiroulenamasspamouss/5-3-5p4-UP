namespace Kampai.Game
{
	public class EnableAllMinionRenderersCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public bool enable { get; set; }

		[Inject]
		public global::Kampai.Game.EnableMinionRendererSignal enableSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			foreach (global::Kampai.Game.Minion item in playerService.GetInstancesByType<global::Kampai.Game.Minion>())
			{
				enableSignal.Dispatch(item.ID, enable);
			}
		}
	}
}
