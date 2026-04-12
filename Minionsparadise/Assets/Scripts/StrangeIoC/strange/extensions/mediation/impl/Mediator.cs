namespace strange.extensions.mediation.impl
{
	public class Mediator : global::UnityEngine.MonoBehaviour, global::strange.extensions.mediation.api.IMediator
	{
		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		public virtual void PreRegister()
		{
		}

		public virtual void OnRegister()
		{
		}

		public virtual void OnRemove()
		{
		}
	}
}
