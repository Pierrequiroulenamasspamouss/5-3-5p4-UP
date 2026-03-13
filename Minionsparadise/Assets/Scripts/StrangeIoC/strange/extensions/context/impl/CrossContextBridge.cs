namespace strange.extensions.context.impl
{
	public class CrossContextBridge : global::strange.framework.impl.Binder, global::strange.extensions.dispatcher.api.ITriggerable
	{
		protected global::System.Collections.Generic.HashSet<object> eventsInProgress = new global::System.Collections.Generic.HashSet<object>();

		[Inject(global::strange.extensions.context.api.ContextKeys.CROSS_CONTEXT_DISPATCHER)]
		public global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher crossContextDispatcher { get; set; }

		public override global::strange.framework.api.IBinding Bind(object key)
		{
			global::strange.framework.api.IBinding rawBinding = GetRawBinding();
			rawBinding.Bind(key);
			resolver(rawBinding);
			return rawBinding;
		}

		public bool Trigger<T>(object data)
		{
			return Trigger(typeof(T), data);
		}

		public bool Trigger(object key, object data)
		{
			global::strange.framework.api.IBinding binding = GetBinding(key, null);
			if (binding != null && !eventsInProgress.Contains(key))
			{
				eventsInProgress.Add(key);
				crossContextDispatcher.Dispatch(key, data);
				eventsInProgress.Remove(key);
			}
			return true;
		}
	}
}
