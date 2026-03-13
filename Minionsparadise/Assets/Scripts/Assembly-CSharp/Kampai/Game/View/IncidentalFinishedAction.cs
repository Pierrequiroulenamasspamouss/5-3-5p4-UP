namespace Kampai.Game.View
{
	internal sealed class IncidentalFinishedAction : global::Kampai.Game.View.KampaiAction
	{
		private int id;

		private global::Kampai.Game.MinionStateChangeSignal stateChangeSignal;

		public IncidentalFinishedAction(int id, global::Kampai.Game.MinionStateChangeSignal stateChangeSignal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.id = id;
			this.stateChangeSignal = stateChangeSignal;
		}

		public override void Execute()
		{
			stateChangeSignal.Dispatch(id, global::Kampai.Game.MinionState.Idle);
			base.Done = true;
		}
	}
}
