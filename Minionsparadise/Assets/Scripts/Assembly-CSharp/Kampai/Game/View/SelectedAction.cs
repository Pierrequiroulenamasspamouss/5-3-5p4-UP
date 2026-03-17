namespace Kampai.Game.View
{
	public class SelectedAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Util.Boxed<global::UnityEngine.Vector3> RunLocation;

		private global::Kampai.Game.MinionMoveToSignal MoveSignal;

		private int MinionId;

		public SelectedAction(int minionId, global::Kampai.Util.Boxed<global::UnityEngine.Vector3> runLocation, global::Kampai.Game.MinionMoveToSignal moveSignal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			RunLocation = runLocation;
			MoveSignal = moveSignal;
			MinionId = minionId;
		}

		public override void Execute()
		{
			if (RunLocation != null)
			{
				MoveSignal.Dispatch(MinionId, RunLocation.Value, false);
			}
			else
			{
				base.Done = true;
			}
		}
	}
}
