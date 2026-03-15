namespace Kampai.Game
{
	public class QuestSignal : global::strange.extensions.signal.impl.Signal
	{
		[Inject]
		public global::Kampai.Game.QuestScriptKernel kernel { get; set; }

		public new virtual void Dispatch()
		{
			base.Dispatch();
			kernel.SignalDispatched<object, object, object, object>(GetType().Name, 0, null, null, null, null);
		}
	}
	public class QuestSignal<T> : global::strange.extensions.signal.impl.Signal<T>
	{
		[Inject]
		public global::Kampai.Game.QuestScriptKernel kernel { get; set; }

		public new void Dispatch(T param1)
		{
			base.Dispatch(param1);
			kernel.SignalDispatched<T, object, object, object>(GetType().Name, 1, param1, null, null, null);
		}
	}
	public class QuestSignal<T, U> : global::strange.extensions.signal.impl.Signal<T, U>
	{
		[Inject]
		public global::Kampai.Game.QuestScriptKernel kernel { get; set; }

		public new void Dispatch(T param1, U param2)
		{
			base.Dispatch(param1, param2);
			kernel.SignalDispatched<T, U, object, object>(GetType().Name, 2, param1, param2, null, null);
		}
	}
	public class QuestSignal<T, U, V> : global::strange.extensions.signal.impl.Signal<T, U, V>
	{
		[Inject]
		public global::Kampai.Game.QuestScriptKernel kernel { get; set; }

		public new void Dispatch(T param1, U param2, V param3)
		{
			base.Dispatch(param1, param2, param3);
			kernel.SignalDispatched<T, U, V, object>(GetType().Name, 3, param1, param2, param3, null);
		}
	}
}
