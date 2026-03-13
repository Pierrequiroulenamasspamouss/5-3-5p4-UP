namespace Kampai.Util
{
	public interface Taskable
	{
		int TaskDuration { get; set; }

		int UTCTaskStartTime { get; set; }
	}
}
