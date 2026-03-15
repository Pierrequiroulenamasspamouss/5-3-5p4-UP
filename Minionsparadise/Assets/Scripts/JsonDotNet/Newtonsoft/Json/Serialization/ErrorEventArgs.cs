namespace Newtonsoft.Json.Serialization
{
	public class ErrorEventArgs : global::System.EventArgs
	{
		public object CurrentObject { get; private set; }

		public global::Newtonsoft.Json.Serialization.ErrorContext ErrorContext { get; private set; }

		public ErrorEventArgs(object currentObject, global::Newtonsoft.Json.Serialization.ErrorContext errorContext)
		{
			CurrentObject = currentObject;
			ErrorContext = errorContext;
		}
	}
}
