namespace Discord.Unity
{
	internal class GroupCreateResult : global::Discord.Unity.ResultBase, global::Discord.Unity.IGroupCreateResult, global::Discord.Unity.IResult
	{
		public const string IDKey = "id";

		public string GroupId { get; private set; }

		public GroupCreateResult(global::Discord.Unity.ResultContainer resultContainer)
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
			return global::Discord.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string> { { "GroupId", GroupId } });
		}
	}
}
