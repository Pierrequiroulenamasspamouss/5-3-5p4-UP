public static class TimeUtil
{
	private static readonly global::System.DateTime EPOCH = new global::System.DateTime(1970, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc);

	public static long CurrentTimeMillis()
	{
		return (long)(global::System.DateTime.UtcNow - EPOCH).TotalMilliseconds;
	}
}
