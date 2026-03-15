namespace Facebook.Unity
{
	public class AccessToken
	{
		public static global::Facebook.Unity.AccessToken CurrentAccessToken { get; internal set; }

		public string TokenString { get; private set; }

		public global::System.DateTime ExpirationTime { get; private set; }

		public global::System.Collections.Generic.IEnumerable<string> Permissions { get; private set; }

		public string UserId { get; private set; }

		public global::System.DateTime? LastRefresh { get; private set; }

		internal AccessToken(string tokenString, string userId, global::System.DateTime expirationTime, global::System.Collections.Generic.IEnumerable<string> permissions, global::System.DateTime? lastRefresh)
		{
			if (string.IsNullOrEmpty(tokenString))
			{
				throw new global::System.ArgumentNullException("tokenString");
			}
			if (string.IsNullOrEmpty(userId))
			{
				throw new global::System.ArgumentNullException("userId");
			}
			if (expirationTime == global::System.DateTime.MinValue)
			{
				throw new global::System.ArgumentException("Expiration time is unassigned");
			}
			if (permissions == null)
			{
				throw new global::System.ArgumentNullException("permissions");
			}
			TokenString = tokenString;
			ExpirationTime = expirationTime;
			Permissions = permissions;
			UserId = userId;
			LastRefresh = lastRefresh;
		}

		public override string ToString()
		{
			return global::Facebook.Unity.Utilities.FormatToString(null, GetType().Name, new global::System.Collections.Generic.Dictionary<string, string>
			{
				{
					"ExpirationTime",
					ExpirationTime.TotalSeconds().ToString()
				},
				{
					"Permissions",
					Permissions.ToCommaSeparateList()
				},
				{
					"UserId",
					UserId.ToStringNullOk()
				},
				{
					"LastRefresh",
					LastRefresh.ToStringNullOk()
				}
			});
		}

		internal string ToJson()
		{
			global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
			dictionary[global::Facebook.Unity.LoginResult.PermissionsKey] = string.Join(",", global::System.Linq.Enumerable.ToArray(Permissions));
			dictionary[global::Facebook.Unity.LoginResult.ExpirationTimestampKey] = ExpirationTime.TotalSeconds().ToString();
			dictionary[global::Facebook.Unity.LoginResult.AccessTokenKey] = TokenString;
			dictionary[global::Facebook.Unity.LoginResult.UserIdKey] = UserId;
			if (LastRefresh.HasValue)
			{
				dictionary["last_refresh"] = LastRefresh.Value.TotalSeconds().ToString();
			}
			return global::Facebook.MiniJSON.Json.Serialize(dictionary);
		}
	}
}
