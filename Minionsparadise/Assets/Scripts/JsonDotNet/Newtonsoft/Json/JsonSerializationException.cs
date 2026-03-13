namespace Newtonsoft.Json
{
	public class JsonSerializationException : global::System.Exception
	{
		public JsonSerializationException()
		{
		}

		public JsonSerializationException(string message)
			: base(message)
		{
		}

		public JsonSerializationException(string message, global::System.Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
