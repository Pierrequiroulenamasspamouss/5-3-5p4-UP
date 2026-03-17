namespace strange.extensions.context.impl
{
	public class MVCSContext : global::strange.extensions.context.impl.CrossContext
	{
		protected static global::strange.framework.api.ISemiBinding viewCache = new global::strange.framework.impl.SemiBinding();

		public global::strange.extensions.command.api.ICommandBinder commandBinder { get; set; }

		public global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher dispatcher { get; set; }

		public global::strange.extensions.mediation.api.IMediationBinder mediationBinder { get; set; }

		public global::strange.extensions.implicitBind.api.IImplicitBinder implicitBinder { get; set; }

		public global::strange.extensions.sequencer.api.ISequencer sequencer { get; set; }

		public MVCSContext()
		{
		}

		public MVCSContext(global::UnityEngine.MonoBehaviour view)
			: base(view)
		{
		}

		public MVCSContext(global::UnityEngine.MonoBehaviour view, global::strange.extensions.context.api.ContextStartupFlags flags)
			: base(view, flags)
		{
		}

		public MVCSContext(global::UnityEngine.MonoBehaviour view, bool autoMapping)
			: base(view, autoMapping)
		{
		}

		public override global::strange.extensions.context.api.IContext SetContextView(object view)
		{
			if (base.contextView != null && global::strange.extensions.context.impl.Context.knownContexts.ContainsKey(base.contextView))
			{
				global::strange.extensions.context.impl.Context.knownContexts.Remove(base.contextView);
			}
			base.contextView = (view as global::UnityEngine.MonoBehaviour).gameObject;
			if (base.contextView == null)
			{
				throw new global::strange.extensions.context.impl.ContextException("MVCSContext requires a ContextView of type MonoBehaviour", global::strange.extensions.context.api.ContextExceptionType.NO_CONTEXT_VIEW);
			}
			global::strange.extensions.context.impl.Context.knownContexts.Add(base.contextView, this);
			return this;
		}

		protected override void addCoreComponents()
		{
			base.addCoreComponents();
			base.injectionBinder.Bind<global::strange.framework.api.IInstanceProvider>().Bind<global::strange.extensions.injector.api.IInjectionBinder>().ToValue(base.injectionBinder);
			base.injectionBinder.Bind<global::strange.extensions.context.api.IContext>().ToValue(this).ToName(global::strange.extensions.context.api.ContextKeys.CONTEXT);
			base.injectionBinder.Bind<global::strange.extensions.command.api.ICommandBinder>().To<global::strange.extensions.command.impl.EventCommandBinder>().ToSingleton();
			base.injectionBinder.Bind<global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher>().To<global::strange.extensions.dispatcher.eventdispatcher.impl.EventDispatcher>();
			base.injectionBinder.Bind<global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher>().To<global::strange.extensions.dispatcher.eventdispatcher.impl.EventDispatcher>().ToSingleton()
				.ToName(global::strange.extensions.context.api.ContextKeys.CONTEXT_DISPATCHER);
			base.injectionBinder.Bind<global::strange.extensions.mediation.api.IMediationBinder>().To<global::strange.extensions.mediation.impl.MediationBinder>().ToSingleton();
			base.injectionBinder.Bind<global::strange.extensions.sequencer.api.ISequencer>().To<global::strange.extensions.sequencer.impl.EventSequencer>().ToSingleton();
			base.injectionBinder.Bind<global::strange.extensions.implicitBind.api.IImplicitBinder>().To<global::strange.extensions.implicitBind.impl.ImplicitBinder>().ToSingleton();
		}

		protected override void instantiateCoreComponents()
		{
			base.instantiateCoreComponents();
			if (base.contextView == null)
			{
				throw new global::strange.extensions.context.impl.ContextException("MVCSContext requires a ContextView of type MonoBehaviour", global::strange.extensions.context.api.ContextExceptionType.NO_CONTEXT_VIEW);
			}
			base.injectionBinder.Bind<global::UnityEngine.GameObject>().ToValue(base.contextView).ToName(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW);
			commandBinder = base.injectionBinder.GetInstance<global::strange.extensions.command.api.ICommandBinder>();
			dispatcher = base.injectionBinder.GetInstance<global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher>(global::strange.extensions.context.api.ContextKeys.CONTEXT_DISPATCHER);
			mediationBinder = base.injectionBinder.GetInstance<global::strange.extensions.mediation.api.IMediationBinder>();
			sequencer = base.injectionBinder.GetInstance<global::strange.extensions.sequencer.api.ISequencer>();
			implicitBinder = base.injectionBinder.GetInstance<global::strange.extensions.implicitBind.api.IImplicitBinder>();
			(dispatcher as global::strange.extensions.dispatcher.api.ITriggerProvider).AddTriggerable(commandBinder as global::strange.extensions.dispatcher.api.ITriggerable);
			(dispatcher as global::strange.extensions.dispatcher.api.ITriggerProvider).AddTriggerable(sequencer as global::strange.extensions.dispatcher.api.ITriggerable);
		}

		protected override void postBindings()
		{
			mediateViewCache();
			mediationBinder.Trigger(global::strange.extensions.mediation.api.MediationEvent.AWAKE, (base.contextView as global::UnityEngine.GameObject).GetComponent<global::strange.extensions.context.impl.ContextView>());
		}

		public override void Launch()
		{
			dispatcher.Dispatch(global::strange.extensions.context.api.ContextEvent.START);
		}

		public override object GetComponent<T>()
		{
			return GetComponent<T>(null);
		}

		public override object GetComponent<T>(object name)
		{
			global::strange.extensions.injector.api.IInjectionBinding binding = base.injectionBinder.GetBinding<T>(name);
			if (binding != null)
			{
				return base.injectionBinder.GetInstance<T>(name);
			}
			return null;
		}

		public override void AddView(object view)
		{
			if (mediationBinder != null)
			{
				mediationBinder.Trigger(global::strange.extensions.mediation.api.MediationEvent.AWAKE, view as global::strange.extensions.mediation.api.IView);
			}
			else
			{
				cacheView(view as global::UnityEngine.MonoBehaviour);
			}
		}

		public override void RemoveView(object view)
		{
			mediationBinder.Trigger(global::strange.extensions.mediation.api.MediationEvent.DESTROYED, view as global::strange.extensions.mediation.api.IView);
		}

		protected virtual void cacheView(global::UnityEngine.MonoBehaviour view)
		{
			if (viewCache.constraint.Equals(global::strange.framework.api.BindingConstraintType.ONE))
			{
				viewCache.constraint = global::strange.framework.api.BindingConstraintType.MANY;
			}
			viewCache.Add(view);
		}

		protected virtual void mediateViewCache()
		{
			if (mediationBinder == null)
			{
				throw new global::strange.extensions.context.impl.ContextException("MVCSContext cannot mediate views without a mediationBinder", global::strange.extensions.context.api.ContextExceptionType.NO_MEDIATION_BINDER);
			}
			object[] array = viewCache.value as object[];
			if (array != null)
			{
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					mediationBinder.Trigger(global::strange.extensions.mediation.api.MediationEvent.AWAKE, array[i] as global::strange.extensions.mediation.api.IView);
				}
				viewCache = new global::strange.framework.impl.SemiBinding();
			}
		}

		public override void OnRemove()
		{
			base.OnRemove();
			commandBinder.OnRemove();
		}
	}
}
