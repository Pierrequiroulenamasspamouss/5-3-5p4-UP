namespace Facebook.Unity
{
	public interface ILoginResult : global::Facebook.Unity.IResult
	{
		global::Facebook.Unity.AccessToken AccessToken { get; }
	}
}
