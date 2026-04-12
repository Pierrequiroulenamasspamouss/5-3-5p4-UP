namespace Kampai.Util
{
	public interface ICoroutineProgressMonitor
	{
		object waitForPreviousTaskToComplete { get; }

		object waitForNextFrame { get; }

		bool HasRunningTasks();

		int GetRunningTasksCount();

		void StartTask(global::System.Collections.IEnumerator enumerator, string tag);
	}
}
