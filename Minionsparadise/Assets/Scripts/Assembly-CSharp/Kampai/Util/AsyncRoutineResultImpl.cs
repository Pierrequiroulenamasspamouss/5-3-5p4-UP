namespace Kampai.Util
{
	internal sealed class AsyncRoutineResultImpl : global::Kampai.Util.AsyncRoutineResult
	{
		public static global::Kampai.Util.AsyncRoutineResultImpl Start(global::Kampai.Util.IInvokerService invoker, global::System.Action action, global::System.Action onComplete)
		{
			global::Kampai.Util.AsyncRoutineResultImpl result = new global::Kampai.Util.AsyncRoutineResultImpl();
			global::System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				action();
				result.IsDone = true;
				if (onComplete != null)
				{
					invoker.Add(onComplete);
				}
			});
			return result;
		}

		public static global::Kampai.Util.AsyncRoutineResultImpl Start(global::Kampai.Util.IInvokerService invoker, global::Kampai.Util.ContidionTask task, global::System.Action onComplete)
		{
			global::Kampai.Util.AsyncRoutineResultImpl result = new global::Kampai.Util.AsyncRoutineResultImpl();
			global::System.Threading.ThreadPool.QueueUserWorkItem(delegate
			{
				bool flag = task();
				result.IsDone = true;
				if (flag && onComplete != null)
				{
					invoker.Add(onComplete);
				}
			});
			return result;
		}
	}
}
