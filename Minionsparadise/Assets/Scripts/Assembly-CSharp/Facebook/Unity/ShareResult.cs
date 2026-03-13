namespace Facebook.Unity
{
	internal class ShareResult : global::Facebook.Unity.ResultBase, global::Facebook.Unity.IResult, global::Facebook.Unity.IShareResult
	{
		public string PostId { get; private set; }

		internal static string PostIDKey
		{
			get
			{
				return (!global::Facebook.Unity.Constants.IsWeb) ? "id" : "post_id";
			}
		}

		internal ShareResult(global::Facebook.Unity.ResultContainer resultContainer)
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
			return global::Facebook.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string> { { "PostId", PostId } });
		}
	}
}
