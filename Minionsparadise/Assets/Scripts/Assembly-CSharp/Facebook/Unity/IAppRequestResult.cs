namespace Discord.Unity
{
	public interface IAppRequestResult : global::Discord.Unity.IResult
	{
		string RequestID { get; }

		global::System.Collections.Generic.IEnumerable<string> To { get; }
	}
}
