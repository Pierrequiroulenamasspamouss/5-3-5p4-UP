namespace Facebook.Unity
{
	internal class LoginResult : global::Facebook.Unity.ResultBase, global::Facebook.Unity.ILoginResult, global::Facebook.Unity.IResult
	{
		public const string LastRefreshKey = "last_refresh";

		public static readonly string UserIdKey = ((!global::Facebook.Unity.Constants.IsWeb) ? "user_id" : "userID");

		public static readonly string ExpirationTimestampKey = ((!global::Facebook.Unity.Constants.IsWeb) ? "expiration_timestamp" : "expiresIn");

		public static readonly string PermissionsKey = ((!global::Facebook.Unity.Constants.IsWeb) ? "permissions" : "grantedScopes");

		public static readonly string AccessTokenKey = ((!global::Facebook.Unity.Constants.IsWeb) ? "access_token" : "accessToken");

		public global::Facebook.Unity.AccessToken AccessToken { get; private set; }

		internal LoginResult(global::Facebook.Unity.ResultContainer resultContainer)
			: base(resultContainer)
		{
			if (ResultDictionary != null && ResultDictionary.ContainsKey(AccessTokenKey))
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
