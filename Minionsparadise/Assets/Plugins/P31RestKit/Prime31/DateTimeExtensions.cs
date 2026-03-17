namespace Prime31
{
	public static class DateTimeExtensions
	{
		public static long toEpochTime(this global::System.DateTime self)
		{
			global::System.DateTime dateTime = new global::System.DateTime(1970, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc);
			return global::System.Convert.ToInt64((self - dateTime).TotalSeconds);
		}

		public static global::System.DateTime fromEpochTime(this long unixTime)
		{
			return new global::System.DateTime(1970, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc).AddSeconds(unixTime);
		}
	}
}
