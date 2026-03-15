namespace Kampai.Game
{
	public class DeselectMinionCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int minionID { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal stateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		public override void Execute()
		{
			if (!model.HasPlayedSFX)
			{
				soundFXSignal.Dispatch("Play_minion_deselect_01");
				model.HasPlayedSFX = true;
			}
			stateChangeSignal.Dispatch(minionID, global::Kampai.Game.MinionState.Idle);
			routineRunner.StopTimer("MinionSelectionComplete");
			global::Kampai.Util.Point item = new global::Kampai.Util.Point(-1, -1);
			if (model.Points != null)
			{
				global::Kampai.Common.SelectedMinionModel selectedMinionModel = model.SelectedMinions[minionID];
				item = new global::Kampai.Util.Point(selectedMinionModel.RunLocation.x, selectedMinionModel.RunLocation.z);
				model.Points.Enqueue(item);
			}
			model.SelectedMinions.Remove(minionID);
			if (model.SelectedMinions.Count == 0)
			{
				model.HasPlayedGacha = false;
			}
			else
			{
				if (model.Points == null || !item.Equals(model.MainPoint))
				{
					return;
				}
				using (global::System.Collections.Generic.Dictionary<int, global::Kampai.Common.SelectedMinionModel>.KeyCollection.Enumerator enumerator = model.SelectedMinions.Keys.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						int current = enumerator.Current;
						global::Kampai.Common.SelectedMinionModel selectedMinionModel2 = model.SelectedMinions[current];
						model.MainPoint = new global::Kampai.Util.Point(selectedMinionModel2.RunLocation.x, selectedMinionModel2.RunLocation.z);
					}
				}
			}
		}
	}
}
