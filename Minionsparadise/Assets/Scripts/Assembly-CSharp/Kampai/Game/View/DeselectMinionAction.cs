namespace Kampai.Game.View
{
	internal sealed class DeselectMinionAction : global::Kampai.Game.View.KampaiAction
	{
		private int minionID;

		private global::strange.extensions.signal.impl.Signal<int> signal;

		public DeselectMinionAction(int minionID, global::strange.extensions.signal.impl.Signal<int> signal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.minionID = minionID;
			this.signal = signal;
		}

		public override void Abort()
		{
			base.Done = true;
		}

		public override void Execute()
		{
			signal.Dispatch(minionID);
			base.Done = true;
		}
	}
}
