namespace Discord.Unity
{
	internal class AccessTokenRefreshResult : global::Discord.Unity.ResultBase, global::Discord.Unity.IAccessTokenRefreshResult, global::Discord.Unity.IResult
	{
		public global::Discord.Unity.AccessToken AccessToken { get; private set; }

		public AccessTokenRefreshResult(global::Discord.Unity.ResultContainer resultContainer)
			: base(resultContainer)
		{
			if (ResultDictionary != null && ResultDictionary.ContainsKey(global::Discord.Unity.LoginResult.AccessTokenKey))
			{
				AccessToken = global::Discord.Unity.Utilities.ParseAccessTokenFromResult(ResultDictionary);
			}
		}

		public override string ToString()
		{
			return global::Discord.Unity.Utilities.FormatToString(base.ToString(), GetType().Name, new global::System.Collections.Generic.Dictionary<string, string> { 
			{
				"AccessToken",
				AccessToken.ToStringNullOk()
			} });
		}
	}
}
