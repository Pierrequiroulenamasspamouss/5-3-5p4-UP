namespace Kampai.Game
{
	public class ErrorResponseContent
	{
		[global::Newtonsoft.Json.JsonProperty("code")]
		public int Code { get; set; }

		[global::Newtonsoft.Json.JsonProperty("responseCode")]
		public int ResponseCode { get; set; }

		[global::Newtonsoft.Json.JsonProperty("description")]
		public string Description { get; set; }

		[global::Newtonsoft.Json.JsonProperty("message")]
		public string Message { get; set; }

		[global::Newtonsoft.Json.JsonProperty("exceptionDetails")]
		public string ExceptionDetails { get; set; }
	}
}
