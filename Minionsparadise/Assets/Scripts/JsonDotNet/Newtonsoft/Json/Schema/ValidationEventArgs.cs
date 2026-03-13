namespace Newtonsoft.Json.Schema
{
	public class ValidationEventArgs : global::System.EventArgs
	{
		private readonly global::Newtonsoft.Json.Schema.JsonSchemaException _ex;

		public global::Newtonsoft.Json.Schema.JsonSchemaException Exception
		{
			get
			{
				return _ex;
			}
		}

		public string Message
		{
			get
			{
				return _ex.Message;
			}
		}

		internal ValidationEventArgs(global::Newtonsoft.Json.Schema.JsonSchemaException ex)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(ex, "ex");
			_ex = ex;
		}
	}
}
