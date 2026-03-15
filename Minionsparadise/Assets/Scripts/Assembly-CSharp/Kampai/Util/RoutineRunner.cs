namespace Kampai.Util
{
	public class RoutineRunner : global::Kampai.Util.IRoutineRunner
	{
		private global::Kampai.Util.RoutineRunnerBehaviour mb;

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		[Inject]
		public global::Kampai.Util.IInvokerService invoker { get; set; }

		[PostConstruct]
		public void PostConstruct()
		{
			mb = contextView.GetComponent<global::Kampai.Util.RoutineRunnerBehaviour>();
			if (mb == null)
			{
				mb = contextView.AddComponent<global::Kampai.Util.RoutineRunnerBehaviour>();
			}
		}

		private bool IsBehaviourAlive()
		{
			return mb != null;
		}

		public global::UnityEngine.Coroutine StartCoroutine(global::System.Collections.IEnumerator method)
		{
			return (!IsBehaviourAlive()) ? null : mb.StartCoroutine(method);
		}

		public void StopCoroutine(global::System.Collections.IEnumerator method)
		{
			if (IsBehaviourAlive())
			{
				mb.StopCoroutine(method);
			}
		}

		public void StartTimer(string timerID, float time, global::System.Action onComplete)
		{
			if (IsBehaviourAlive())
			{
				mb.StartTimer(timerID, time, onComplete);
			}
		}

		public void StopTimer(string timerID)
		{
			if (IsBehaviourAlive())
			{
				mb.StopTimer(timerID);
			}
		}

		public global::Kampai.Util.AsyncRoutineResult StartAsyncTask(global::System.Action task, global::System.Action onComplete)
		{
			return global::Kampai.Util.AsyncRoutineResultImpl.Start(invoker, task, onComplete);
		}

		public global::Kampai.Util.AsyncRoutineResult StartAsyncConditionTask(global::Kampai.Util.ContidionTask task, global::System.Action onComplete = null)
		{
			return global::Kampai.Util.AsyncRoutineResultImpl.Start(invoker, task, onComplete);
		}
	}
}
