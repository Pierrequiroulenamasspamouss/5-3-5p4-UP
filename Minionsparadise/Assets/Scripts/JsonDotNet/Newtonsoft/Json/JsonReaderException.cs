namespace Newtonsoft.Json
{
	public class JsonReaderException : global::System.Exception
	{
		public int LineNumber { get; private set; }

		public int LinePosition { get; private set; }

		public JsonReaderException()
		{
		}

		public JsonReaderException(string message)
			: base(message)
		{
		}

		public JsonReaderException(string message, global::System.Exception innerException)
			: base(message, innerException)
		{
		}

		internal JsonReaderException(string message, global::System.Exception innerException, int lineNumber, int linePosition)
			: base(message, innerException)
		{
			LineNumber = lineNumber;
			LinePosition = linePosition;
		}
	}
}
