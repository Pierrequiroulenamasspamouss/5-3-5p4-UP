namespace Kampai.Game.View
{
	internal sealed class SendIDSignalAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.ActionableObject minionObj;

		private global::strange.extensions.signal.impl.Signal<int> signal;

		public SendIDSignalAction(global::Kampai.Game.View.ActionableObject obj, global::strange.extensions.signal.impl.Signal<int> signal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			minionObj = obj;
			this.signal = signal;
		}

		public override void Execute()
		{
			signal.Dispatch(minionObj.ID);
			base.Done = true;
		}
	}
}
