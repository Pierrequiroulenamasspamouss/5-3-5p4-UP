namespace Kampai.Util
{
	public class UpdateRunner : global::Kampai.Util.IUpdateRunner
	{
		private global::Kampai.Util.UpdateRunnerBehaviour ub;

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		[PostConstruct]
		public void PostConstruct()
		{
			ub = contextView.GetComponent<global::Kampai.Util.UpdateRunnerBehaviour>();
			if (ub == null)
			{
				ub = contextView.AddComponent<global::Kampai.Util.UpdateRunnerBehaviour>();
			}
		}

		public void Subscribe(global::System.Action action)
		{
			ub.Subscribe(action);
		}

		public void Unsubscribe(global::System.Action action)
		{
			ub.Unsubscribe(action);
		}
	}
}
