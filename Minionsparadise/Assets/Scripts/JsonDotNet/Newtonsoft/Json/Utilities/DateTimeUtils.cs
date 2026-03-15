namespace Newtonsoft.Json.Utilities
{
	internal static class DateTimeUtils
	{
		public static string GetLocalOffset(this global::System.DateTime d)
		{
			global::System.TimeSpan utcOffset = global::System.TimeZone.CurrentTimeZone.GetUtcOffset(d);
			return utcOffset.Hours.ToString("+00;-00", global::System.Globalization.CultureInfo.InvariantCulture) + ":" + utcOffset.Minutes.ToString("00;00", global::System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
