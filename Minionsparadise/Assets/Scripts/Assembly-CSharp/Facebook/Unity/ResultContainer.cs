namespace Discord.Unity
{
	internal class ResultContainer
	{
		private const string CanvasResponseKey = "response";

		public string RawResult { get; private set; }

		public global::System.Collections.Generic.IDictionary<string, object> ResultDictionary { get; set; }

		public ResultContainer(global::System.Collections.Generic.IDictionary<string, object> dictionary)
		{
			RawResult = dictionary.ToJson();
			ResultDictionary = dictionary;
			if (global::Discord.Unity.Constants.IsWeb)
			{
				ResultDictionary = GetWebFormattedResponseDictionary(ResultDictionary);
			}
		}

		public ResultContainer(string result)
		{
			RawResult = result;
			if (string.IsNullOrEmpty(result))
			{
				ResultDictionary = new global::System.Collections.Generic.Dictionary<string, object>();
				return;
			}
			ResultDictionary = global::Discord.MiniJSON.Json.Deserialize(result) as global::System.Collections.Generic.Dictionary<string, object>;
			if (global::Discord.Unity.Constants.IsWeb && ResultDictionary != null)
			{
				ResultDictionary = GetWebFormattedResponseDictionary(ResultDictionary);
			}
		}

		private global::System.Collections.Generic.IDictionary<string, object> GetWebFormattedResponseDictionary(global::System.Collections.Generic.IDictionary<string, object> resultDictionary)
		{
			global::System.Collections.Generic.IDictionary<string, object> value;
			if (resultDictionary.TryGetValue<global::System.Collections.Generic.IDictionary<string, object>>("response", out value))
			{
				object value2;
				if (resultDictionary.TryGetValue("callback_id", out value2))
				{
					value["callback_id"] = value2;
				}
				return value;
			}
			return resultDictionary;
		}
	}
}
