namespace Kampai.Game
{
	public class DeselectTaskedMinionsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Common.DeselectMinionSignal deselectMinionSignal { get; set; }

		public override void Execute()
		{
			Deselect();
		}

		private void Deselect()
		{
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			foreach (int key in model.SelectedMinions.Keys)
			{
				global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(key);
				if (byInstanceId.State == global::Kampai.Game.MinionState.Tasking || byInstanceId.State == global::Kampai.Game.MinionState.PlayingMignette || byInstanceId.State == global::Kampai.Game.MinionState.Leisure)
				{
					list.Add(key);
				}
			}
			foreach (int item in list)
			{
				global::Kampai.Game.Minion byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.Minion>(item);
				global::Kampai.Game.MinionState state = byInstanceId2.State;
				deselectMinionSignal.Dispatch(item);
				minionStateChangeSignal.Dispatch(byInstanceId2.ID, state);
			}
		}
	}
}
