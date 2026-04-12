namespace Discord.Unity
{
	public interface IAccessTokenRefreshResult : global::Discord.Unity.IResult
	{
		global::Discord.Unity.AccessToken AccessToken { get; }
	}
}
