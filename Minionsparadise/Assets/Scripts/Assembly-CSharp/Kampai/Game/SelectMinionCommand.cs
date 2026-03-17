namespace Kampai.Game
{
	public class SelectMinionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SelectMinionCommand") as global::Kampai.Util.IKampaiLogger;

		private bool selected;

		[Inject]
		public int id { get; set; }

		[Inject]
		public bool mute { get; set; }

		[Inject]
		public global::Kampai.Util.Boxed<global::UnityEngine.Vector3> runLocation { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.Game.AnimateSelectedMinionSignal animateSelectedMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal stateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService player { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Minion byInstanceId = player.GetByInstanceId<global::Kampai.Game.Minion>(id);
			if (byInstanceId == null)
			{
				logger.Error("SelectMinionCommand: Cannot find minion with id of {0}.", id);
				return;
			}
			if (byInstanceId.State == global::Kampai.Game.MinionState.Idle || byInstanceId.State == global::Kampai.Game.MinionState.Selectable || byInstanceId.State == global::Kampai.Game.MinionState.WaitingOnMagnetFinger || byInstanceId.State == global::Kampai.Game.MinionState.Selected || byInstanceId.State == global::Kampai.Game.MinionState.Uninitialized)
			{
				HandleMinionSelected(byInstanceId);
			}
			if (!selected)
			{
				global::UnityEngine.Vector3 value = runLocation.Value;
				model.Points.Enqueue(new global::Kampai.Util.Point(value.x, value.z));
			}
		}

		private void HandleMinionSelected(global::Kampai.Game.Minion minionModel)
		{
			global::Kampai.Game.SelectMinionState selectMinionState = new global::Kampai.Game.SelectMinionState();
			selectMinionState.minionID = id;
			selectMinionState.runLocation = runLocation;
			selectMinionState.muteStatus = mute;
			selectMinionState.triggerIncidentalAnimation = model.CurrentMode != global::Kampai.Common.PickControllerModel.Mode.MagnetFinger;
			animateSelectedMinionSignal.Dispatch(selectMinionState);
			if (minionModel.State != global::Kampai.Game.MinionState.Selected)
			{
				global::Kampai.Common.SelectedMinionModel selectedMinionModel = new global::Kampai.Common.SelectedMinionModel();
				selectedMinionModel.ID = id;
				selectedMinionModel.FinishedRouting = false;
				selectedMinionModel.RunLocation = runLocation.Value;
				model.SelectedMinions.Add(id, selectedMinionModel);
				model.HasPlayedSFX = false;
				stateChangeSignal.Dispatch(id, global::Kampai.Game.MinionState.Selected);
				selected = true;
			}
		}
	}
}
