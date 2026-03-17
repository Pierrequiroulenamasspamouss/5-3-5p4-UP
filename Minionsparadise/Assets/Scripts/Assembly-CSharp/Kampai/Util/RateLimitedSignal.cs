namespace Kampai.Util
{
	public abstract class RateLimitedSignal : global::strange.extensions.signal.impl.Signal, global::Kampai.Util.RateLimitedSignalProvider
	{
		private float lastDispatch = float.MinValue;

		public abstract float MinimumGap { get; }

		public abstract float CurrentTime { get; }

		public new virtual void Dispatch()
		{
			if (CanDispatch(this, ref lastDispatch))
			{
				base.Dispatch();
			}
		}

		public static bool CanDispatch(global::Kampai.Util.RateLimitedSignalProvider provider, ref float lastDispatch)
		{
			float currentTime = provider.CurrentTime;
			if (currentTime - lastDispatch > provider.MinimumGap)
			{
				lastDispatch = currentTime;
				return true;
			}
			return false;
		}
	}
	public abstract class RateLimitedSignal<T> : global::strange.extensions.signal.impl.Signal<T>, global::Kampai.Util.RateLimitedSignalProvider
	{
		private float lastDispatch = float.MinValue;

		public abstract float MinimumGap { get; }

		public abstract float CurrentTime { get; }

		public new void Dispatch(T param1)
		{
			if (global::Kampai.Util.RateLimitedSignal.CanDispatch(this, ref lastDispatch))
			{
				base.Dispatch(param1);
			}
		}
	}
	public abstract class RateLimitedSignal<T, U> : global::strange.extensions.signal.impl.Signal<T, U>, global::Kampai.Util.RateLimitedSignalProvider
	{
		private float lastDispatch = float.MinValue;

		public abstract float MinimumGap { get; }

		public abstract float CurrentTime { get; }

		public new void Dispatch(T param1, U param2)
		{
			if (global::Kampai.Util.RateLimitedSignal.CanDispatch(this, ref lastDispatch))
			{
				base.Dispatch(param1, param2);
			}
		}
	}
	public abstract class RateLimitedSignal<T, U, V> : global::strange.extensions.signal.impl.Signal<T, U, V>, global::Kampai.Util.RateLimitedSignalProvider
	{
		private float lastDispatch = float.MinValue;

		public abstract float MinimumGap { get; }

		public abstract float CurrentTime { get; }

		public new void Dispatch(T param1, U param2, V param3)
		{
			if (global::Kampai.Util.RateLimitedSignal.CanDispatch(this, ref lastDispatch))
			{
				base.Dispatch(param1, param2, param3);
			}
		}
	}
}
