namespace Kampai.Util
{
	public class AsyncRoutineResult
	{
		public bool IsDone { get; protected set; }

		public AsyncRoutineResult(bool isDone = false)
		{
			IsDone = isDone;
		}
	}
}
