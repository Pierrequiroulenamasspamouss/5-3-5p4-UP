namespace Kampai.Game.Mtx
{
	public class ServerValidationError
	{
		public enum Code
		{
			RECEIPT_DUPLICATE = 11,
			RECEIPT_INVALID = 12,
			VALIDATION_UNAVAILABLE = 13
		}

		public global::Kampai.Game.Mtx.ServerValidationError.Code code;

		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
		public string description;

		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
		public string message;

		[global::Newtonsoft.Json.JsonProperty(NullValueHandling = global::Newtonsoft.Json.NullValueHandling.Ignore)]
		public string exceptionDetails;
	}
}
