namespace Facebook.Unity
{
	public interface IAppRequestResult : global::Facebook.Unity.IResult
	{
		string RequestID { get; }

		global::System.Collections.Generic.IEnumerable<string> To { get; }
	}
}
