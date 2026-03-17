namespace Kampai.Game
{
	public class MoveMinionFinishedCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int minionID { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.SelectionCompleteSignal selectionCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Common.DeselectAllMinionsSignal deselectAllMinionsSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionID);
			if (byInstanceId != null && byInstanceId.State == global::Kampai.Game.MinionState.Selected)
			{
				SelectedMinionFinishedRouting();
			}
		}

		private void SelectedMinionFinishedRouting()
		{
			model.SelectedMinions[minionID].FinishedRouting = true;
			bool flag = true;
			global::UnityEngine.Vector3 type = default(global::UnityEngine.Vector3);
			foreach (global::Kampai.Common.SelectedMinionModel value in model.SelectedMinions.Values)
			{
				if (!value.FinishedRouting)
				{
					flag = false;
					break;
				}
				type += value.RunLocation;
			}
			if (flag)
			{
				type /= (float)model.SelectedMinions.Count;
				selectionCompleteSignal.Dispatch(type);
				deselectAllMinionsSignal.Dispatch();
			}
		}
	}
}
