namespace Prime31
{
	public sealed class P31Error
	{
		private bool _containsOnlyMessage;

		public string message { get; private set; }

		public string domain { get; private set; }

		public int code { get; private set; }

		public global::System.Collections.Generic.Dictionary<string, object> userInfo { get; private set; }

		public static global::Prime31.P31Error errorFromJson(string json)
		{
			global::Prime31.P31Error p31Error = new global::Prime31.P31Error();
			if (!json.StartsWith("{"))
			{
				p31Error.message = json;
				p31Error._containsOnlyMessage = true;
				return p31Error;
			}
			global::System.Collections.Generic.Dictionary<string, object> dictionary = global::Prime31.Json.decode(json) as global::System.Collections.Generic.Dictionary<string, object>;
			if (dictionary == null)
			{
				p31Error.message = "Unknown error";
			}
			else
			{
				p31Error.message = ((!dictionary.ContainsKey("message")) ? null : dictionary["message"].ToString());
				p31Error.domain = ((!dictionary.ContainsKey("domain")) ? null : dictionary["domain"].ToString());
				p31Error.code = ((!dictionary.ContainsKey("code")) ? (-1) : int.Parse(dictionary["code"].ToString()));
				p31Error.userInfo = ((!dictionary.ContainsKey("userInfo")) ? null : (dictionary["userInfo"] as global::System.Collections.Generic.Dictionary<string, object>));
			}
			return p31Error;
		}

		public override string ToString()
		{
			if (_containsOnlyMessage)
			{
				return string.Format("[P31Error]: {0}", message);
			}
			try
			{
				string input = global::Prime31.Json.encode(this);
				return string.Format("[P31Error]: {0}", global::Prime31.JsonFormatter.prettyPrint(input));
			}
			catch (global::System.Exception)
			{
				return string.Format("[P31Error]: {0}", message);
			}
		}
	}
}
