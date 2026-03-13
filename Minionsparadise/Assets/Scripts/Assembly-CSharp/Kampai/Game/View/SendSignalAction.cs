namespace Kampai.Game.View
{
	internal sealed class SendSignalAction : global::Kampai.Game.View.KampaiAction
	{
		private global::strange.extensions.signal.impl.Signal signal;

		public SendSignalAction(global::strange.extensions.signal.impl.Signal signal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.signal = signal;
		}

		public override void Execute()
		{
			signal.Dispatch();
			base.Done = true;
		}
	}
}
