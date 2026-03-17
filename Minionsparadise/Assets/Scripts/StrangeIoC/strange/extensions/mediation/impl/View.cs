namespace strange.extensions.mediation.impl
{
	public class View : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
	{
		private bool _requiresContext = true;

		protected bool registerWithContext = true;

		protected global::strange.extensions.context.api.IContext currentContext;

		public bool requiresContext
		{
			get
			{
				return _requiresContext;
			}
			set
			{
				_requiresContext = value;
			}
		}

		public virtual bool autoRegisterWithContext
		{
			get
			{
				return registerWithContext;
			}
			set
			{
				registerWithContext = value;
			}
		}

		public bool registeredWithContext { get; set; }

		protected virtual void Awake()
		{
			if (autoRegisterWithContext && !registeredWithContext)
			{
				bubbleToContext(this, true, false);
			}
		}

		protected virtual void Start()
		{
			if (autoRegisterWithContext && !registeredWithContext)
			{
				bubbleToContext(this, true, true);
			}
		}

		protected virtual void OnDestroy()
		{
			bubbleToContext(this, false, false);
		}

		protected virtual void bubbleToContext(global::UnityEngine.MonoBehaviour view, bool toAdd, bool finalTry)
		{
			int num = 0;
			global::UnityEngine.Transform parent = view.gameObject.transform;
			if (currentContext != null)
			{
				if (toAdd)
				{
					currentContext.AddView(view);
				}
				else
				{
					currentContext.RemoveView(view);
				}
				return;
			}
			while (parent.parent != null && num < 100)
			{
				num++;
				parent = parent.parent;
				global::UnityEngine.GameObject key = parent.gameObject;
				global::strange.extensions.context.api.IContext value;
				if (global::strange.extensions.context.impl.Context.knownContexts.TryGetValue(key, out value))
				{
					if (toAdd)
					{
						value.AddView(view);
						currentContext = value;
						registeredWithContext = true;
					}
					else
					{
						value.RemoveView(view);
					}
					return;
				}
			}
			if (requiresContext && finalTry)
			{
				if (global::strange.extensions.context.impl.Context.firstContext == null)
				{
					string text = ((num != 100) ? (text = "A view was added with no context. Views must be added into the hierarchy of their ContextView lest all hell break loose.") : (text = "A view couldn't find a context. Loop limit reached."));
					text = text + "\nView: " + view.ToString();
					throw new global::strange.extensions.mediation.impl.MediationException(text, global::strange.extensions.mediation.api.MediationExceptionType.NO_CONTEXT);
				}
				global::strange.extensions.context.impl.Context.firstContext.AddView(view);
				currentContext = global::strange.extensions.context.impl.Context.firstContext;
				registeredWithContext = true;
			}
		}
	}
}
