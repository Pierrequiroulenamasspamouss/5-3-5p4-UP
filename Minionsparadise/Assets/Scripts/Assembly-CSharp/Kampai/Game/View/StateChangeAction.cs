namespace Kampai.Game.View
{
	public class StateChangeAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.MinionStateChangeSignal StateChangeSignal;

		private int MinionId;

		private global::Kampai.Game.MinionState NewState;

		public StateChangeAction(int minionId, global::Kampai.Game.MinionStateChangeSignal stateChangeSignal, global::Kampai.Game.MinionState newState, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			StateChangeSignal = stateChangeSignal;
			MinionId = minionId;
			NewState = newState;
		}

		public override void Abort()
		{
			base.Done = true;
		}

		public override void Execute()
		{
			StateChangeSignal.Dispatch(MinionId, NewState);
			base.Done = true;
		}
	}
}
