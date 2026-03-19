namespace Discord.Unity
{
	internal class AppLinkResult : global::Discord.Unity.ResultBase, global::Discord.Unity.IAppLinkResult, global::Discord.Unity.IResult
	{
		public string Url { get; private set; }

		public string TargetUrl { get; private set; }

		public string Ref { get; private set; }

		public global::System.Collections.Generic.IDictionary<string, object> Extras { get; private set; }

		public AppLinkResult(global::Discord.Unity.ResultContainer resultContainer)
			: base(resultContainer)
		{
			if (ResultDictionary != null)
			{
				string value;
				if (ResultDictionary.TryGetValue<string>("url", out value))
				{
					Url = value;
				}
				string value2;
				if (ResultDictionary.TryGetValue<string>("target_url", out value2))
				{
					TargetUrl = value2;
				}
				string value3;
				if (ResultDictionary.TryGetValue<string>("ref", out value3))
				{
					Ref = value3;
				}
				global::System.Collections.Generic.IDictionary<string, object> value4;
				if (ResultDictionary.TryGetValue<global::System.Collections.Generic.IDictionary<string, object>>("extras", out value4))
				{
					Extras = value4;
				}
			}
		}

		public override string ToString()
		{
			return global::Discord.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string>
			{
				{ "Url", Url },
				{ "TargetUrl", TargetUrl },
				{ "Ref", Ref },
				{
					"Extras",
					Extras.ToJson()
				}
			});
		}
	}
}
