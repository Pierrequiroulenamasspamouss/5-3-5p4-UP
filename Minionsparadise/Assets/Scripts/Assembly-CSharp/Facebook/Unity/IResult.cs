namespace Facebook.Unity
{
	public interface IResult
	{
		string Error { get; }

		global::System.Collections.Generic.IDictionary<string, object> ResultDictionary { get; }

		string RawResult { get; }

		bool Cancelled { get; }
	}
}
