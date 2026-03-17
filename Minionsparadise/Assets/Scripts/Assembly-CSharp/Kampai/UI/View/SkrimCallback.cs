namespace Kampai.UI.View
{
	public class SkrimCallback
	{
		public global::strange.extensions.signal.impl.Signal<global::Kampai.Util.KampaiDisposable> Callback { get; private set; }

		public SkrimCallback(global::strange.extensions.signal.impl.Signal<global::Kampai.Util.KampaiDisposable> signal)
		{
			Callback = signal;
		}
	}
}
