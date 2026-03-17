namespace strange.extensions.context.impl
{
	public class Context : global::strange.framework.impl.Binder, global::strange.extensions.context.api.IContext, global::strange.framework.api.IBinder
	{
		public static global::strange.extensions.context.api.IContext firstContext;

		public static global::System.Collections.Generic.Dictionary<object, global::strange.extensions.context.api.IContext> knownContexts = new global::System.Collections.Generic.Dictionary<object, global::strange.extensions.context.api.IContext>();

		public bool autoStartup;

		public object contextView { get; set; }

		public Context()
		{
		}

		public Context(object view, global::strange.extensions.context.api.ContextStartupFlags flags)
		{
			if (firstContext == null || firstContext.GetContextView() == null)
			{
				firstContext = this;
			}
			else
			{
				firstContext.AddContext(this);
			}
			SetContextView(view);
			addCoreComponents();
			autoStartup = (flags & global::strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH) != global::strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH;
			if ((flags & global::strange.extensions.context.api.ContextStartupFlags.MANUAL_MAPPING) != global::strange.extensions.context.api.ContextStartupFlags.MANUAL_MAPPING)
			{
				Start();
			}
		}

		public Context(object view)
			: this(view, global::strange.extensions.context.api.ContextStartupFlags.AUTOMATIC)
		{
		}

		public Context(object view, bool autoMapping)
			: this(view, autoMapping ? global::strange.extensions.context.api.ContextStartupFlags.MANUAL_MAPPING : (global::strange.extensions.context.api.ContextStartupFlags.MANUAL_MAPPING | global::strange.extensions.context.api.ContextStartupFlags.MANUAL_LAUNCH))
		{
		}

		protected virtual void addCoreComponents()
		{
		}

		protected virtual void instantiateCoreComponents()
		{
		}

		public virtual global::strange.extensions.context.api.IContext SetContextView(object view)
		{
			if (contextView != null && knownContexts.ContainsKey(contextView))
			{
				knownContexts.Remove(contextView);
			}
			contextView = view;
			if (view != null)
			{
				knownContexts.Add(view, this);
			}
			return this;
		}

		public virtual object GetContextView()
		{
			return contextView;
		}

		public virtual global::strange.extensions.context.api.IContext Start()
		{
			instantiateCoreComponents();
			mapBindings();
			postBindings();
			if (autoStartup)
			{
				Launch();
			}
			return this;
		}

		public virtual void Launch()
		{
		}

		protected virtual void mapBindings()
		{
		}

		protected virtual void postBindings()
		{
		}

		public virtual global::strange.extensions.context.api.IContext AddContext(global::strange.extensions.context.api.IContext context)
		{
			return this;
		}

		public virtual global::strange.extensions.context.api.IContext RemoveContext(global::strange.extensions.context.api.IContext context)
		{
			if (context == firstContext)
			{
				firstContext = null;
			}
			context.OnRemove();
			return this;
		}

		public virtual object GetComponent<T>()
		{
			return null;
		}

		public virtual object GetComponent<T>(object name)
		{
			return null;
		}

		public virtual void AddView(object view)
		{
		}

		public virtual void RemoveView(object view)
		{
		}
	}
}
