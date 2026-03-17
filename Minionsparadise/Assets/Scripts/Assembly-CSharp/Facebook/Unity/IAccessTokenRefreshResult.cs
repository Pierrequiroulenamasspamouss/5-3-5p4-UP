namespace Facebook.Unity
{
	public interface IAccessTokenRefreshResult : global::Facebook.Unity.IResult
	{
		global::Facebook.Unity.AccessToken AccessToken { get; }
	}
}
