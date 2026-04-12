namespace Discord.Unity
{
	internal class AppRequestResult : global::Discord.Unity.ResultBase, global::Discord.Unity.IAppRequestResult, global::Discord.Unity.IResult
	{
		public const string RequestIDKey = "request";

		public const string ToKey = "to";

		public string RequestID { get; private set; }

		public global::System.Collections.Generic.IEnumerable<string> To { get; private set; }

		public AppRequestResult(global::Discord.Unity.ResultContainer resultContainer)
			: base(resultContainer)
		{
			if (ResultDictionary == null)
			{
				return;
			}
			string value;
			if (ResultDictionary.TryGetValue<string>("request", out value))
			{
				RequestID = value;
			}
			string value2;
			if (ResultDictionary.TryGetValue<string>("to", out value2))
			{
				To = value2.Split(',');
			}
			else
			{
				global::System.Collections.Generic.IEnumerable<object> value3;
				if (!ResultDictionary.TryGetValue<global::System.Collections.Generic.IEnumerable<object>>("to", out value3))
				{
					return;
				}
				global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
				foreach (object item in value3)
				{
					string text = item as string;
					if (text != null)
					{
						list.Add(text);
					}
				}
				To = list;
			}
		}

		public override string ToString()
		{
			return global::Discord.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string>
			{
				{ "RequestID", RequestID },
				{
					"To",
					(To == null) ? null : To.ToCommaSeparateList()
				}
			});
		}
	}
}
