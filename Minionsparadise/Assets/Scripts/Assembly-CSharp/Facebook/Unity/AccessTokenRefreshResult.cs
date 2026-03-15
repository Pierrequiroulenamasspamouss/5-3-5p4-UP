namespace Facebook.Unity
{
	internal class AccessTokenRefreshResult : global::Facebook.Unity.ResultBase, global::Facebook.Unity.IAccessTokenRefreshResult, global::Facebook.Unity.IResult
	{
		public global::Facebook.Unity.AccessToken AccessToken { get; private set; }

		public AccessTokenRefreshResult(global::Facebook.Unity.ResultContainer resultContainer)
			: base(resultContainer)
		{
			if (ResultDictionary != null && ResultDictionary.ContainsKey(global::Facebook.Unity.LoginResult.AccessTokenKey))
			{
				AccessToken = global::Facebook.Unity.Utilities.ParseAccessTokenFromResult(ResultDictionary);
			}
		}

		public override string ToString()
		{
			return global::Facebook.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string> { 
			{
				"AccessToken",
				AccessToken.ToStringNullOk()
			} });
		}
	}
}
