namespace Kampai.Util
{
	public class ActionSignal : global::strange.extensions.signal.impl.Signal
	{
		private global::System.Action action;

		private bool onlyFireOnce;

		private bool alreadyFired;

		public ActionSignal(global::System.Action action, bool onlyFireOnce = false)
		{
			this.action = action;
			this.onlyFireOnce = onlyFireOnce;
			AddListener(PerformAction);
		}

		private void PerformAction()
		{
			if (!onlyFireOnce || !alreadyFired)
			{
				action();
			}
		}
	}
}
