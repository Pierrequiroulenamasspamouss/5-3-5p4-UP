namespace Swrve.REST
{
	public class RESTResponse
	{
		public readonly string Body;

		public readonly global::Swrve.Helpers.WwwDeducedError Error;

		public readonly global::System.Collections.Generic.Dictionary<string, string> Headers;

		public RESTResponse(string body)
		{
			Body = body;
		}

		public RESTResponse(string body, global::System.Collections.Generic.Dictionary<string, string> headers)
			: this(body)
		{
			Headers = headers;
		}

		public RESTResponse(global::Swrve.Helpers.WwwDeducedError error)
		{
			Error = error;
		}
	}
}
