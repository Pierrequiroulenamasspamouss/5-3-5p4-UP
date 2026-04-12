namespace Discord.Unity
{
	internal class ShareResult : global::Discord.Unity.ResultBase, global::Discord.Unity.IResult, global::Discord.Unity.IShareResult
	{
		public string PostId { get; private set; }

		internal static string PostIDKey
		{
			get
			{
				return (!global::Discord.Unity.Constants.IsWeb) ? "id" : "post_id";
			}
		}

		internal ShareResult(global::Discord.Unity.ResultContainer resultContainer)
			: base(resultContainer)
		{
			if (ResultDictionary != null)
			{
				string value;
				if (ResultDictionary.TryGetValue<string>(PostIDKey, out value))
				{
					PostId = value;
				}
				else if (ResultDictionary.TryGetValue<string>("postId", out value))
				{
					PostId = value;
				}
			}
		}

		public override string ToString()
		{
			return global::Discord.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string> { { "PostId", PostId } });
		}
	}
}
