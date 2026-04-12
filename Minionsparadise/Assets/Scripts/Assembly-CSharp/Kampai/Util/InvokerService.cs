namespace Kampai.Util
{
	public class InvokerService : global::Kampai.Util.IInvokerService
	{
		private static global::System.Collections.Generic.Queue<global::System.Action> work = new global::System.Collections.Generic.Queue<global::System.Action>();
		private static readonly object _lock = new object();

		public void Add(global::System.Action a)
		{
			lock (_lock)
			{
				work.Enqueue(a);
			}
			//global::UnityEngine.Debug.LogFormat("InvokerService: Action enqueued. Current count: {0}", work.Count);
		}

		public void Update()
		{
			if (work.Count <= 0)
			{
				return;
			}

			while (true)
			{
				global::System.Action action = null;
				lock (_lock)
				{
					if (work.Count > 0)
					{
						action = work.Dequeue();
					}
				}

				if (action == null)
				{
					break;
				}

				// global::UnityEngine.Debug.LogFormat("InvokerService: Executing action. Remaining in queue: {0}", work.Count);
				try
				{
					action();
				}
				catch (global::System.Exception ex)
				{
					global::UnityEngine.Debug.LogErrorFormat("InvokerService: Exception during action execution: {0}\n{1}", ex.Message, ex.StackTrace);
				}
				global::UnityEngine.Debug.LogFormat("InvokerService: Finished executing action.");
			}
		}
	}
}
