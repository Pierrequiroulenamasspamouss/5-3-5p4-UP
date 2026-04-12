namespace Discord.Unity
{
	internal class LoginResult : global::Discord.Unity.ResultBase, global::Discord.Unity.ILoginResult, global::Discord.Unity.IResult
	{
		public const string LastRefreshKey = "last_refresh";

		public static readonly string UserIdKey = ((!global::Discord.Unity.Constants.IsWeb) ? "user_id" : "userID");

		public static readonly string ExpirationTimestampKey = ((!global::Discord.Unity.Constants.IsWeb) ? "expiration_timestamp" : "expiresIn");

		public static readonly string PermissionsKey = ((!global::Discord.Unity.Constants.IsWeb) ? "permissions" : "grantedScopes");

		public static readonly string AccessTokenKey = ((!global::Discord.Unity.Constants.IsWeb) ? "access_token" : "accessToken");

		public global::Discord.Unity.AccessToken AccessToken { get; private set; }

		internal LoginResult(global::Discord.Unity.ResultContainer resultContainer)
			: base(resultContainer)
		{
			if (ResultDictionary != null && ResultDictionary.ContainsKey(AccessTokenKey))
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
