namespace Facebook.Unity
{
	internal class GroupCreateResult : global::Facebook.Unity.ResultBase, global::Facebook.Unity.IGroupCreateResult, global::Facebook.Unity.IResult
	{
		public const string IDKey = "id";

		public string GroupId { get; private set; }

		public GroupCreateResult(global::Facebook.Unity.ResultContainer resultContainer)
			: base(resultContainer)
		{
			string value;
			if (ResultDictionary != null && ResultDictionary.TryGetValue<string>("id", out value))
			{
				GroupId = value;
			}
		}

		public override string ToString()
		{
			return global::Facebook.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string> { { "GroupId", GroupId } });
		}
	}
}
