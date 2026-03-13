namespace Kampai.Util
{
	public class InvokerService : global::Kampai.Util.IInvokerService
	{
		private global::System.Collections.Generic.Queue<global::System.Action> work = new global::System.Collections.Generic.Queue<global::System.Action>();

		private global::System.Threading.Mutex mutex = new global::System.Threading.Mutex(false);

		public void Add(global::System.Action a)
		{
			try
			{
				mutex.WaitOne();
				work.Enqueue(a);
			}
			finally
			{
				mutex.ReleaseMutex();
			}
		}

		public void Update()
		{
			if (work.Count <= 0)
			{
				return;
			}
			try
			{
				mutex.WaitOne();
				while (work.Count > 0)
				{
					global::System.Action action = work.Dequeue();
					try
					{
						action();
					}
					catch (global::System.Exception ex)
					{
						global::UnityEngine.Debug.LogErrorFormat("InvokerService: Exception during action execution: {0}\n{1}", ex.Message, ex.StackTrace);
					}
				}
			}
			finally
			{
				mutex.ReleaseMutex();
			}
		}
	}
}
