namespace Kampai.Game.View
{
	internal sealed class SignalAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.ActionableObject minionObj;

		private global::strange.extensions.signal.impl.Signal<int> signal;

		public SignalAction(global::Kampai.Game.View.ActionableObject obj, global::strange.extensions.signal.impl.Signal<int> signal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			minionObj = obj;
			this.signal = signal;
			signal.AddListener(Callback);
		}

		public override void Abort()
		{
			signal.RemoveListener(Callback);
			base.Done = true;
		}

		private void Callback(int id)
		{
			if (!base.Done && (minionObj.currentAction == this || minionObj.GetNextAction() == this) && minionObj.ID == id)
			{
				base.Done = true;
				signal.RemoveListener(Callback);
			}
		}
	}
}
