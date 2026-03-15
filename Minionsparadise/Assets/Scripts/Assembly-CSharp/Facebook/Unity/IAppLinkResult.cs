namespace Facebook.Unity
{
	public interface IAppLinkResult : global::Facebook.Unity.IResult
	{
		string Url { get; }

		string TargetUrl { get; }

		string Ref { get; }

		global::System.Collections.Generic.IDictionary<string, object> Extras { get; }
	}
}
