namespace Newtonsoft.Json.Converters
{
	public abstract class DateTimeConverterBase : global::Newtonsoft.Json.JsonConverter
	{
		public override bool CanConvert(global::System.Type objectType)
		{
			if (objectType == typeof(global::System.DateTime) || objectType == typeof(global::System.DateTime?))
			{
				return true;
			}
			if (objectType == typeof(global::System.DateTimeOffset) || objectType == typeof(global::System.DateTimeOffset?))
			{
				return true;
			}
			return false;
		}
	}
}
