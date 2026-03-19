namespace Discord.Unity
{
	public interface IAppLinkResult : global::Discord.Unity.IResult
	{
		string Url { get; }

		string TargetUrl { get; }

		string Ref { get; }

		global::System.Collections.Generic.IDictionary<string, object> Extras { get; }
	}
}
