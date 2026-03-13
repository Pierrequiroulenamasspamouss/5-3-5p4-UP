namespace strange.extensions.context.impl
{
	public class ContextView : global::UnityEngine.MonoBehaviour, global::strange.extensions.context.api.IContextView, global::strange.extensions.mediation.api.IView
	{
		public global::strange.extensions.context.api.IContext context { get; set; }

		public bool requiresContext { get; set; }

		public bool registeredWithContext { get; set; }

		public bool autoRegisterWithContext { get; set; }

		protected virtual void OnDestroy()
		{
			if (context != null && global::strange.extensions.context.impl.Context.firstContext != null)
			{
				global::strange.extensions.context.impl.Context.firstContext.RemoveContext(context);
			}
			if (global::strange.extensions.context.impl.Context.knownContexts.ContainsKey(base.gameObject))
			{
				global::strange.extensions.context.impl.Context.knownContexts.Remove(base.gameObject);
			}
		}
	}
}
