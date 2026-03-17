namespace Kampai.Util
{
	public class KampaiView : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
	{
		private static global::System.Collections.Generic.Dictionary<int, global::strange.extensions.context.api.IContext> s_contextCache = new global::System.Collections.Generic.Dictionary<int, global::strange.extensions.context.api.IContext>();

		protected global::strange.extensions.context.api.IContext currentContext;

		private bool m_requiresContext = true;

		protected bool registerWithContext = true;

		public bool requiresContext
		{
			get
			{
				return m_requiresContext;
			}
			set
			{
				m_requiresContext = value;
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

		public static void BubbleToContextOnAwake<T>(T view, ref global::strange.extensions.context.api.IContext currentContext, bool addIfFound = false) where T : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
		{
			if (!(view == null) && view.autoRegisterWithContext && !view.registeredWithContext)
			{
				BubbleToContext(view, true, addIfFound, ref currentContext);
			}
		}

		public static void BubbleToContextOnStart<T>(T view, ref global::strange.extensions.context.api.IContext currentContext) where T : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
		{
			if (!(view == null) && view.autoRegisterWithContext && !view.registeredWithContext)
			{
				BubbleToContext(view, true, true, ref currentContext);
			}
		}

		public static void BubbleToContextOnDestroy<T>(T view, ref global::strange.extensions.context.api.IContext currentContext) where T : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
		{
			BubbleToContext(view, false, false, ref currentContext);
		}

		public static void ClearContextCache()
		{
			s_contextCache.Clear();
			s_contextCache = new global::System.Collections.Generic.Dictionary<int, global::strange.extensions.context.api.IContext>();
		}

		protected virtual void Awake()
		{
			BubbleToContextOnAwake(this, ref currentContext);
		}

		protected virtual void Start()
		{
			BubbleToContextOnStart(this, ref currentContext);
		}

		protected virtual void OnDestroy()
		{
			BubbleToContextOnDestroy(this, ref currentContext);
		}

		public static void BubbleToContext<T>(T view, bool toAdd, bool finalTry, ref global::strange.extensions.context.api.IContext currentContext) where T : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
		{
			if (currentContext != null)
			{
				AttachViewToContext(view, currentContext, toAdd, ref currentContext);
			}
			else
			{
				FindViewContext(view, toAdd, finalTry, ref currentContext);
			}
		}

		protected static void AttachViewToContext<T>(T view, global::strange.extensions.context.api.IContext context, bool toAdd, ref global::strange.extensions.context.api.IContext currentContext) where T : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
		{
			if (toAdd)
			{
				context.AddView(view);
				currentContext = context;
				view.registeredWithContext = true;
			}
			else
			{
				context.RemoveView(view);
			}
		}

		protected static int BubbleUpToContext<T>(T view, bool toAdd, global::UnityEngine.GameObject viewGameObject, ref global::strange.extensions.context.api.IContext currentContext) where T : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
		{
			int result = 0;
			global::UnityEngine.Transform parent = viewGameObject.transform;
			while (parent.parent != null && result++ < 100)
			{
				parent = parent.parent;
				global::UnityEngine.GameObject gameObject = parent.gameObject;
				global::strange.extensions.context.api.IContext value;
				if (!global::strange.extensions.context.impl.Context.knownContexts.TryGetValue(gameObject, out value) && !s_contextCache.TryGetValue(gameObject.layer, out value))
				{
					continue;
				}
				AttachViewToContext(view, value, toAdd, ref currentContext);
				return -1;
			}
			return result;
		}

		protected static void FindViewContext<T>(T view, bool toAdd, bool finalTry, ref global::strange.extensions.context.api.IContext currentContext) where T : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
		{
			global::UnityEngine.GameObject gameObject = view.gameObject;
			int layer = gameObject.layer;
			if (s_contextCache.ContainsKey(layer))
			{
				currentContext = s_contextCache[layer];
				if (!toAdd || finalTry)
				{
					AttachViewToContext(view, currentContext, toAdd, ref currentContext);
				}
			}
			else if (!toAdd || finalTry)
			{
				int num = BubbleUpToContext(view, toAdd, gameObject, ref currentContext);
				if (layer != 0 && currentContext != null && !s_contextCache.ContainsKey(layer))
				{
					s_contextCache.Add(layer, currentContext);
				}
				if (view.requiresContext && finalTry && num != -1)
				{
					FinalTryToGetContext(view, num, ref currentContext);
				}
			}
		}

		protected static void FinalTryToGetContext<T>(T view, int numOfTries, ref global::strange.extensions.context.api.IContext currentContext) where T : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IView
		{
			if (global::strange.extensions.context.impl.Context.firstContext != null)
			{
				AttachViewToContext(view, global::strange.extensions.context.impl.Context.firstContext, true, ref currentContext);
				return;
			}
			string arg = ((numOfTries == 100) ? "A view couldn't find a context. Loop limit reached." : "A view was added with no context. Views must be added into the hierarchy of their ContextView lest all hell break loose.");
			throw new global::strange.extensions.mediation.impl.MediationException(string.Format("{0}\nView: {1}", arg, view), global::strange.extensions.mediation.api.MediationExceptionType.NO_CONTEXT);
		}
	}
}
