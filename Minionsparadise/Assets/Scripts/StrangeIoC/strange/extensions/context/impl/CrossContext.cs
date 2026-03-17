namespace strange.extensions.context.impl
{
	public class CrossContext : global::strange.extensions.context.impl.Context, global::strange.extensions.context.api.ICrossContextCapable
	{
		private global::strange.extensions.injector.api.ICrossContextInjectionBinder _injectionBinder;

		private global::strange.framework.api.IBinder _crossContextBridge;

		protected global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher _crossContextDispatcher;

		public global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder
		{
			get
			{
				return _injectionBinder ?? (_injectionBinder = new global::strange.extensions.injector.impl.CrossContextInjectionBinder());
			}
			set
			{
				_injectionBinder = value;
			}
		}

		public virtual global::strange.extensions.dispatcher.api.IDispatcher crossContextDispatcher
		{
			get
			{
				return _crossContextDispatcher;
			}
			set
			{
				_crossContextDispatcher = value as global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher;
			}
		}

		public virtual global::strange.framework.api.IBinder crossContextBridge
		{
			get
			{
				if (_crossContextBridge == null)
				{
					_crossContextBridge = injectionBinder.GetInstance<global::strange.extensions.context.impl.CrossContextBridge>();
				}
				return _crossContextBridge;
			}
			set
			{
				_crossContextDispatcher = value as global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher;
			}
		}

		public CrossContext()
		{
		}

		public CrossContext(object view)
			: base(view)
		{
		}

		public CrossContext(object view, global::strange.extensions.context.api.ContextStartupFlags flags)
			: base(view, flags)
		{
		}

		public CrossContext(object view, bool autoMapping)
			: base(view, autoMapping)
		{
		}

		protected override void addCoreComponents()
		{
			base.addCoreComponents();
			if (injectionBinder.CrossContextBinder == null)
			{
				injectionBinder.CrossContextBinder = new global::strange.extensions.injector.impl.CrossContextInjectionBinder();
			}
			if (global::strange.extensions.context.impl.Context.firstContext == this)
			{
				injectionBinder.Bind<global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher>().To<global::strange.extensions.dispatcher.eventdispatcher.impl.EventDispatcher>().ToSingleton()
					.ToName(global::strange.extensions.context.api.ContextKeys.CROSS_CONTEXT_DISPATCHER)
					.CrossContext();
				injectionBinder.Bind<global::strange.extensions.context.impl.CrossContextBridge>().ToSingleton().CrossContext();
			}
		}

		protected override void instantiateCoreComponents()
		{
			base.instantiateCoreComponents();
			global::strange.extensions.injector.api.IInjectionBinding binding = injectionBinder.GetBinding<global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher>(global::strange.extensions.context.api.ContextKeys.CONTEXT_DISPATCHER);
			if (binding != null)
			{
				global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher instance = injectionBinder.GetInstance<global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher>(global::strange.extensions.context.api.ContextKeys.CONTEXT_DISPATCHER);
				if (instance != null)
				{
					crossContextDispatcher = injectionBinder.GetInstance<global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher>(global::strange.extensions.context.api.ContextKeys.CROSS_CONTEXT_DISPATCHER);
					(crossContextDispatcher as global::strange.extensions.dispatcher.api.ITriggerProvider).AddTriggerable(instance as global::strange.extensions.dispatcher.api.ITriggerable);
					(instance as global::strange.extensions.dispatcher.api.ITriggerProvider).AddTriggerable(crossContextBridge as global::strange.extensions.dispatcher.api.ITriggerable);
				}
			}
		}

		public override global::strange.extensions.context.api.IContext AddContext(global::strange.extensions.context.api.IContext context)
		{
			base.AddContext(context);
			if (context is global::strange.extensions.context.api.ICrossContextCapable)
			{
				AssignCrossContext((global::strange.extensions.context.api.ICrossContextCapable)context);
			}
			return this;
		}

		public virtual void AssignCrossContext(global::strange.extensions.context.api.ICrossContextCapable childContext)
		{
			childContext.crossContextDispatcher = crossContextDispatcher;
			childContext.injectionBinder.CrossContextBinder = injectionBinder.CrossContextBinder;
		}

		public virtual void RemoveCrossContext(global::strange.extensions.context.api.ICrossContextCapable childContext)
		{
			if (childContext.crossContextDispatcher != null)
			{
				(childContext.crossContextDispatcher as global::strange.extensions.dispatcher.api.ITriggerProvider).RemoveTriggerable(childContext.GetComponent<global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher>(global::strange.extensions.context.api.ContextKeys.CONTEXT_DISPATCHER) as global::strange.extensions.dispatcher.api.ITriggerable);
				childContext.crossContextDispatcher = null;
			}
		}

		public override global::strange.extensions.context.api.IContext RemoveContext(global::strange.extensions.context.api.IContext context)
		{
			if (context is global::strange.extensions.context.api.ICrossContextCapable)
			{
				RemoveCrossContext((global::strange.extensions.context.api.ICrossContextCapable)context);
			}
			return base.RemoveContext(context);
		}
	}
}
